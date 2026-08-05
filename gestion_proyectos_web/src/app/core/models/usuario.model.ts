// Modelos de usuario — reflejan los DTOs del backend

export enum RolUsuario {
  Administrador = 1,
  Miembro = 2
}

export interface UsuarioDto {
  id: string;
  nombre: string;
  correoElectronico: string;
  rol: RolUsuario;
  activo: boolean;
}

export interface LoginPeticion {
  correoElectronico: string;
  password: string;
}

export interface RegistroPeticion {
  nombre: string;
  correoElectronico: string;
  password: string;
  rol: RolUsuario;
}

export interface LoginRespuesta {
  token: string;
  usuario: UsuarioDto;
}

export interface CrearUsuarioPeticion {
  nombre: string;
  correoElectronico: string;
  password: string;
  rol: RolUsuario;
}

export interface ActualizarUsuarioPeticion {
  nombre: string;
  correoElectronico: string;
  rol: RolUsuario;
  activo: boolean;
}
