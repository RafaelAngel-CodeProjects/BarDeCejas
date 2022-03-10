﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarCejas.Entities
{
    public partial class HorarioAtencionProfesional
    {
        [Key]
        public int Id { get; set; }
        public int IdDia { get; set; }
        public int IdProfesional { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public bool? EsActivo { get; set; }

        [ForeignKey(nameof(IdDia))]
        [InverseProperty(nameof(Dia.HorarioAtencionProfesional))]
        public virtual Dia IdDiaNavigation { get; set; }
        [ForeignKey(nameof(IdProfesional))]
        [InverseProperty(nameof(Profesional.HorarioAtencionProfesional))]
        public virtual Profesional IdProfesionalNavigation { get; set; }
    }
}