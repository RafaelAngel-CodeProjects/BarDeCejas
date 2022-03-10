using BarCejas.Entities;
using BarCejas.Entities.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace BarCejas.App.Areas.Admin.Models
{
    public class OrderitemViewModel: OrdenItem
    {
        public string nombreServicio { get; set; }
        public string descripcionServicio { get; set; }
        public string urlImagenServicio { get; set; }
        public string nombreProfesional { get; set; }
        public string nombreCliente { get; set; }
        public string apellidoCliente { get; set; }
        public string direccionSuc { get; set; }
        public string modalidadPago { get; set; }
        public int estatusPago { get; set; }

        public Servicio IdServicioNavigation { get; set; }
    }
}
