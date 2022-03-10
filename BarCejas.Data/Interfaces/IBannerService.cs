using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface IBannerService
    {
        Task InsertBanner(Banner entity);
        Task<bool> UpdateBanner(Banner entity);
        Task<Banner> GetBannerById(int id);
        IEnumerable<Banner> GetBannerAll();
        Task<IEnumerable<Banner>> GetBannerAllActive();
        Task<bool> UpdateOrdenBanner(List<Banner> lstEntity);
    }
}
