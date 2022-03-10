using BarCejas.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface IContactoLocalService
    {
        Task InsertContactoLocal(ContactoLocal entity);
        Task<bool> UpdateContactoLocal(ContactoLocal entity);
        Task<ContactoLocal> GetContactoLocalById(int id);
        IEnumerable<ContactoLocal> GetContactoLocalAll();
        Task<IEnumerable<ContactoLocal>> GetContactoLocalAllActive();
        Task<bool> UpdateOrdenContactoLocal(List<ContactoLocal> lstEntity);
    }
}
