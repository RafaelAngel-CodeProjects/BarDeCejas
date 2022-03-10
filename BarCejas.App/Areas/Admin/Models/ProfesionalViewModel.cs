using BarCejas.Entities;
using System.ComponentModel.DataAnnotations;

namespace BarCejas.App.Areas.Admin.Models
{
    public class ProfesionalViewModel : Profesional
    {
        [Required(ErrorMessage = "Campo requerido.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las Contraseñas no coinciden, verifique por favor!")]
        public string ConfirmarPassword { get; set; }
    }
}
