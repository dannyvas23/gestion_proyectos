import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  ProyectoDto,
  CrearProyectoPeticion,
  ActualizarProyectoPeticion,
  RespuestaPaginada
} from '../models';

@Injectable({
  providedIn: 'root'
})
export class ProyectoService {
  private readonly apiUrl = `${environment.apiUrl}/proyectos`;

  constructor(private http: HttpClient) {}

  obtenerPaginado(pagina: number, tamanio: number, nombre?: string): Observable<RespuestaPaginada<ProyectoDto>> {
    let params = new HttpParams()
      .set('pagina', pagina.toString())
      .set('tamanio', tamanio.toString());

    if (nombre) {
      params = params.set('nombre', nombre);
    }

    return this.http.get<RespuestaPaginada<ProyectoDto>>(this.apiUrl, { params });
  }

  obtenerPorId(id: string): Observable<ProyectoDto> {
    return this.http.get<ProyectoDto>(`${this.apiUrl}/${id}`);
  }

  crear(peticion: CrearProyectoPeticion): Observable<ProyectoDto> {
    return this.http.post<ProyectoDto>(this.apiUrl, peticion);
  }

  actualizar(id: string, peticion: ActualizarProyectoPeticion): Observable<ProyectoDto> {
    return this.http.put<ProyectoDto>(`${this.apiUrl}/${id}`, peticion);
  }

  eliminar(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
