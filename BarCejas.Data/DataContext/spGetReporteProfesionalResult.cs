﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarCejas.Data.DataContext
{
    public partial class spGetReporteProfesionalResult
    {
        public int IdOrden { get; set; }
        public string NombreProfesional { get; set; }
        public string NombreServicio { get; set; }
        public decimal? Precio { get; set; }
        public DateTime Fecha { get; set; }
    }
}
