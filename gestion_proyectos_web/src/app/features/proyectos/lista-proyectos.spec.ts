import { ProyectoDto, EstadoProyecto } from '../../core/models';
/**
 * TEST para el DTO de proyecto
 */
describe('Test de prueba para DTO ProyectoDto', () => {

  const proyectos: ProyectoDto[] = [
    {
      id: '1',
      nombre: 'Proyecto A',
      descripcion: 'Descripción A',
      fechaInicio: '2026-08-04',
      fechaFinPrevista: null,
      estado: EstadoProyecto.Activo,
      activo: true
    },
    {
      id: '2',
      nombre: 'Proyecto B',
      descripcion: 'Descripción B',
      fechaInicio: '2026-08-05',
      fechaFinPrevista: '2026-12-31',
      estado: EstadoProyecto.Pausado,
      activo: true
    }
  ];
    it('Debe existir dos proyectos, uno activo y otro pausado', () => {
      expect(proyectos.length).toBe(2);
      expect(proyectos[0].id).toBe('1');
      expect(proyectos[1].id).toBe('2');
      expect(proyectos[0].estado).toBe(EstadoProyecto.Activo);
      expect(proyectos[1].estado).toBe(EstadoProyecto.Pausado);

    });

    it('Debe validarse la creación de el Proyecto B', () => {

      const proyectoB = proyectos.find(p => p.id === '2');
      expect(proyectoB).toBeDefined();
      expect(proyectoB?.nombre).toBe('Proyecto B');
      expect(proyectoB?.estado).toBe(EstadoProyecto.Pausado);
      
    });

    it('Debe permitirse actualizar la fecha de finalización para proyecto A', () => {
      const proyectoA = proyectos.find(p => p.id === '1');
      expect(proyectoA).toBeDefined();
      if (proyectoA) {
        proyectoA.fechaFinPrevista = '2026-12-31';
        expect(proyectoA.fechaFinPrevista).toBe('2026-12-31');
      }

      
    });

});







/**
 * TEST: Creación de proyecto
 */