import { TareaDto } from './tarea.model';

// Modelos de columna del tablero

export interface ColumnaDto {
  id: string;
  nombre: string;
  orden: number;
  proyectoId: string;
  activa: boolean;
  tareas: TareaDto[];
}

export interface CrearColumnaPeticion {
  nombre: string;
  proyectoId: string;
}

export interface ActualizarColumnaPeticion {
  nombre: string;
}

export interface ReordenarColumnasPeticion {
  proyectoId: string;
  columnasOrdenadas: string[];
}
