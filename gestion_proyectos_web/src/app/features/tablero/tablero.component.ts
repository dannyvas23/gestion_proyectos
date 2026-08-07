import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule, formatDate } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import {
  CdkDragDrop,
  moveItemInArray,
  transferArrayItem
} from '@angular/cdk/drag-drop';
import { DragDropModule } from '@angular/cdk/drag-drop';

// Servicios
import { ColumnaService } from '../../core/services/columna.service';
import { TareaService } from '../../core/services/tarea.service';
import { ProyectoService } from '../../core/services/proyecto.service';
import { SignalrService } from '../../core/services/signalr.service';
import { UsuarioService } from '../../core/services/usuario.service';
import { ReporteService } from '../../core/services/reporte.service';
import { AuthService } from '../../core/services/auth.service';

// Modelos
import {
  ColumnaDto, CrearColumnaPeticion, ActualizarColumnaPeticion, ReordenarColumnasPeticion,
  TareaDto, CrearTareaPeticion, ActualizarTareaPeticion, MoverTareaPeticion, Prioridad,
  ProyectoDto, UsuarioDto
} from '../../core/models';

// PrimeNG
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { DropdownModule } from 'primeng/dropdown';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { TagModule } from 'primeng/tag';
import { TooltipModule } from 'primeng/tooltip';
import { AvatarModule } from 'primeng/avatar';
import { AvatarGroupModule } from 'primeng/avatargroup';
import { BadgeModule } from 'primeng/badge';
import { ChipModule } from 'primeng/chip';
import { MessageService, ConfirmationService } from 'primeng/api';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { OverlayPanelModule } from 'primeng/overlaypanel';

/**
 * Componente principal del Tablero Kanban.
 * - cdkDropListGroup: agrupa todos los cdkDropList para permitir arrastre entre ellos.
 * - cdkDropList: cada columna es una lista droppable.
 * - cdkDrag: cada tarjeta de tarea es arrastrable.
 * - (cdkDropListDropped): evento que se dispara al soltar una tarea.
 * - Actualización optimista: la UI se actualiza inmediatamente y si el backend falla, se revierte.
 */
