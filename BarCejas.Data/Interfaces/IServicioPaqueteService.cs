using BarCejas.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface IServicioPaqueteService
    {
       
        Task<ServicioPaquete> GetServicioPaqueteById(long id);
        IEnumerable<ServicioPaquete> GetServicioPaqueteAll();
    
    }
}
