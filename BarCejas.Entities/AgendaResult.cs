using System;
using System.Collections.Generic;
using System.Text;

namespace BarCejas.Entities
{
    public class AgendaResult
    {
        public int IdOrden { get; set; }
        public int IdOrdenItem { get; set; }
        public int IdEstatus { get; set; }
        public int IdEstatusPago { get; set; }
        public string estatus { get; set; }
        public int IdFormaPago { get; set; }
        public string Hora { get; set; }
        public DateTime FechaDeCita { get; set; }
        public decimal? Monto { get; set; }
        public long IdServicio { get; set; }
        public string nombreServicio { get; set; }
        public int Duracion { get; set; }
        public string RutaImagenPaagina { get; set; }
        public string DescripcionCorta { get; set; }
        public int IdContacto { get; set; }
        public string Sede { get; set; }
        public string direccionSuc { get; set; }
        public int IdProfesional { get; set; }
        public string Profesional { get; set; }
        public string nombreCliente { get; set; }
        public string apellidoCliente { get; set; }
        public string modalidadPago { get; set; }
    }
}
