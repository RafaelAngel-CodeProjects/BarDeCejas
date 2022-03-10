using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Models
{
    public class PaymentMethodTransferViewModel : PaymentMethodViewModel
    {
        #region DataLists
        public IEnumerable<BarCejas.Entities.ModalidadPago> PaymentModes { get; set; }

        #endregion
    }
}
