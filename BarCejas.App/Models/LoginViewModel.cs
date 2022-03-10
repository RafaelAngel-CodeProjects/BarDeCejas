using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Ingrese email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Ingrese contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
