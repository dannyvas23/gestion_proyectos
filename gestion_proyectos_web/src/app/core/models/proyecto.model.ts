// Modelos de proyecto

export enum EstadoProyecto {
  Activo = 1,
  Pausado = 2,
  Finalizado = 3
}

export interface ProyectoDto {
  id: string;
  nombre: string;
  descripcion: string;
  fechaInicio: string;
  fechaFinPrevista: string | null;
  estado: EstadoProyecto;
  activo: boolean;
}

export interface CrearProyectoPeticion {
  nombre: string;
  descripcion: string;
  fechaInicio: string;
  fechaFinPrevista: string | null;
}

export interface ActualizarProyectoPeticion {
  nombre: string;
  descripcion: string;
  fechaInicio: string;
  fechaFinPrevista: string | null;
  estado: EstadoProyecto;
}

export interface RespuestaPaginada<T> {
  items: T[];
  total: number;
  pagina: number;
  tamanio: number;
  totalPaginas: number;
}
