using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Models
{
    public class ProfessionalHoursViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        public int professionalId { get; set; }


        [DataType(DataType.Text)]
        public int sucursalId { get; set; }

        [DataType(DataType.Text)]
        public int serviceId { get; set; }

        [DataType(DataType.Text)]
        public int packageId { get; set; }


        public int orderId { get; set; }


        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime serviceDate { get; set; }

        public IEnumerable<BarCejas.Entities.Profesional> profesionals { get; set; }

        public IEnumerable<BarCejas.Entities.ContactoLocal> sucursales { get; set; }

        public IEnumerable<BlockedTurnsViewModel> blockedTurns { get; set; }
        public IEnumerable<BlockedHoursViewModel> blockedHours { get; set; }

    }
}