using System;
using System.Collections.Generic;
using System.Text;

namespace BarCejas.Entities
{
    public class GetHorariosProfesionalResult
    {
        public string NombreServicio { get; set; }
        public int? DuracionServicio { get; set; }
        public string NombreSucursal { get; set; }
        public string DireccionSucursal { get; set; }
        public string NombreProfesional { get; set; }
        public string DescripcionProfesional { get; set; }
        public TimeSpan? HoraInicial { get; set; }
        public TimeSpan? HoraFinal { get; set; }
        public int? IdProfesional { get; set; }

    }
}
