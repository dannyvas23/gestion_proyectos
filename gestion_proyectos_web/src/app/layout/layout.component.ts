import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SidebarComponent } from './sidebar.component';
import { TopbarComponent } from './topbar.component';

/**
 * Layout principal estilo Sakai.
 *
 * Para defender:
 * - Estructura de 3 zonas: sidebar (menú lateral), topbar (barra superior), y contenido.
 * - <router-outlet> renderiza el componente correspondiente a la ruta actual.
 * - La clase 'layout-sidebar-active' controla si el sidebar está abierto o cerrado (responsive).
 */
@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, RouterModule, SidebarComponent, TopbarComponent],
  template: `
    <div class="layout-wrapper" [class.layout-sidebar-active]="sidebarVisible">
      <!-- Overlay para cerrar sidebar en móvil -->
      <div class="layout-sidebar-overlay" *ngIf="sidebarVisible" (click)="sidebarVisible = false"></div>

      <!-- Sidebar 
      <app-sidebar [visible]="sidebarVisible" (cerrar)="sidebarVisible = false"></app-sidebar>

      <!-- Contenedor principal -->
      <div class="layout-main-container">
        <app-topbar></app-topbar>
        <div class="layout-main p-4">
          <router-outlet></router-outlet>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .layout-wrapper {
      display: flex;
      min-height: 100vh;
    }
    .layout-main-container {
      flex: 1;
      display: flex;
      flex-direction: column;
      min-width: 0;
    }
    .layout-main {
      flex: 1;
      background-color: #f8f9fa;
    }
    .layout-sidebar-overlay {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background: rgba(0,0,0,0.4);
      z-index: 998;
    }
    @media (min-width: 992px) {
      .layout-sidebar-overlay {
        display: none;
      }
    }
  `]
})
export class LayoutComponent {
  sidebarVisible = true;
}
