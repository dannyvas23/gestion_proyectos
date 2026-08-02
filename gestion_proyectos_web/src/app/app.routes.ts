import { Routes } from '@angular/router';

export const routes: Routes = [

// Rutas protegidas dentro del layout
  {
    path: '',
    loadComponent: () => import('./layout/layout.component').then(m => m.LayoutComponent),
    children: [
      {
        path: '',
        redirectTo: 'proyectos',
        pathMatch: 'full'
      },
      {
        path: 'proyectos',
        loadComponent: () => import('./features/proyectos/lista-proyectos.component').then(m => m.ListaProyectosComponent)
      },
      {
        path: 'tablero/:id',
        loadComponent: () => import('./features/tablero/tablero.component').then(m => m.TableroComponent)
      },
      {
        path: 'usuarios',
        loadComponent: () => import('./features/usuarios/lista-usuarios.component').then(m => m.ListaUsuariosComponent),
      }
    ]
  },

  // Ruta por defecto
  { path: '**', redirectTo: '' }

];
