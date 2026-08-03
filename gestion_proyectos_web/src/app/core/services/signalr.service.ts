import { Injectable, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { TareaDto, ColumnaDto } from '../models';

/**
 * Servicio de SignalR para comunicación en tiempo real.
 *
 * Para defender:
 * - HubConnectionBuilder: crea la conexión al Hub del backend.
 * - withUrl(url, { accessTokenFactory }): envía el JWT en el query string del handshake.
 * - withAutomaticReconnect(): reconecta automáticamente si se pierde la conexión.
 * - Subject<T>: emite eventos que los componentes pueden suscribirse con .subscribe().
 * - Al destruir el servicio o cambiar de tablero: se desuscribe del grupo y se detiene la conexión.
 */
@Injectable({
  providedIn: 'root'
})
export class SignalrService implements OnDestroy {
  private hubConnection: signalR.HubConnection | null = null;
  private proyectoIdActual: string | null = null;

  // Eventos que los componentes pueden suscribirse
  tareaMovida$ = new Subject<{ tarea: TareaDto; usuarioId: string }>();
  tareaCreada$ = new Subject<{ tarea: TareaDto; usuarioId: string }>();
  tareaActualizada$ = new Subject<{ tarea: TareaDto; usuarioId: string }>();
  tareaEliminada$ = new Subject<{ tareaId: string; usuarioId: string }>();
  columnaCreada$ = new Subject<{ columna: ColumnaDto; usuarioId: string }>();
  columnaActualizada$ = new Subject<{ columna: ColumnaDto; usuarioId: string }>();
  columnaEliminada$ = new Subject<{ columnaId: string; usuarioId: string }>();
  columnasReordenadas$ = new Subject<{ columnas: ColumnaDto[]; usuarioId: string }>();
  usuarioConectado$ = new Subject<{ usuarioId: string; nombre: string }>();
  usuarioDesconectado$ = new Subject<{ usuarioId: string; nombre: string }>();



  /**
   * Conectar al Hub y suscribirse a un tablero específico.
   */
  async conectar(proyectoId: string): Promise<void> {
    // Si ya hay una conexión a otro tablero, desconectar primero
    if (this.hubConnection && this.proyectoIdActual) {
      await this.desconectar();
    }

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.hubUrl, {
        accessTokenFactory: () => ''
      })
      .withAutomaticReconnect()
      .build();

    // Registrar listeners para cada tipo de evento
    this.hubConnection.on('TareaMovida', (tarea: TareaDto, usuarioId: string) => {
      this.tareaMovida$.next({ tarea, usuarioId });
    });

    this.hubConnection.on('TareaCreada', (tarea: TareaDto, usuarioId: string) => {
      this.tareaCreada$.next({ tarea, usuarioId });
    });

    this.hubConnection.on('TareaActualizada', (tarea: TareaDto, usuarioId: string) => {
      this.tareaActualizada$.next({ tarea, usuarioId });
    });

    this.hubConnection.on('TareaEliminada', (tareaId: string, usuarioId: string) => {
      this.tareaEliminada$.next({ tareaId, usuarioId });
    });

    this.hubConnection.on('ColumnaCreada', (columna: ColumnaDto, usuarioId: string) => {
      this.columnaCreada$.next({ columna, usuarioId });
    });

    this.hubConnection.on('ColumnaActualizada', (columna: ColumnaDto, usuarioId: string) => {
      this.columnaActualizada$.next({ columna, usuarioId });
    });

    this.hubConnection.on('ColumnaEliminada', (columnaId: string, usuarioId: string) => {
      this.columnaEliminada$.next({ columnaId, usuarioId });
    });

    this.hubConnection.on('ColumnasReordenadas', (columnas: ColumnaDto[], usuarioId: string) => {
      this.columnasReordenadas$.next({ columnas, usuarioId });
    });

    this.hubConnection.on('UsuarioConectado', (data: { usuarioId: string; nombre: string }) => {
      this.usuarioConectado$.next(data);
    });

    this.hubConnection.on('UsuarioDesconectado', (data: { usuarioId: string; nombre: string }) => {
      this.usuarioDesconectado$.next(data);
    });

    // Iniciar la conexión y suscribirse al tablero
    await this.hubConnection.start();
    await this.hubConnection.invoke('SuscribirTablero', proyectoId);
    this.proyectoIdActual = proyectoId;
  }

  /**
   * Desconectar del Hub y limpiar la suscripción al tablero.
   * Evita conexiones huérfanas.
   */
  async desconectar(): Promise<void> {
    if (this.hubConnection && this.proyectoIdActual) {
      try {
        await this.hubConnection.invoke('DesuscribirTablero', this.proyectoIdActual);
      } catch {
        // Ignorar errores al desuscribir si la conexión ya se perdió
      }
      await this.hubConnection.stop();
      this.hubConnection = null;
      this.proyectoIdActual = null;
    }
  }

  ngOnDestroy(): void {
    this.desconectar();
  }
}
