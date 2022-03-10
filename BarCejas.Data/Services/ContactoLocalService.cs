using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Services
{
    public class ContactoLocalService : IContactoLocalService
    {

        private readonly IUnitOfWork _unitOfWork;

        public ContactoLocalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ContactoLocal> GetContactoLocalAll()
        {
            return _unitOfWork.contactoLocalRepository.GetAll().Where(x => !x.EsEliminado);
        }

        public async Task<IEnumerable<ContactoLocal>> GetContactoLocalAllActive()
        {
            var children = new string[] { "HorarioAtencionLocal" };
            IEnumerable<ContactoLocal> contact = await _unitOfWork.contactoLocalRepository.GetByEagerLoad((x => !x.EsEliminado && x.EsActivo == true), children);
            return contact;
        }

        public async Task<ContactoLocal> GetContactoLocalById(int id)
        {
            var children = new string[] { "HorarioAtencionLocal" };
            IEnumerable<ContactoLocal> contact = await _unitOfWork.contactoLocalRepository.GetByEagerLoad((d => d.Id == id && !d.EsEliminado), children);
            return contact.First();
        }

        public async Task InsertContactoLocal(ContactoLocal entity)
        {
            int nro = this.GetContactoLocalAll().Count() + 1;
            entity.Orden = nro;
            await _unitOfWork.contactoLocalRepository.Add(entity);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<bool> UpdateContactoLocal(ContactoLocal entity)
        {
            _unitOfWork.contactoLocalRepository.Update(entity);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }

        public async Task<bool> UpdateOrdenContactoLocal(List<ContactoLocal> lstEntity)
        {
            int i = 1;
            ContactoLocal contact = new ContactoLocal();
            try
            {

                foreach (var item in lstEntity)
                {
                    contact = await _unitOfWork.contactoLocalRepository.GetById(item.Id);
                    if (contact != null && contact.Id > 0)
                    {
                        contact.Orden = i;
                        contact.FechaModif = DateTime.UtcNow;
                        _unitOfWork.contactoLocalRepository.Update(contact);
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
