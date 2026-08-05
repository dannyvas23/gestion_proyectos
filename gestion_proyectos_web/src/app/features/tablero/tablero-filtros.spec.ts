import { TableroComponent } from './tablero.component';
import { TareaDto, Prioridad } from '../../core/models';

/**
 * TEST: Filtro por responsable reduce la lista de tareas visible.
 */
describe('TableroComponent - Filtros', () => {

  const tareasMock: TareaDto[] = [
    {
      id: '1', titulo: 'Tarea 1', descripcion: 'Descripción 1',
      prioridad: Prioridad.Alta, orden: 1000, fechaCreacion: '',
      columnaId: 'col1', responsableId: 'user1', responsableNombre: 'Juan'
    },
    {
      id: '2', titulo: 'Tarea 2', descripcion: 'Descripción 2',
      prioridad: Prioridad.Baja, orden: 2000, fechaCreacion: '',
      columnaId: 'col1', responsableId: 'user2', responsableNombre: 'María'
    },
    {
      id: '3', titulo: 'Tarea urgente', descripcion: 'Desc urgente',
      prioridad: Prioridad.Alta, orden: 3000, fechaCreacion: '',
      columnaId: 'col1', responsableId: 'user1', responsableNombre: 'Juan'
    }
  ];

  it('debe filtrar por responsable y retornar solo sus tareas', () => {
    //  crear instancia para probar getTareasFiltradas
    const component = Object.create(TableroComponent.prototype);
    component.filtroResponsable = 'user1';
    component.filtroPrioridad = null;
    component.filtroBusqueda = '';

    const resultado = component.getTareasFiltradas(tareasMock);
    expect(resultado.length).toBe(2);
    expect(resultado.every((t: TareaDto) => t.responsableId === 'user1')).toBeTrue();
  });

  it('debe filtrar por prioridad Alta y retornar solo las correspondientes', () => {
    const component = Object.create(TableroComponent.prototype);
    component.filtroResponsable = null;
    component.filtroPrioridad = Prioridad.Alta;
    component.filtroBusqueda = '';

    const resultado = component.getTareasFiltradas(tareasMock);

    expect(resultado.length).toBe(2);
    expect(resultado.every((t: TareaDto) => t.prioridad === Prioridad.Alta)).toBeTrue();
  });

  it('debe filtrar por búsqueda de texto en título', () => {
    const component = Object.create(TableroComponent.prototype);
    component.filtroResponsable = null;
    component.filtroPrioridad = null;
    component.filtroBusqueda = 'urgente';

    const resultado = component.getTareasFiltradas(tareasMock);

    expect(resultado.length).toBe(1);
    expect(resultado[0].titulo).toBe('Tarea urgente');
  });



});
