// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarCejas.Data.DataContext
{
    public partial class spGetReportePaqueteResult
    {
        public int IdOrden { get; set; }
        public string NombreProfesional { get; set; }
        public string NombrePaquete { get; set; }
        public string NombreCliente { get; set; }
        public DateTime Fecha { get; set; }
        public string NombreLocal { get; set; }
        public decimal Precio { get; set; }
        public int? MedioDePago { get; set; }
        public int FormaDePago { get; set; }
        public int EstadoTurno { get; set; }
        public int EstadoPago { get; set; }
    }
}
