using BarCejas.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Models
{
    public class TurnViewModel
    {
        public int Id { get; set; }
        public int IdOrdenItem { get; set; }
        public int IdEstatus { get; set; }
        public int IdEstatusPago { get; set; }
        public ModalidadPago ModalidadPago { get; set; }
        public FormaPago FormaPago { get; set; }
        public ContactoLocal ContactoLocal { get; set; }
        public Servicio Servicio { get; set; }
        public Profesional Profesional { get; set; }
        public string Hora { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaDeCita { get; set; }
    }
}
