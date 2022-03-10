using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Models
{
    public class PackageInfoViewModel : PaymentMethodViewModel
    {
        public Paquete Package { get; set; }

        public IEnumerable<OrdenItem> OrderItems { get; set; }

        public IEnumerable<Profesional> Profesionals { get; set; }
    }
}
