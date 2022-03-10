using BarCejas.Entities;
using System.Collections.Generic;

namespace BarCejas.Data.Interfaces
{
    public interface IFormaPagoService
    {
        IEnumerable<FormaPago> GetFormaPagoAll();
    }
}
