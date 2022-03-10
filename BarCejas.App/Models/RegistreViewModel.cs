using BarCejas.Entities;
using BarCejas.Entities.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace BarCejas.App.Models
{
    public class RegistreViewModel: Usuario
    {
        [Required(ErrorMessage = "Ingrese email")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Debe ingresar una contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirme contraseña")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmarPassword { get; set; }
        [Required(ErrorMessage = "Ingrese teléfono")]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }
        [Required]
        public int IdTipoGenero { get; set; }
        
        public string Genero { get; set; }
        public string Registro { get; set; }

        public bool LoginOrRegistre { get; set; }

        public string Avatar { get; set; }
    }
}
