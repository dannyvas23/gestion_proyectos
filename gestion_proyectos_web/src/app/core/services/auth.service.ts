import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import {
  LoginPeticion,
  LoginRespuesta,
  RegistroPeticion,
  UsuarioDto,
  RolUsuario
} from '../models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/auth`;
  private usuarioActualSubject = new BehaviorSubject<UsuarioDto | null>(null);
  public usuarioActual$ = this.usuarioActualSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) {
    // Restaurar sesión desde localStorage al iniciar
    const usuarioGuardado = localStorage.getItem('usuario');
    if (usuarioGuardado) {
      this.usuarioActualSubject.next(JSON.parse(usuarioGuardado));
    }
  }

  login(peticion: LoginPeticion): Observable<LoginRespuesta> {
    return this.http.post<LoginRespuesta>(`${this.apiUrl}/login`, peticion).pipe(
      tap(respuesta => this.guardarSesion(respuesta))
    );
  }

  registro(peticion: RegistroPeticion): Observable<LoginRespuesta> {
    return this.http.post<LoginRespuesta>(`${this.apiUrl}/registro`, peticion).pipe(
      tap(respuesta => this.guardarSesion(respuesta))
    );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('usuario');
    this.usuarioActualSubject.next(null);
    this.router.navigate(['/auth/login']);
  }

  obtenerToken(): string | null {
    return localStorage.getItem('token');
  }

  estaAutenticado(): boolean {
    const token = this.obtenerToken();
    if (!token) return false;

    // Verificar si el token no ha expirado
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.exp * 1000 > Date.now();
    } catch {
      return false;
    }
  }

  obtenerUsuarioActual(): UsuarioDto | null {
    return this.usuarioActualSubject.value;
  }

  obtenerNombre(): string{
    return this.usuarioActualSubject.value?.nombre || '';
  }

  esAdministrador(): boolean {
    const usuario = this.obtenerUsuarioActual();
    console.log('Usuario actual:', usuario);
    return usuario?.rol === RolUsuario.Administrador;
  }

  private guardarSesion(respuesta: LoginRespuesta): void {
    localStorage.setItem('token', respuesta.token);
    localStorage.setItem('usuario', JSON.stringify(respuesta.usuario));
    this.usuarioActualSubject.next(respuesta.usuario);
  }
}
