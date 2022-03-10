using BarCejas.Entities;
using BarCejas.Entities.CustomEntities;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface INewnessService
    {
        Task<bool> InsertNewness(Novedades Newness);

        Task<bool> UpdateNewness(Novedades post);

        Task<Novedades> GetNewnessById(long id);


        public IEnumerable<Novedades> GetAll();

        public Task<bool> ChangeStatus(long Id, bool Estatus);

        public Task<bool> ActivateIndHome(long Id, bool pIndHome);

        public Task<IEnumerable<Novedades>> GetNewnessAllActive();

    }

}
