import { Component } from '@angular/core';
import { CommonModule, formatDate } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
// PrimeNG
import { MessageService, ConfirmationService } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { ToolbarModule } from 'primeng/toolbar';
import { PaginatorModule } from 'primeng/paginator';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { TagModule } from 'primeng/tag';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DropdownModule } from 'primeng/dropdown';
import { CalendarModule } from 'primeng/calendar';

import { CrearProyectoPeticion, ActualizarProyectoPeticion, EstadoProyecto, ProyectoDto, Prioridad, UsuarioDto } from '../../core/models';
import { ProyectoService } from '../../core/services/proyecto.service';
import { AuthService } from '../../core/services/auth.service';
import { ReporteService } from '../../core/services/reporte.service';
import { UsuarioService } from '../../core/services/usuario.service';

/**
 * Lista de proyectos con paginación backend y filtro por nombre.
 */

@Component({
  selector: 'app-lista-proyectos',
  standalone: true,
  imports: [

    TableModule,
    ButtonModule,
    CommonModule,
    InputTextModule,
    ToolbarModule,
    PaginatorModule,
    InputTextareaModule,
    TagModule,
    DialogModule,
    ToastModule,
    ConfirmDialogModule,
    DropdownModule,
    ReactiveFormsModule,
    CalendarModule,
  ],
  providers: [MessageService, ConfirmationService],
  template: `
    <div class="p-4">
      <p-toast></p-toast>
      <p-confirmDialog></p-confirmDialog>

    <div class="card">
      <p-toolbar styleClass="mb-3 gap-2">
        <ng-template pTemplate="start">
          <h2 class="m-0 text-900">
            <i class="pi pi-folder mr-2"></i>Proyectos
          </h2>
        </ng-template>

        <ng-template pTemplate="center">
          <!-- Filtro por nombre -->
          <span class="p-input-icon-left">
            <i class="pi pi-search"></i>
            <input pInputText
                   type="text"
                   placeholder="Buscar por nombre..."
                   (input)="filtrar($event)"
                   style="width: 300px;" />
          </span>
        </ng-template>

        <ng-template pTemplate="end">
          <p-button 
                    label="Nuevo Proyecto"
                    icon="pi pi-plus"
                    (onClick)="abrirNuevo()">
          </p-button>
        </ng-template>
      </p-toolbar>

      <!-- Tabla de proyectos -->
      <p-table [value]="proyectos"
               [lazy]="false"
               [rowHover]="true"
               styleClass="p-datatable-sm">
        <ng-template pTemplate="header">
          <tr>
            <th>Nombre</th>
            <th>Descripción</th>
            <th>Fecha Inicio</th>
            <th>Fin Previsto</th>
            <th>Estado</th>
            <th style="width: 200px;">Acciones</th>
          </tr>
        </ng-template>
        <ng-template pTemplate="body" let-proyecto>
          <tr>
            <td class="font-semibold">{{ proyecto.nombre }}</td>
            <td class="text-overflow-ellipsis" style="max-width: 250px;">{{ proyecto.descripcion }}</td>
            <td>{{ proyecto.fechaInicio | date:'dd/MM/yyyy' }}</td>
            <td>{{ proyecto.fechaFinPrevista ? (proyecto.fechaFinPrevista | date:'dd/MM/yyyy') : 'N/A' }}</td>
            <td>
              <p-tag [value]="obtenerNombreEstado(proyecto.estado)"
                     [severity]="obtenerSeveridadEstado(proyecto.estado)">
              </p-tag>
            </td>
            <td>
              <div class="flex gap-1">
                <p-button icon="pi pi-th-large"
                          [rounded]="true"
                          [text]="true"
                          severity="success"
                          pTooltip="Ver Tablero"
                          (onClick)="verTablero(proyecto)">
                </p-button>
                <p-button 
                          icon="pi pi-pencil"
                          [rounded]="true"
                          [text]="true"
                          severity="info"
                          pTooltip="Editar"
                          (onClick)="abrirEdicion(proyecto)">
                </p-button>
                <p-button 
                          *ngIf="esAdmin"
                          icon="pi pi-trash"
                          [rounded]="true"
                          [text]="true"
                          severity="danger"
                          pTooltip="Eliminar"
                          (onClick)="confirmarEliminar(proyecto)">
                </p-button>
                <p-button 
                          *ngIf="esAdmin"
                          icon="pi pi-file"
                          [rounded]="true"
                          [text]="true"
                          severity="secondary"
                          pTooltip="Reporte"
                          (onClick)="generarReporte(proyecto)">
                </p-button>
              </div>
            </td>
          </tr>
        </ng-template>
        <ng-template pTemplate="emptymessage">
          <tr>
            <td colspan="6" class="text-center p-4 text-500">No hay proyectos</td>
          </tr>
        </ng-template>
      </p-table>

      <p-paginator [rows]="tamanio"
                   [totalRecords]="total"
                   [rowsPerPageOptions]="[5, 10, 20]"
                   (onPageChange)="onPageChange($event)"
                   styleClass="mt-3">
      </p-paginator>
      
    </div>

    <!-- Diálogo crear/editar proyecto -->
    <p-dialog [header]="editando ? 'Editar Proyecto' : 'Nuevo Proyecto'"
              [(visible)]="dialogoVisible"
              [modal]="true"
              [style]="{ width: '550px' }">
      <form [formGroup]="formulario" class="flex flex-column gap-3 pt-3">
        <div class="flex flex-column gap-1">
          <label class="font-semibold">Nombre</label>
          <input pInputText formControlName="nombre" placeholder="Nombre del proyecto" />
        </div>

        <div class="flex flex-column gap-1">
          <label class="font-semibold">Descripción</label>
          <textarea pInputTextarea formControlName="descripcion" rows="3"
                    placeholder="Descripción del proyecto"></textarea>
        </div>        

        <div class="grid">
          <div class="col-6 flex flex-column gap-1">
            <label class="font-semibold">Fecha de Inicio</label>
            <p-calendar formControlName="fechaInicio" dateFormat="dd/mm/yy" [showIcon]="true" appendTo="body"></p-calendar>
          </div>
          <div class="col-6 flex flex-column gap-1">
            <label class="font-semibold">Fin Previsto</label>
            <p-calendar formControlName="fechaFinPrevista" dateFormat="dd/mm/yy" [showIcon]="true" appendTo="body"></p-calendar>
          </div>
        </div>

        <div class="flex flex-column gap-1" *ngIf="editando">
          <label class="font-semibold">Estado</label>
          <p-dropdown formControlName="estado"
                      [options]="estados"
                      optionLabel="label"
                      optionValue="value"
                      placeholder="Seleccione un estado"
                      appendTo="body">
          </p-dropdown>
        </div>
      </form>

      <ng-template pTemplate="footer">
        <p-button label="Cancelar" icon="pi pi-times" [text]="true" (onClick)="dialogoVisible = false"></p-button>
        <p-button label="Guardar" icon="pi pi-check" (onClick)="guardar()" [disabled]="formulario.invalid"></p-button>
      </ng-template>
    </p-dialog>

    <!-- Diálogo reporte -->
    <p-dialog header="Generar Reporte" [(visible)]="dialogoReporteVisible" [modal]="true" [style]="{ width: '400px' }">
      <div class="flex flex-column gap-3 pt-3">
        <div class="flex flex-column gap-1">
          <label class="font-semibold">Responsable (Opcional)</label>
          <p-dropdown [options]="usuarios" [(ngModel)]="reporteResponsableId"
                      optionLabel="nombre" optionValue="id"
                      placeholder="Todos" [showClear]="true" appendTo="body" [style]="{width: '100%'}">
          </p-dropdown>
        </div>
        <div class="flex flex-column gap-1">
          <label class="font-semibold">Prioridad (Opcional)</label>
          <p-dropdown [options]="prioridades" [(ngModel)]="reportePrioridad"
                      optionLabel="label" optionValue="value"
                      placeholder="Todas" [showClear]="true" appendTo="body" [style]="{width: '100%'}">
          </p-dropdown>
        </div>
        <div class="flex justify-content-between mt-3">
          <p-button label="PDF" icon="pi pi-file-pdf" severity="danger" (onClick)="descargarPdf()"></p-button>
          <p-button label="Excel" icon="pi pi-file-excel" severity="success" (onClick)="descargarExcel()"></p-button>
        </div>
      </div>
    </p-dialog>

      
    </div>
  `,
  styles: [`
    .card {
      background: #ffffff;
      border-radius: 12px;
      padding: 1.5rem;
      box-shadow: 0 2px 8px rgba(0,0,0,0.08);
    }
    .gap-1 {
      gap: 0.5rem;
    }
    .gap-2 {
      gap: 1rem;
    }
    .w-20rem {
      width: 20rem;
    }
    .overflow-x-auto {
      overflow-x: auto;
    }
    .field {
      margin-bottom: 1rem;
    }
    .field label {
      display: block;
      margin-bottom: 0.5rem;
    }
    .w-full {
      width: 100%;
    }
  `]
})
export class ListaProyectosComponent {

