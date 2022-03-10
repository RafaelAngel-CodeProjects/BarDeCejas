using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Services
{
    public class BannerService: IBannerService
    {

        private readonly IUnitOfWork _unitOfWork;

        public BannerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Banner> GetBannerAll()
        {
            return _unitOfWork.bannerRepository.GetAll().Where(x => !x.EsEliminado);
        }

        public async Task<IEnumerable<Banner>> GetBannerAllActive()
        {
            var children = new string[] { };
            
            IEnumerable<Banner> objs = await _unitOfWork.bannerRepository.GetByEagerLoad((x => !x.EsEliminado && x.EsActivo == true), children);
            return objs;
        }

        public async Task<Banner> GetBannerById(int id)
        {
            return await _unitOfWork.bannerRepository.GetById(id);
        }

        public async Task InsertBanner(Banner entity)
        {
            int nro = this.GetBannerAll().Count() + 1;
            entity.Orden = nro;
            await _unitOfWork.bannerRepository.Add(entity);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<bool> UpdateBanner(Banner entity)
        {
            _unitOfWork.bannerRepository.Update(entity);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }

        public async Task<bool> UpdateOrdenBanner(List<Banner> lstEntity)
        {
            int i = 1;
            Banner contact = new Banner();
            try
            {

                foreach (var item in lstEntity)
                {
                    contact = await _unitOfWork.bannerRepository.GetById(item.Id);
                    if (contact != null && contact.Id > 0)
                    {
                        contact.Orden = i;
                        contact.FechaModif = DateTime.UtcNow;
                        _unitOfWork.bannerRepository.Update(contact);
                        await _unitOfWork.SaveChangeAsync();
                        i++;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
