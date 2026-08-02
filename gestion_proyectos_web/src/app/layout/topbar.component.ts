import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { MenuModule } from 'primeng/menu';
import { MenuItem } from 'primeng/api';

/**
 * Barra superior del layout.
 * Muestra el nombre del usuario, su rol y el botón de logout.
 */
@Component({
  selector: 'app-topbar',
  standalone: true,
  imports: [CommonModule, ButtonModule, MenuModule],
  template: `
    <div class="layout-topbar surface-card shadow-2 px-4 flex align-items-center justify-content-between"
         style="height: 60px;">
      <!-- Botón hamburguesa -->
      <p-button icon="pi pi-bars"
                [text]="true"
                [rounded]="true"
                (onClick)="toggleSidebar.emit()">
      </p-button>

      <!-- Info usuario -->
      <div class="flex align-items-center gap-3">
        <span class="text-600 text-sm">
          <i class="pi pi-user mr-1"></i>
          USUARIO
          <span class="ml-1 text-xs surface-200 border-round px-2 py-1">rol</span>
        </span>
        
      </div>
    </div>
  `
})
export class TopbarComponent {
  @Output() toggleSidebar = new EventEmitter<void>();

  
  cerrarSesion(): void {
    
  }
}
