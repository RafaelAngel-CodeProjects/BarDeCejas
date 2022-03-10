using BarCejas.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface IPaquetesService
    {
        Task<bool> InsertPaquete(Paquete entity);
        Task<bool> UpdatePaquete(Paquete entity);
        Task<Paquete> GetPaqueteById(long id);
        IEnumerable<Paquete> GetPaqueteAll();
        IEnumerable<Paquete> GetPaqueteAllClient();

        Task<IEnumerable<Paquete>> GetFullPaqueteAll();
       
        Task<bool> ChangeStatus(long id);
    }
}
