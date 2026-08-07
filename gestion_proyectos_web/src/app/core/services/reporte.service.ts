import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Prioridad } from '../models';

@Injectable({
  providedIn: 'root'
})
export class ReporteService {
  private readonly apiUrl = `${environment.apiUrl}/reportes/proyectos`;

  constructor(private http: HttpClient) {}

  generarPdf(proyectoId: string, responsableId?: string | null, prioridad?: Prioridad | null): Observable<Blob> {
    let params = new HttpParams();
    if (responsableId) params = params.set('responsableId', responsableId);
    if (prioridad !== null && prioridad !== undefined) params = params.set('prioridad', prioridad.toString());

    return this.http.get(`${this.apiUrl}/${proyectoId}/pdf`, { params, responseType: 'blob' });
  }

  generarExcel(proyectoId: string, responsableId?: string | null, prioridad?: Prioridad | null): Observable<Blob> {
    let params = new HttpParams();
    if (responsableId) params = params.set('responsableId', responsableId);
    if (prioridad !== null && prioridad !== undefined) params = params.set('prioridad', prioridad.toString());

    return this.http.get(`${this.apiUrl}/${proyectoId}/excel`, { params, responseType: 'blob' });
  }
}
