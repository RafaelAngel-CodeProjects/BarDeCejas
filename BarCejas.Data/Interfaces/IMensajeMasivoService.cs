using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface IMensajeMasivoService
    {
        Task InsertMensajeMasivo(MensajeMasivo entity);
        Task<bool> UpdateMensajeMasivo(MensajeMasivo entity);
        Task<MensajeMasivo> GetMensajeMasivoById(int id);
        Task<IEnumerable<MensajeMasivo>> GetMensajeMasivoAll();
    }
}
