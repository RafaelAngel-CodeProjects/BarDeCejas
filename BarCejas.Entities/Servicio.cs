// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarCejas.Entities
{
    public partial class Servicio
    {
        public Servicio()
        {
            FormaPagoServicio = new HashSet<FormaPagoServicio>();
            ModalidadPagoServicio = new HashSet<ModalidadPagoServicio>();
            ServicioContactoLocal = new HashSet<ServicioContactoLocal>();
            ServicioPaquete = new HashSet<ServicioPaquete>();
            ServicioProfesional = new HashSet<ServicioProfesional>();
        }

        [Key]
        public long Id { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(100, ErrorMessage = "No puede contener más de 100 caracteres.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(500, ErrorMessage = "No puede contener más de 500 caracteres.")]
        public string DescripcionCorta { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(2000, ErrorMessage = "No puede contener más de 2000 caracteres.")]
        public string DescripcionLarga { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public int IdCategoria { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public int Duracion { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [Required(ErrorMessage = "Campo requerido.")]
        public decimal? PrecioLista { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Formato invalido")]
        public decimal? PrecioPromocioal { get; set; }
        [Range(0, 100, ErrorMessage ="Valor máximo permitido 100")]
        public int? Descuento { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FechaDescuentoInicio { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FechaDescuentoFin { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string RutaImagenTarjeta { get; set; }
        public string RutaImagenPaagina { get; set; }
        public string RutaVideoPagina { get; set; }
        [StringLength(500, ErrorMessage = "No puede contener más de 500 caracteres.")]
        public string RecomendacionesReserva { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string RutaFormulario { get; set; }
        public bool? EsActivo { get; set; }
        [Column(TypeName = "date")]
        public DateTime FechaCrecion { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FechaModificacion { get; set; }
        public int IdUsuarioAlta { get; set; }

        [ForeignKey(nameof(IdCategoria))]
        [InverseProperty(nameof(Categoria.Servicio))]
        public virtual Categoria IdCategoriaNavigation { get; set; }
        [ForeignKey(nameof(IdUsuarioAlta))]
        [InverseProperty(nameof(Usuario.Servicio))]
        public virtual Usuario IdUsuarioAltaNavigation { get; set; }
        [InverseProperty("IdServicioNavigation")]
        public virtual ICollection<FormaPagoServicio> FormaPagoServicio { get; set; }
        [InverseProperty("IdServicioNavigation")]
        public virtual ICollection<ModalidadPagoServicio> ModalidadPagoServicio { get; set; }
        [InverseProperty("IdServicioNavigation")]
        public virtual ICollection<ServicioContactoLocal> ServicioContactoLocal { get; set; }
        [InverseProperty("IdServicioNavigation")]
        public virtual ICollection<ServicioPaquete> ServicioPaquete { get; set; }
        [InverseProperty("IdServicioNavigation")]
        public virtual ICollection<ServicioProfesional> ServicioProfesional { get; set; }
    }
}