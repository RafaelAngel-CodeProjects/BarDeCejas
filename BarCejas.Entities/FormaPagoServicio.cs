﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarCejas.Entities
{
    public partial class FormaPagoServicio
    {
        [Key]
        public int Id { get; set; }
        public long IdServicio { get; set; }
        public int IdFormaPago { get; set; }
        [Required]
        public bool? EsActivo { get; set; }

        [ForeignKey(nameof(IdFormaPago))]
        [InverseProperty(nameof(FormaPago.FormaPagoServicio))]
        public virtual FormaPago IdFormaPagoNavigation { get; set; }
        [ForeignKey(nameof(IdServicio))]
        [InverseProperty(nameof(Servicio.FormaPagoServicio))]
        public virtual Servicio IdServicioNavigation { get; set; }
    }
}