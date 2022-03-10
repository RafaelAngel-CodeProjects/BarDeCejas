﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarCejas.Entities
{
    public partial class OrdenItem
    {
        [Key]
        public int Id { get; set; }
        public int IdOrden { get; set; }
        public int IdServicio { get; set; }
        public int IdProfesional { get; set; }
        [Required]
        [StringLength(100)]
        public string Hora { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Monto { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FechaDeCita { get; set; }
        [ForeignKey(nameof(IdProfesional))]
        public virtual Profesional IdProfesionalNavigation { get; set; }
        [ForeignKey(nameof(IdOrden))]
        public virtual Orden IdOrdenNavigation { get; set; }
    }
}