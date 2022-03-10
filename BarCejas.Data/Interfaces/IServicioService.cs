using BarCejas.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface IServicioService
    {
        Task<bool> InsertServicio(Servicio entity);
        Task<bool> UpdateServicio(Servicio entity);
        Task<Servicio> GetServicioById(long id);
        IEnumerable<Servicio> GetServicioAll();
        Task<bool> ChangeStatus(long id);
        IEnumerable<Servicio> GetServicioAllClient();
    }
}
