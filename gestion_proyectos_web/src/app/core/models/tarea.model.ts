// Modelos de tarea

export enum Prioridad {
  Baja = 1,
  Media = 2,
  Alta = 3,
  Critica = 4
}

export interface TareaDto {
  id: string;
  titulo: string;
  descripcion: string;
  prioridad: Prioridad;
  orden: number;
  fechaCreacion: string;
  columnaId: string;
  responsableId: string | null;
  responsableNombre: string | null;
}

export interface CrearTareaPeticion {
  titulo: string;
  descripcion: string;
  prioridad: Prioridad;
  columnaId: string;
  responsableId: string | null;
}

export interface ActualizarTareaPeticion {
  titulo: string;
  descripcion: string;
  prioridad: Prioridad;
  responsableId: string | null;
}

export interface MoverTareaPeticion {
  tareaId: string;
  columnaDestinoId: string;
  nuevaPosicion: number;
}
