using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarCejas.Data.Interfaces
{
    public interface IModalidadPagoService
    {
        IEnumerable<ModalidadPago> GetModalidadPagoAll();
    }
}
