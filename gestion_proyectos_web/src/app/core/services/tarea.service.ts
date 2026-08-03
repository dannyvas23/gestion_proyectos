import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  TareaDto,
  CrearTareaPeticion,
  ActualizarTareaPeticion,
  MoverTareaPeticion,
  Prioridad
} from '../models';

@Injectable({
  providedIn: 'root'
})
export class TareaService {
  private readonly apiUrl = `${environment.apiUrl}/tareas`;

  constructor(private http: HttpClient) {}

  obtenerPorProyecto(
    proyectoId: string,
    responsableId?: string,
    prioridad?: Prioridad,
    busqueda?: string
  ): Observable<TareaDto[]> {
    let params = new HttpParams();
    if (responsableId) params = params.set('responsableId', responsableId);
    if (prioridad) params = params.set('prioridad', prioridad);
    if (busqueda) params = params.set('busqueda', busqueda);

    return this.http.get<TareaDto[]>(`${this.apiUrl}/proyecto/${proyectoId}`, { params });
  }

  crear(peticion: CrearTareaPeticion, proyectoId: string): Observable<TareaDto> {
    const headers = new HttpHeaders().set('X-Proyecto-Id', proyectoId);
    return this.http.post<TareaDto>(this.apiUrl, peticion, { headers });
  }

  actualizar(id: string, peticion: ActualizarTareaPeticion, proyectoId: string): Observable<TareaDto> {
    const headers = new HttpHeaders().set('X-Proyecto-Id', proyectoId);
    return this.http.put<TareaDto>(`${this.apiUrl}/${id}`, peticion, { headers });
  }

  eliminar(id: string, proyectoId: string): Observable<void> {
    const headers = new HttpHeaders().set('X-Proyecto-Id', proyectoId);
    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers });
  }

  mover(peticion: MoverTareaPeticion, proyectoId: string): Observable<TareaDto> {
    const headers = new HttpHeaders().set('X-Proyecto-Id', proyectoId);
    return this.http.put<TareaDto>(`${this.apiUrl}/mover`, peticion, { headers });
  }
}
