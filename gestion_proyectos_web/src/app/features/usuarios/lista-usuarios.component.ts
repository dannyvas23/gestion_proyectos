import { Component } from '@angular/core';

@Component({
  selector: 'app-lista-usuarios',
  standalone: true,
  template: `
    <div class="surface-card shadow-2 border-round p-4">
      <h1 class="text-2xl text-900 m-0 mb-3">
        <i class="pi pi-users mr-2 text-primary"></i>Usuarios
      </h1>
      <p class="text-600 m-0">Listado de usuarios (pendiente de implementar).</p>
    </div>
  `
})
export class ListaUsuariosComponent {}
