using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Models
{
    public class SuccessOrderViewModel
    {
        public long OrderId { get; set; }
        public string ServicePageImagePath { get; set; }
        public string ServiceName { get; set; }
        public string  ServiceDate { get; set; }
        public string  HourService { get; set; }
        public int  DurationService { get; set; }
        public string BranchOfficeAddress { get; set; }
        public string ProfessionalProfileImagePath { get; set; }
        public string ProfessionalDescription { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? PromotionalPrice { get; set; }              
                  
    }
}
