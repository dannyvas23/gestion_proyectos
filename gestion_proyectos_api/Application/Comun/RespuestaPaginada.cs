using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Comun
{
    /// <summary>
    /// Clase genérica para respuestas paginadas con metadatos de paginación.
    /// </summary>
    public class RespuestaPaginada<T>
    {
        public List<T> Items { get; set; } = new();
        public int Total { get; set; }
        public int Pagina { get; set; }
        public int Tamanio { get; set; }
        public int TotalPaginas => (int)Math.Ceiling((double)Total / Tamanio);
    }
}
