using BarCejas.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface IMediosContactoEmpresaService
    {
        Task InsertMediosContactoEmpresa(MediosContactoEmpresa entity);
        Task<bool> UpdateMediosContactoEmpresa(MediosContactoEmpresa entity);
        Task<MediosContactoEmpresa> GetMediosContactoEmpresaById(int id);
        IEnumerable<MediosContactoEmpresa> GetMediosContactoEmpresaAll();
    }
}
