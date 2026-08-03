import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Prioridad } from '../models';

/**
 * Servicio para descarga de reportes PDF y Excel.
 *
 * Para defender:
 * - responseType: 'blob' indica que la respuesta es binaria (archivo), no JSON.
 * - Se usa un <a> invisible con URL.createObjectURL para forzar la descarga del archivo
 *   con el nombre y tipo de contenido correctos.
 * - Los filtros (responsableId, prioridad) se aplican a los reportes para que el contenido
 *   del reporte coincida con lo que el usuario ve filtrado en el tablero.
 */
@Injectable({
  providedIn: 'root'
})
export class ReporteService {
  private readonly apiUrl = `${environment.apiUrl}/reportes`;

  constructor(private http: HttpClient) {}

  descargarPdf(proyectoId: string, responsableId?: string, prioridad?: Prioridad): void {
    const params = this.construirParams(responsableId, prioridad);
    this.http.get(`${this.apiUrl}/proyectos/${proyectoId}/pdf`, {
      params,
      responseType: 'blob'
    }).subscribe(blob => {
      this.descargarArchivo(blob, `Reporte_Proyecto.pdf`, 'application/pdf');
    });
  }

  descargarExcel(proyectoId: string, responsableId?: string, prioridad?: Prioridad): void {
    const params = this.construirParams(responsableId, prioridad);
    this.http.get(`${this.apiUrl}/proyectos/${proyectoId}/excel`, {
      params,
      responseType: 'blob'
    }).subscribe(blob => {
      this.descargarArchivo(blob, `Reporte_Proyecto.xlsx`,
        'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet');
    });
  }

  private construirParams(responsableId?: string, prioridad?: Prioridad): HttpParams {
    let params = new HttpParams();
    if (responsableId) params = params.set('responsableId', responsableId);
    if (prioridad) params = params.set('prioridad', prioridad);
    return params;
  }

  private descargarArchivo(blob: Blob, nombreArchivo: string, tipo: string): void {
    const url = window.URL.createObjectURL(new Blob([blob], { type: tipo }));
    const enlace = document.createElement('a');
    enlace.href = url;
    enlace.download = nombreArchivo;
    enlace.click();
    window.URL.revokeObjectURL(url);
  }
}
