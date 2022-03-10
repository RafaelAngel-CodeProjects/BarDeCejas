using System;
using System.Collections.Generic;
using System.Text;

namespace BarCejas.Entities
{
    public class HistoricoOrdenesCliente
    {
        public int IdOrden { get; set; }
        public int IdEstatus { get; set; }
        public int IdEstatusPago { get; set; }
        public int? IdModalidadPago { get; set; }
        public int IdFormaPago { get; set; }
        public int IdOrdenItem { get; set; }
        public string Hora { get; set; }
        public DateTime FechaDeCita { get; set; }
        public decimal? Monto { get; set; }
        public long IdServicio { get; set; }
        public string Servicio { get; set; }
        public int Duracion { get; set; }
        public string RutaImagenPaagina { get; set; }
        public string RutaImagenTarjeta { get; set; }
        public string RutaFormulario { get; set; }
        public int IdContacto { get; set; }
        public string Sede { get; set; }
        public string Direccion { get; set; }
        public int IdProfesional { get; set; }
        public string Profesional { get; set; }
        public int IdCliente { get; set; }
        public string Cliente { get; set; }
    }
}
