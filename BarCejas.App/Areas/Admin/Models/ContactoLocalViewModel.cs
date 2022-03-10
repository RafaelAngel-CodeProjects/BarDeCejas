using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Models
{
    public class ContactoLocalViewModel: ContactoLocal
    {
        public ContactoLocalViewModel()
        {
            this.Dia = new List<int>();
        }

        [Range(0,7, ErrorMessage = "Disculpe, los días seleccionados son inválidos")]
        [Required(ErrorMessage = "Debe seleccionar los días del horario de local.")]
        public List<int> Dia { get; set; }
    }
}
