import { Routes } from '@angular/router';
import { AppLayoutComponent } from './layout/app.layout.component';
import { ListaProyectosComponent } from './features/proyectos/lista-proyectos.component';
import { TableroComponent } from './features/tablero/tablero.component';


export const routes: Routes = [


    // Rutas dentro del layout
    {
        path: '',
        component: AppLayoutComponent,
        children: [
            //{ path: '', component: ListaProyectosComponent },
            {
                path: '',
                redirectTo: 'proyectos',
                pathMatch: 'full',
            },
            {
                path: 'proyectos',
                component: ListaProyectosComponent
                //loadComponent: () => import('./features/proyectos/lista-proyectos.component').then(m => m.ListaProyectosComponent)
            },
            {
                path: 'tablero/:id',
                component: TableroComponent
                //loadComponent: () => import('./features/tablero/tablero.component').then(m => m.TableroComponent)
            },
        ]
    },


    // Ruta por defecto
    { path: '**', redirectTo: '' }
];
