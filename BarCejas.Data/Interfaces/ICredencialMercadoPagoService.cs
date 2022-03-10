using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface ICredencialMercadoPagoService
    {
        IEnumerable<CredencialMercadoPago> GetCredencialMercadoPagoAll();
        Task<CredencialMercadoPago> GetCredencialMercadoPagoById(int id);
        Task InsertCredencialMercadoPago(CredencialMercadoPago entity);
        Task<bool> UpdateCredencialMercadoPago(CredencialMercadoPago entity);
    }
}
