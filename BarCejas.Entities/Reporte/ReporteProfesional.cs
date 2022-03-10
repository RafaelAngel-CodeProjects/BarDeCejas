using System;
using System.Collections.Generic;
using System.Text;

namespace BarCejas.Entities.Reporte
{
    public class ReporteProfesional
    {
        public int IdOrden { get; set; }
        public string NombreProfesional { get; set; }
        public string NombreServicio { get; set; }
        public decimal? Precio { get; set; }
        public DateTime Fecha { get; set; }
    }
}
