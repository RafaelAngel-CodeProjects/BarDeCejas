using BarCejas.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Models
{
    public class PaymentMethodViewModel
    {
        #region Main Checkout Values

        [DataType(DataType.Text)]
        public long PackageId { get; set; }

        [DataType(DataType.Text)]
        public int ServiceId { get; set; }

        [DataType(DataType.Text)]
        public int BranchOfficeId { get; set; }

        [DataType(DataType.Text)]
        public int ProfessionalId { get; set; }

        public DateTime ServiceDate { get; set; }

        [DataType(DataType.Text)]
        public string HourService { get; set; }

        [DataType(DataType.Text)]
        public int OrderId { get; set; }

        [DataType(DataType.Text)]
        public int PaymentMethodId { get; set; }

        [DataType(DataType.Text)]
        public int PaymentMode { get; set; }

        #endregion

        #region Additional Data

        public decimal? TotalPrice { get; set; }
        public decimal? PromotionalPrice { get; set; }

        [Required]
        public IFormFile PaymentVoucherFile { get; set; }
        public string PaymentVoucherFilePath { get; set; }

        #endregion

    }
}
