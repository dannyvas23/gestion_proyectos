import { Routes } from '@angular/router';
import { AppLayoutComponent } from './layout/app.layout.component';
import { ListaProyectosComponent } from './features/proyectos/lista-proyectos.component';
import { TableroComponent } from './features/tablero/tablero.component';
import { LoginComponent } from './core/auth/login.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    //Pagina inicial de login
    {
        path: 'auth/login',
        component: LoginComponent
    },

    // Rutas dentro del layout
    {
        path: '',
        component: AppLayoutComponent,
        canActivate: [authGuard], //Valida que el usuario esté autenticado antes de acceder a estas rutas
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
