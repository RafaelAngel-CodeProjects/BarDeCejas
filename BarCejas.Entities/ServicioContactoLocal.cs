﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarCejas.Entities
{
    public partial class ServicioContactoLocal
    {
        [Key]
        public int Id { get; set; }
        public int? IdContactoLocal { get; set; }
        public long? IdServicio { get; set; }
        [Required]
        public bool? EsActivo { get; set; }

        [ForeignKey(nameof(IdContactoLocal))]
        [InverseProperty(nameof(ContactoLocal.ServicioContactoLocal))]
        public virtual ContactoLocal IdContactoLocalNavigation { get; set; }
        [ForeignKey(nameof(IdServicio))]
        [InverseProperty(nameof(Servicio.ServicioContactoLocal))]
        public virtual Servicio IdServicioNavigation { get; set; }
    }
}