  constructor(
    private fb: FormBuilder,
    private proyectoService: ProyectoService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private authService: AuthService,
    private reporteService: ReporteService,
    private usuarioService: UsuarioService,
    private router: Router
  ) { }


  proyectos: ProyectoDto[] = [];
  total = 0;
  pagina = 1;
  tamanio = 10;
  filtroNombre = '';
  editando = false;
  proyectoEditandoId: string | null = null;

  formulario!: FormGroup;
  dialogoVisible = false;

  estados = [
    { label: 'Activo', value: EstadoProyecto.Activo },
    { label: 'Pausado', value: EstadoProyecto.Pausado },
    { label: 'Finalizado', value: EstadoProyecto.Finalizado }
  ];

  dialogoReporteVisible = false;
  reporteProyectoId: string | null = null;
  reporteResponsableId: string | null = null;
  reportePrioridad: Prioridad | null = null;
  usuarios: UsuarioDto[] = [];
  prioridades = [
    { label: 'Baja', value: Prioridad.Baja },
    { label: 'Media', value: Prioridad.Media },
    { label: 'Alta', value: Prioridad.Alta },
    { label: 'Crítica', value: Prioridad.Critica }
  ];

  ngOnInit(): void {
    this.formulario = this.fb.group({
      nombre: ['', []],
      descripcion: [''],
      fechaInicio: [new Date()],
      fechaFinPrevista: [null],
      estado: [EstadoProyecto.Activo]
    });
    this.cargarProyectos();
    this.cargarUsuarios();
  }

