import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  UsuarioDto,
  CrearUsuarioPeticion,
  ActualizarUsuarioPeticion
} from '../models';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {
  private readonly apiUrl = `${environment.apiUrl}/usuarios`;

  constructor(private http: HttpClient) {}

  obtenerTodos(): Observable<UsuarioDto[]> {
    return this.http.get<UsuarioDto[]>(this.apiUrl);
  }

  obtenerPorId(id: string): Observable<UsuarioDto> {
    return this.http.get<UsuarioDto>(`${this.apiUrl}/${id}`);
  }

  crear(peticion: CrearUsuarioPeticion): Observable<UsuarioDto> {
    return this.http.post<UsuarioDto>(this.apiUrl, peticion);
  }

  actualizar(id: string, peticion: ActualizarUsuarioPeticion): Observable<UsuarioDto> {
    return this.http.put<UsuarioDto>(`${this.apiUrl}/${id}`, peticion);
  }
}
