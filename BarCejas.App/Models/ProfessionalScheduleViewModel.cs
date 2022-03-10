using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Models
{
    public class ProfessionalScheduleViewModel
    {
        #region Filters

        [DataType(DataType.Text)]
        public int BranchOfficeId { get; set; }

        [DataType(DataType.Text)]
        public int ProfessionalId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ServiceDate { get; set; }

        #endregion

        #region Service/Package

        [DataType(DataType.Text)]
        public int ServiceId { get; set; }

        [DataType(DataType.Text)]
        public int PackageId { get; set; }

        #endregion

        #region Professional Schedule
        public List<List<Entities.GetHorariosProfesionalResult>> ProfessionalSchedule { get; set; }

        #endregion


        #region DataLists
        public IEnumerable<BarCejas.Entities.Profesional> Professionals { get; set; }

        public IEnumerable<BarCejas.Entities.ContactoLocal> BranchOffices { get; set; }

        #endregion

        public int OrderId { get; set; }



    }
}