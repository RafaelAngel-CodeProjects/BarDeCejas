using BarCejas.Entities;
using BarCejas.Entities.Enumerations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public int IdTipoGenero { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string Genero { get; set; }
        public string Registro { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las Contraseñas no coinciden, verifique por favor!")]
        public string ConfirmarPassword { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string FechaNacimiento { get; set; }
        public int IdTipoUsuario { get; set; }
        [Required]
        public string Dni { get; set; }
        public bool? EsActivo { get; set; }

        public string Avatar { get; set; }
    }
}
