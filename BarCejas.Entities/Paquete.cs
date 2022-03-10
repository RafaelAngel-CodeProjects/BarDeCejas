﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarCejas.Entities
{
    public partial class Paquete
    {
        public Paquete()
        {
            ServicioPaquete = new HashSet<ServicioPaquete>();
        }

        [Key]
        public long Id { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(100, ErrorMessage = "No puede contener más de 100 caracteres.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(100, ErrorMessage = "No puede contener más de 100 caracteres.")]
        public string DescripcionCorta { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(500, ErrorMessage = "No puede contener más de 500 caracteres.")]
        public string DescripcionLarga { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecioFinal { get; set; }
        public int? Descuento { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FechaVigenciaInicio { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FechaVigenciaFin { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string RutaImagenTarjeta { get; set; }
        public string RutaImagenHome { get; set; }
        public bool? EsActivo { get; set; }

        [InverseProperty("IdPaqueteNavigation")]
        public virtual ICollection<ServicioPaquete> ServicioPaquete { get; set; }
    }
}