﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarCejas.Entities
{
    public partial class MediosContactoEmpresa
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(500)]
        public string LinkFacebook { get; set; }
        [Required]
        [StringLength(500)]
        public string LinkInstagram { get; set; }
        [Required]
        [Column("whatsapp")]
        [StringLength(20)]
        public string Whatsapp { get; set; }
    }
}