import { Routes } from '@angular/router';
import { AppLayoutComponent } from './layout/app.layout.component';
import { ListaProyectosComponent } from './features/proyectos/lista-proyectos.component';
//import { MiComponenteComponent } from './features/mi-componente/mi-componente.component';


export const routes: Routes = [
    {
        path: '',
        component: AppLayoutComponent,
        children: [
            { path: '', component: ListaProyectosComponent },
            //{ path: '', component: MiComponenteComponent },
        ]
    }
];
