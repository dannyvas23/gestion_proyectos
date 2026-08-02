import { Component } from '@angular/core';

@Component({
  selector: 'app-lista-proyectos',
  standalone: true,
  template: `
    <div class="surface-card shadow-2 border-round p-4">
      <h1 class="text-2xl text-900 m-0 mb-3">
        <i class="pi pi-folder mr-2 text-primary"></i>Proyectos
      </h1>
      <p class="text-600 m-0">Listado de proyectos (pendiente de implementar).</p>
    </div>
  `
})
export class ListaProyectosComponent {}
