import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

/**
 * Sidebar de navegación estilo Sakai.
 * Muestra opciones de menú según el rol del usuario.
 *
 * Para defender:
 * - Los items del menú se filtran por rol: los de admin solo aparecen si el usuario es Administrador.
 * - routerLink: directiva de Angular que navega a una ruta sin recargar la página (SPA).
 * - routerLinkActive: agrega una clase CSS cuando la ruta del link coincide con la ruta actual.
 */
@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="layout-sidebar surface-card shadow-2"
         [class.layout-sidebar-visible]="visible"
         style="width: 260px; min-height: 100vh; transition: all 0.3s;">
      <!-- Logo / Título -->
      <div class="flex align-items-center justify-content-center px-4"
           style="height: 60px; border-bottom: 1px solid var(--surface-border);">
        <i class="pi pi-th-large text-primary text-2xl mr-2"></i>
        <span class="font-bold text-xl text-900">GestiónPro</span>
      </div>

      <!-- Menú -->
      <nav class="p-3">
        <ul class="list-none p-0 m-0">
          <li *ngFor="let item of menuItems" class="mb-1">
            <a [routerLink]="item.ruta"
               routerLinkActive="bg-primary text-white"
               [routerLinkActiveOptions]="{ exact: item.exacto }"
               class="flex align-items-center px-3 py-2 border-round text-700 hover:surface-hover cursor-pointer no-underline transition-colors transition-duration-200"
               (click)="cerrar.emit()">
              <i [class]="item.icono + ' mr-3 text-lg'"></i>
              <span class="font-medium">{{ item.etiqueta }}</span>
            </a>
          </li>
        </ul>
      </nav>
    </div>
  `,
  styles: [`
    .layout-sidebar {
      position: fixed;
      top: 0;
      left: 0;
      z-index: 999;
      overflow-y: auto;
    }
    @media (min-width: 992px) {
      .layout-sidebar {
        position: relative;
      }
    }
    @media (max-width: 991px) {
      .layout-sidebar {
        transform: translateX(-100%);
      }
      .layout-sidebar.layout-sidebar-visible {
        transform: translateX(0);
      }
    }
  `]
})
export class SidebarComponent {
  @Input() visible = true;
  @Output() cerrar = new EventEmitter<void>();


  get menuItems() {
    const items = [
      { etiqueta: 'Proyectos', icono: 'pi pi-folder', ruta: '/proyectos', exacto: false, soloAdmin: false },
      { etiqueta: 'Usuarios', icono: 'pi pi-users', ruta: '/usuarios', exacto: false, soloAdmin: true }
    ];
    return items;
  }
}