  cargarUsuarios(): void {
    this.usuarioService.obtenerTodos().subscribe({
      next: (u) => this.usuarios = u,
      error: () => console.error('Error al cargar usuarios')
    });
  }

  cargarProyectos(): void {
    this.proyectoService.obtenerPaginado(this.pagina, this.tamanio, this.filtroNombre || undefined).subscribe({
      next: (resp) => {
        console.log('Proyectos cargados:', resp);
        this.proyectos = resp.items;
        this.total = resp.total;
      },
      error: () => this.messageService.add({
        severity: 'error', summary: 'Error', detail: 'No se pudieron cargar los proyectos'
      })
    });
  }

  abrirNuevo(): void {
    this.editando = false;
    this.proyectoEditandoId = null;
    this.formulario.reset({ fechaInicio: new Date(), estado: EstadoProyecto.Activo });
    this.dialogoVisible = true;
  }

  verTablero(proyecto: ProyectoDto): void {
    this.router.navigate(['/tablero', proyecto.id]);
  }

  guardar(): void {
    const valores = this.formulario.value;

    if (this.editando && this.proyectoEditandoId) {
      const peticion: ActualizarProyectoPeticion = {
        nombre: valores.nombre,
        descripcion: valores.descripcion,
        fechaInicio: valores.fechaInicio?.toISOString() || new Date().toISOString(),
        fechaFinPrevista: valores.fechaFinPrevista?.toISOString() || null,
        estado: valores.estado
      };
      this.proyectoService.actualizar(this.proyectoEditandoId, peticion).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Éxito', detail: 'Proyecto actualizado' });
          this.dialogoVisible = false;
          this.cargarProyectos();
        },
        error: (err) => this.messageService.add({
          severity: 'error', summary: 'Error', detail: err.error?.error || 'Error al actualizar'
        })
      });
    } else {
      const peticion: CrearProyectoPeticion = {
        nombre: valores.nombre,
        descripcion: valores.descripcion,
        fechaInicio: valores.fechaInicio?.toISOString() || new Date().toISOString(),
        fechaFinPrevista: valores.fechaFinPrevista?.toISOString() || null
      };
      this.proyectoService.crear(peticion).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Éxito', detail: 'Proyecto creado' });
          this.dialogoVisible = false;
          this.cargarProyectos();
        },
        error: (err) => {
          this.messageService.add({
            severity: 'error', summary: 'Error', detail: err.status === 403 ? 'No tiene permisos para crear proyectos' : err.error?.error || 'Error al crear'
          })
        }
      });
    }
  }

  brirEdicion(proyecto: ProyectoDto): void {
    this.editando = true;
    this.proyectoEditandoId = proyecto.id;
    this.formulario.patchValue({
      nombre: proyecto.nombre,
      descripcion: proyecto.descripcion,
      fechaInicio: new Date(proyecto.fechaInicio),
      fechaFinPrevista: proyecto.fechaFinPrevista ? new Date(proyecto.fechaFinPrevista) : null,
      estado: proyecto.estado
    });
    this.dialogoVisible = true;
  }

  abrirEdicion(proyecto: ProyectoDto): void {
    this.editando = true;
    this.proyectoEditandoId = proyecto.id;
    this.formulario.patchValue({
      nombre: proyecto.nombre,
      descripcion: proyecto.descripcion,
      fechaInicio: new Date(proyecto.fechaInicio),
      fechaFinPrevista: proyecto.fechaFinPrevista ? new Date(proyecto.fechaFinPrevista) : null,
      estado: proyecto.estado
    });
    this.dialogoVisible = true;
  }

  confirmarEliminar(proyecto: ProyectoDto): void {
    this.confirmationService.confirm({
      message: `¿Está seguro de eliminar el proyecto "${proyecto.nombre}"?`,
      header: 'Confirmar eliminación',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.proyectoService.eliminar(proyecto.id).subscribe({
          next: () => {
            this.messageService.add({ severity: 'success', summary: 'Éxito', detail: 'Proyecto eliminado' });
            this.cargarProyectos();
          },
          error: () => this.messageService.add({
            severity: 'error', summary: 'Error', detail: 'No se pudo eliminar el proyecto'
          })
        });
      }
    });
  }

  generarReporte(proyecto: ProyectoDto): void {
    this.reporteProyectoId = proyecto.id;
    this.reporteResponsableId = null;
    this.reportePrioridad = null;
    this.dialogoReporteVisible = true;
  }

  descargarPdf(): void {
    if (!this.reporteProyectoId) return;
    this.reporteService.generarPdf(this.reporteProyectoId, this.reporteResponsableId, this.reportePrioridad)
      .subscribe({
        next: (blob) => {
          this.descargarArchivo(blob, `Reporte_${this.generarFechaActual()}.pdf`);
          this.dialogoReporteVisible = false;
        },
        error: () => this.messageService.add({ severity: 'error', summary: 'Error', detail: 'No se pudo generar PDF' })
      });
  }

  descargarExcel(): void {
    if (!this.reporteProyectoId) return;
    this.reporteService.generarExcel(this.reporteProyectoId, this.reporteResponsableId, this.reportePrioridad)
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

  filtrar(event: Event): void {
    this.filtroNombre = (event.target as HTMLInputElement).value;
    this.pagina = 1;
    this.cargarProyectos();
  }

  onPageChange(event: any): void {
    this.pagina = Math.floor(event.first / event.rows) + 1;
    this.tamanio = event.rows;
    this.cargarProyectos();
  }

  obtenerSeveridadEstado(estado: number): 'success' | 'info' | 'warning' | 'danger' {
    switch (estado) {
      case 1: return 'success';
      case 2: return 'warning';
      case 3: return 'info';
      default: return 'info';
    }
  }

  obtenerNombreEstado(estado: number): 'Activo' | 'Pausado' | 'Finalizado' | 'Desconocido' {
    switch (estado) {
      case 1: return 'Activo';
      case 2: return 'Pausado';
      case 3: return 'Finalizado';
      default: return 'Desconocido';
    }
  }

  get esAdmin(): boolean {
    return this.authService.esAdministrador();
  }
}
