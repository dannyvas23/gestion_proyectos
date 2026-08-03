import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  ColumnaDto,
  CrearColumnaPeticion,
  ActualizarColumnaPeticion,
  ReordenarColumnasPeticion
} from '../models';

@Injectable({
  providedIn: 'root'
})
export class ColumnaService {
  private readonly apiUrl = `${environment.apiUrl}/columnas`;

  constructor(private http: HttpClient) {}

  obtenerPorProyecto(proyectoId: string): Observable<ColumnaDto[]> {
    return this.http.get<ColumnaDto[]>(`${this.apiUrl}/proyecto/${proyectoId}`);
  }

  crear(peticion: CrearColumnaPeticion): Observable<ColumnaDto> {
    return this.http.post<ColumnaDto>(this.apiUrl, peticion);
  }

  actualizar(id: string, peticion: ActualizarColumnaPeticion): Observable<ColumnaDto> {
    return this.http.put<ColumnaDto>(`${this.apiUrl}/${id}`, peticion);
  }

  eliminar(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  reordenar(peticion: ReordenarColumnasPeticion): Observable<ColumnaDto[]> {
    return this.http.put<ColumnaDto[]>(`${this.apiUrl}/reordenar`, peticion);
  }
}