@Component({
  selector: 'app-tablero',
  standalone: true,
  imports: [
    CommonModule, FormsModule, DragDropModule,
    ButtonModule, DialogModule, InputTextModule, InputTextareaModule,
    DropdownModule, ToastModule, ToolbarModule, TagModule, TooltipModule,
    AvatarModule, AvatarGroupModule, BadgeModule, ChipModule,
    ConfirmDialogModule, OverlayPanelModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './tablero.component.html',
  styleUrl: './tablero.component.scss'
})
export class TableroComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  proyectoId = '';
  proyecto: ProyectoDto | null = null;
  columnas: ColumnaDto[] = [];
  usuarios: UsuarioDto[] = [];
  usuariosConectados: { usuarioId: string; nombre: string }[] = [];

  // Filtros
  filtroResponsable: string | null = null;
  filtroPrioridad: Prioridad | null = null;
  filtroBusqueda = '';

  // Diálogos
  dialogoColumna = false;
  dialogoTarea = false;
  editandoColumna = false;
  editandoTarea = false;

  // Datos de formularios
  columnaEditId: string | null = null;
  columnaNombre = '';
  tareaEditId: string | null = null;
  tareaTitulo = '';
  tareaDescripcion = '';
  tareaPrioridad: Prioridad = Prioridad.Media;
  tareaResponsableId: string | null = null;
  tareaColumnaId = '';

  prioridades = [
    { label: 'Baja', value: Prioridad.Baja },
    { label: 'Media', value: Prioridad.Media },
    { label: 'Alta', value: Prioridad.Alta },
    { label: 'Crítica', value: Prioridad.Critica }
  ];

  dialogoReporteVisible = false;
  reporteResponsableId: string | null = null;
  reportePrioridad: Prioridad | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private columnaService: ColumnaService,
    private tareaService: TareaService,
    private proyectoService: ProyectoService,
    private signalrService: SignalrService,
    private usuarioService: UsuarioService,
    private reporteService: ReporteService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.proyectoId = this.route.snapshot.params['id'];
    this.cargarProyecto();
    this.cargarColumnas();
    this.cargarUsuarios();
    this.conectarSignalR();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.signalrService.desconectar();
  }

  cargarProyecto(): void {
    this.proyectoService.obtenerPorId(this.proyectoId).subscribe({
      next: (p) => {
        console.log('Proyecto cargado:', p);
        this.proyecto = p;
      }
    });
  }

  cargarColumnas(): void {
    this.columnaService.obtenerPorProyecto(this.proyectoId).subscribe({
      next: (cols) => {
        console.log('Columnas cargadas:', cols);
        this.columnas = cols;
      },
      error: () => {
        console.error('Error al cargar columnas');
        this.messageService.add({
          severity: 'error', summary: 'Error', detail: 'No se pudieron cargar las columnas'
        });
      }
    });
  }

  cargarUsuarios(): void {
    this.usuarioService.obtenerTodos().subscribe({
      next: (u) => {
        console.log('Usuarios cargados:', u);
        this.usuarios = u;
      },
      error: () => {
        console.error('Error al cargar usuarios');
      } // Silenciar error si no es admin
    });
  }


  getTareasFiltradas(tareas: TareaDto[]): TareaDto[] {
    let resultado = [...tareas];

    if (this.filtroResponsable) {
      resultado = resultado.filter(t => t.responsableId === this.filtroResponsable);
    }
    if (this.filtroPrioridad) {
      resultado = resultado.filter(t => t.prioridad === this.filtroPrioridad);
    }
    if (this.filtroBusqueda.trim()) {
      const busqueda = this.filtroBusqueda.toLowerCase();
      resultado = resultado.filter(t =>
        t.titulo.toLowerCase().includes(busqueda) ||
        t.descripcion.toLowerCase().includes(busqueda)
      );
    }

    return resultado;
  }

  limpiarFiltros(): void {
    this.filtroResponsable = null;
    this.filtroPrioridad = null;
    this.filtroBusqueda = '';
  }

  // ============================================================
  //  DRAG & DROP
  // ============================================================

  /**
   * Evento que se dispara al soltar una tarea (dentro de la misma columna o entre columnas).
   * - moveItemInArray: CDK utility que mueve un elemento dentro del mismo array.
   * - transferArrayItem: CDK utility que mueve un elemento de un array a otro.
   */
  onTareaDrop(event: CdkDragDrop<TareaDto[]>, columnaDestinoId: string): void {
    //const estadoPrevio = structuredClone(this.columnas);
    // Guardar estado para reversión
    const estadoPrevio = this.columnas.map(c => ({
      ...c,
      tareas: [...c.tareas]
    }));


    if (event.previousContainer === event.container) {
      // Mover dentro de la misma columna
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      // Mover entre columnas
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    }

    const tarea = event.container.data[event.currentIndex];
    const peticion: MoverTareaPeticion = {
      tareaId: tarea.id,
      columnaDestinoId: columnaDestinoId,
      nuevaPosicion: event.currentIndex
    };
    //console.log('Estado previo:', estadoPrevio);
    //console.log('Moviendo tarea:', peticion);
    this.tareaService.mover(peticion, this.proyectoId).subscribe({
      next: (tareaActualizada) => {
        // Actualizar la tarea con los datos del servidor
        const col = this.columnas.find(c => c.id === columnaDestinoId);
        if (col) {
          const idx = col.tareas.findIndex(t => t.id === tareaActualizada.id);
          if (idx >= 0) col.tareas[idx] = tareaActualizada;
        }
      },
      error: () => {
        // REVERSIÓN: restaurar estado anterior
        this.columnas = estadoPrevio;
        this.messageService.add({
          severity: 'error', summary: 'Error',
          detail: 'No se pudo mover la tarea. Se revirtió el cambio.'
        });
      }
    });
  }

  /**
   * Reordenar columnas por drag & drop.
   */
  onColumnaDrop(event: CdkDragDrop<ColumnaDto[]>): void {
    moveItemInArray(this.columnas, event.previousIndex, event.currentIndex);

    const peticion: ReordenarColumnasPeticion = {
      proyectoId: this.proyectoId,
      columnasOrdenadas: this.columnas.map(c => c.id)
    };

    const estadoPrevio = [...this.columnas];

    this.columnaService.reordenar(peticion).subscribe({
      error: () => {
        this.columnas = estadoPrevio;
        this.messageService.add({
          severity: 'error', summary: 'Error', detail: 'No se pudieron reordenar las columnas.'
        });
      }
    });
  }

  abrirNuevaColumna(): void {
    this.editandoColumna = false;
    this.columnaEditId = null;
    this.columnaNombre = '';
    this.dialogoColumna = true;
  }

  abrirEditarColumna(col: ColumnaDto): void {
    this.editandoColumna = true;
    this.columnaEditId = col.id;
    this.columnaNombre = col.nombre;
    this.dialogoColumna = true;
  }

  guardarColumna(): void {
    if (!this.columnaNombre.trim()) return;

    if (this.editandoColumna && this.columnaEditId) {
      const peticion: ActualizarColumnaPeticion = { nombre: this.columnaNombre };
      this.columnaService.actualizar(this.columnaEditId, peticion).subscribe({
        next: () => { this.dialogoColumna = false; this.cargarColumnas(); },
        error: (err) => this.messageService.add({
          severity: 'error', summary: 'Error', detail: err.error?.error || 'Error al actualizar columna'
        })
      });
    } else {
      const peticion: CrearColumnaPeticion = { nombre: this.columnaNombre, proyectoId: this.proyectoId };
      this.columnaService.crear(peticion).subscribe({
        next: () => { this.dialogoColumna = false; this.cargarColumnas(); },
        error: (err) => this.messageService.add({
          severity: 'error', summary: 'Error', detail: err.error?.error || 'Error al crear columna'
        })
      });
    }
  }

  eliminarColumna(col: ColumnaDto): void {
    this.confirmationService.confirm({
      message: `¿Eliminar la columna "${col.nombre}"?`,
      header: 'Confirmar',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.columnaService.eliminar(col.id).subscribe({
          next: () => {
            this.cargarColumnas();
            this.messageService.add({ severity: 'success', summary: 'Éxito', detail: 'Columna eliminada' });
          },
          error: (err) => this.messageService.add({
            severity: 'error', summary: 'Error', detail: err.error?.error || 'No se pudo eliminar'
          })
        });
      }
    });
  }

  abrirNuevaTarea(columnaId: string): void {
    this.editandoTarea = false;
    this.tareaEditId = null;
    this.tareaTitulo = '';
    this.tareaDescripcion = '';
    this.tareaPrioridad = Prioridad.Media;
    this.tareaResponsableId = null;
    this.tareaColumnaId = columnaId;
    this.dialogoTarea = true;
  }

  abrirEditarTarea(tarea: TareaDto): void {
    this.editandoTarea = true;
    this.tareaEditId = tarea.id;
    this.tareaTitulo = tarea.titulo;
    this.tareaDescripcion = tarea.descripcion;
    this.tareaPrioridad = tarea.prioridad;
    this.tareaResponsableId = tarea.responsableId;
    this.tareaColumnaId = tarea.columnaId;
    this.dialogoTarea = true;
  }

  guardarTarea(): void {
    if (!this.tareaTitulo.trim()) return;

    if (this.editandoTarea && this.tareaEditId) {
      const peticion: ActualizarTareaPeticion = {
        titulo: this.tareaTitulo,
        descripcion: this.tareaDescripcion,
        prioridad: this.tareaPrioridad,
        responsableId: this.tareaResponsableId
      };
      this.tareaService.actualizar(this.tareaEditId, peticion, this.proyectoId).subscribe({
        next: () => { this.dialogoTarea = false; this.cargarColumnas(); },
        error: (err) => this.messageService.add({
          severity: 'error', summary: 'Error', detail: err.error?.error || 'Error al actualizar tarea'
        })
      });
    } else {
      const peticion: CrearTareaPeticion = {
        titulo: this.tareaTitulo,
        descripcion: this.tareaDescripcion,
        prioridad: this.tareaPrioridad,
        columnaId: this.tareaColumnaId,
        responsableId: this.tareaResponsableId
      };
      this.tareaService.crear(peticion, this.proyectoId).subscribe({
        next: () => { this.dialogoTarea = false; this.cargarColumnas(); },
        error: (err) => this.messageService.add({
          severity: 'error', summary: 'Error', detail: err.error?.error || 'Error al crear tarea'
        })
      });
    }
  }

  eliminarTarea(tarea: TareaDto): void {
    this.confirmationService.confirm({
      message: `¿Eliminar la tarea "${tarea.titulo}"?`,
      header: 'Confirmar',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.tareaService.eliminar(tarea.id, this.proyectoId).subscribe({
          next: () => this.cargarColumnas(),
          error: () => this.messageService.add({
            severity: 'error', summary: 'Error', detail: 'No se pudo eliminar la tarea'
          })
        });
      }
    });
  }

  //  SIGNALR — Tiempo real
  private conectarSignalR(): void {
    this.signalrService.conectar(this.proyectoId).then(() => {
      // Escuchar eventos de otros usuarios
      this.signalrService.tareaCreada$.pipe(takeUntil(this.destroy$)).subscribe(({ tarea, usuarioId }) => {
        this.cargarColumnas();
      });

      this.signalrService.tareaActualizada$.pipe(takeUntil(this.destroy$)).subscribe(({ tarea, usuarioId }) => {
        this.cargarColumnas();
      });

      this.signalrService.tareaMovida$.pipe(takeUntil(this.destroy$)).subscribe(({ tarea, usuarioId }) => {
        this.cargarColumnas();
      });

      this.signalrService.tareaEliminada$.pipe(takeUntil(this.destroy$)).subscribe(({ tareaId, usuarioId }) => {
        this.cargarColumnas();
      });

      this.signalrService.columnaCreada$.pipe(takeUntil(this.destroy$)).subscribe(({ columna, usuarioId }) => {
        this.cargarColumnas();
      });

      this.signalrService.columnaActualizada$.pipe(takeUntil(this.destroy$)).subscribe(({ columna, usuarioId }) => {
        this.cargarColumnas();
      });

      this.signalrService.columnaEliminada$.pipe(takeUntil(this.destroy$)).subscribe(({ columnaId, usuarioId }) => {
        this.cargarColumnas();
      });

      this.signalrService.columnasReordenadas$.pipe(takeUntil(this.destroy$)).subscribe(({ columnas, usuarioId }) => {
        this.cargarColumnas();
      });

      // Indicador de usuarios conectados
      this.signalrService.usuarioConectado$.pipe(takeUntil(this.destroy$)).subscribe((data) => {
        if (!this.usuariosConectados.find(u => u.usuarioId === data.usuarioId)) {
          this.usuariosConectados.push(data);
        }
      });

      this.signalrService.usuarioDesconectado$.pipe(takeUntil(this.destroy$)).subscribe((data) => {
        this.usuariosConectados = this.usuariosConectados.filter(u => u.usuarioId !== data.usuarioId);
      });
    }).catch(() => {
      this.messageService.add({
        severity: 'warn', summary: 'Aviso', detail: 'No se pudo conectar al tiempo real'
      });
    });
  }

  obtenerSeveridadPrioridad(prioridad: number): 'success' | 'info' | 'warning' | 'danger' {
    switch (prioridad) {
      case 1: return 'success';
      case 2: return 'info';
      case 3: return 'warning';
      case 4: return 'danger';
      default: return 'info';
    }
  }

  obtenerNombrePrioridad(prioridad: number): 'Baja' | 'Media' | 'Alta' | 'Crítica' | 'Desconocida' {
    switch (prioridad) {
      case 1: return 'Baja';
      case 2: return 'Media';
      case 3: return 'Alta';
      case 4: return 'Crítica';
      default: return 'Desconocida';
    }
  }

  obtenerIniciales(nombre: string | null): string {
    if (!nombre) return '?';
    return nombre.split(' ').map(n => n[0]).join('').toUpperCase().substring(0, 2);
  }

  volver(): void {
    this.router.navigate(['/proyectos']);
  }

  get listaIdsColumnas(): string[] {
    return this.columnas.map(c => 'col-' + c.id);
  }

  trackByColumna(index: number, col: ColumnaDto): string { return col.id; }
  trackByTarea(index: number, tarea: TareaDto): string { return tarea.id; }

  generarReporte(): void {
    this.reporteResponsableId = null;
    this.reportePrioridad = null;
    this.dialogoReporteVisible = true;
  }

  descargarPdf(): void {
    if (!this.proyectoId) return;
    this.reporteService.generarPdf(this.proyectoId, this.reporteResponsableId, this.reportePrioridad)
      .subscribe({
        next: (blob) => {
          this.descargarArchivo(blob, `Reporte_${this.generarFechaActual()}.pdf`);
          this.dialogoReporteVisible = false;
        },
        error: () => this.messageService.add({ severity: 'error', summary: 'Error', detail: 'No se pudo generar PDF' })
      });
  }

  descargarExcel(): void {
    if (!this.proyectoId) return;
    this.reporteService.generarExcel(this.proyectoId, this.reporteResponsableId, this.reportePrioridad)
      .subscribe({
        next: (blob) => {
          this.descargarArchivo(blob, `Reporte_${this.generarFechaActual()}.xlsx`);
          this.dialogoReporteVisible = false;
        },
        error: () => this.messageService.add({ severity: 'error', summary: 'Error', detail: 'No se pudo generar Excel' })
      });
  }

  generarFechaActual(): string {
      return formatDate(new Date(), 'yyyyMMdd_HHmm', 'en-US');
  }

  get esAdmin(): boolean {
    return this.authService.esAdministrador();
  }

  private descargarArchivo(blob: Blob, nombre: string): void {
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = nombre;
    document.body.appendChild(a);
    a.click();
    window.URL.revokeObjectURL(url);
    document.body.removeChild(a);
  }
}




