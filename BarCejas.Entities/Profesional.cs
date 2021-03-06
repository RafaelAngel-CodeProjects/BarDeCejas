// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarCejas.Entities
{
    public partial class Profesional
    {
        public Profesional()
        {
            HorarioAtencionProfesional = new HashSet<HorarioAtencionProfesional>();
            HorarioBloqueado = new HashSet<HorarioBloqueado>();
            ServicioProfesional = new HashSet<ServicioProfesional>();
        }

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(100, ErrorMessage = "No puede contener más de 100 caracteres.")]
        public string Descripcion { get; set; }
        public int IdUsuario { get; set; }
        public int IdContactoLocal { get; set; }
        public bool? EsActivo { get; set; }

        [ForeignKey(nameof(IdContactoLocal))]
        [InverseProperty(nameof(ContactoLocal.Profesional))]
        public virtual ContactoLocal IdContactoLocalNavigation { get; set; }
        [ForeignKey(nameof(IdUsuario))]
        [InverseProperty(nameof(Usuario.Profesional))]
        public virtual Usuario IdUsuarioNavigation { get; set; }
        [InverseProperty("IdProfesionalNavigation")]
        public virtual ICollection<HorarioAtencionProfesional> HorarioAtencionProfesional { get; set; }
        [InverseProperty("IdProfesionalNavigation")]
        public virtual ICollection<HorarioBloqueado> HorarioBloqueado { get; set; }
        [InverseProperty("IdProfesionalNavigation")]
        public virtual ICollection<ServicioProfesional> ServicioProfesional { get; set; }
    }
}