import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

// PrimeNG
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { ToolbarModule } from 'primeng/toolbar';
import { PaginatorModule } from 'primeng/paginator';
import { InputTextareaModule } from 'primeng/inputtextarea';
// IMPORTANTE: No uses el enum si no está definido
// import { EstadoProyecto } from '../../core/models'; // ← Coméntalo si da error
import { TagModule } from 'primeng/tag';

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
    TagModule
  ],
  template: `
    <div class="p-4">
      <!-- 
      <p-toast></p-toast>
      <p-confirmDialog></p-confirmDialog>-->

      <div class="card">
        <!-- Toolbar -->
        <div class="flex justify-content-between align-items-center mb-3">
          <h2 class="m-0">
            <i class="pi pi-folder mr-2"></i>Proyectos
          </h2>
          <div class="flex gap-2">
            <span class="p-input-icon-left">
              <i class="pi pi-search"></i>
              <input pInputText
                     type="text"
                     placeholder="Buscar por nombre..."
                     class="w-20rem" />
            </span>
            <p-button label="Nuevo Proyecto"
                      icon="pi pi-plus"
                      (onClick)="dialogoVisible = true">
            </p-button>
          </div>
        </div>

        <!-- Tabla de proyectos -->
        <div class="overflow-x-auto">
          <p-table [value]="proyectos"
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
                <td>{{ proyecto.descripcion }}</td>
                <td>{{ proyecto.fechaInicio | date:'dd/MM/yyyy' }}</td>
                <td>{{ proyecto.fechaFinPrevista ? (proyecto.fechaFinPrevista | date:'dd/MM/yyyy') : 'N/A' }}</td>
                <td>
                  <p-tag [value]="proyecto.estado"
                         [severity]="proyecto.severidad">
                  </p-tag>
                </td>
                <td>
                  <div class="flex gap-1">
                    <p-button icon="pi pi-th-large"
                              [rounded]="true"
                              [text]="true"
                              severity="success"
                              pTooltip="Ver Tablero">
                    </p-button>
                    <p-button icon="pi pi-pencil"
                              [rounded]="true"
                              [text]="true"
                              severity="info"
                              pTooltip="Editar">
                    </p-button>
                    <p-button icon="pi pi-trash"
                              [rounded]="true"
                              [text]="true"
                              severity="danger"
                              pTooltip="Eliminar">
                    </p-button>
                  </div>
                </td>
              </tr>
            </ng-template>
            <ng-template pTemplate="emptymessage">
              <tr>
                <td colspan="6" class="text-center p-4">
                  <i class="pi pi-inbox text-4xl text-400 mb-2"></i>
                  <p class="m-0">No hay proyectos disponibles</p>
                </td>
              </tr>
            </ng-template>
          </p-table>
        </div>

       
      </div>

      
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
  dialogoVisible = false;
  
  // Datos de ejemplo
  proyectos = [
    {
      id: 1,
      nombre: 'Sistema de Gestión',
      descripcion: 'Sistema integral para gestión de proyectos y tareas',
      fechaInicio: new Date('2024-01-15'),
      fechaFinPrevista: new Date('2024-06-30'),
      estado: 'Activo',
      severidad: 'success'
    },
    {
      id: 2,
      nombre: 'App Móvil',
      descripcion: 'Aplicación móvil para clientes',
      fechaInicio: new Date('2024-02-01'),
      fechaFinPrevista: new Date('2024-08-15'),
      estado: 'En Progreso',
      severidad: 'info'
    },
    {
      id: 3,
      nombre: 'Migración Cloud',
      descripcion: 'Migración de infraestructura a la nube',
      fechaInicio: new Date('2024-03-10'),
      fechaFinPrevista: new Date('2024-05-20'),
      estado: 'Pendiente',
      severidad: 'warning'
    },
    {
      id: 4,
      nombre: 'Dashboard Analytics',
      descripcion: 'Dashboard para análisis de datos en tiempo real',
      fechaInicio: new Date('2024-04-01'),
      fechaFinPrevista: new Date('2024-07-01'),
      estado: 'En Progreso',
      severidad: 'info'
    },
    {
      id: 5,
      nombre: 'Integración API',
      descripcion: 'Integración con APIs de terceros',
      fechaInicio: new Date('2024-05-01'),
      fechaFinPrevista: null,
      estado: 'Completado',
      severidad: 'success'
    },
    {
      id: 6,
      nombre: 'Sistema de Reporting',
      descripcion: 'Sistema de reportes y análisis de datos',
      fechaInicio: new Date('2024-06-01'),
      fechaFinPrevista: new Date('2024-09-30'),
      estado: 'Pendiente',
      severidad: 'warning'
    }
  ];

  // Opciones de estado para el dropdown
  estados = [
    { label: 'Activo', value: 'Activo' },
    { label: 'En Progreso', value: 'En Progreso' },
    { label: 'Pendiente', value: 'Pendiente' },
    { label: 'Completado', value: 'Completado' },
    { label: 'Cancelado', value: 'Cancelado' }
  ];
}