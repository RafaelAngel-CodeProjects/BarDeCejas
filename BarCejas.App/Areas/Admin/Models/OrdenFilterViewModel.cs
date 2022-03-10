using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Models
{
    public class OrdenFilterViewModel
    {
        public int IdServicio { get; set; }
        public int IdProfesional { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int IdModalidadPago { get; set; }
        public int IdEstatusOrden { get; set; }
        public int IdEstatusPago { get; set; }
    }
}
