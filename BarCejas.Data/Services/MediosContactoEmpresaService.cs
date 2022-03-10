using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Services
{
    public class MediosContactoEmpresaService : IMediosContactoEmpresaService{
        
        private readonly IUnitOfWork _unitOfWork;

        public MediosContactoEmpresaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<MediosContactoEmpresa> GetMediosContactoEmpresaAll()
        {
            return _unitOfWork.mediosContactoEmpresaRepository.GetAll();
        }

        public async Task<MediosContactoEmpresa> GetMediosContactoEmpresaById(int id)
        {
            return await _unitOfWork.mediosContactoEmpresaRepository.GetById(id);
        }

        public async Task InsertMediosContactoEmpresa(MediosContactoEmpresa entity)
        {
            await _unitOfWork.mediosContactoEmpresaRepository.Add(entity);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<bool> UpdateMediosContactoEmpresa(MediosContactoEmpresa entity)
        {
            _unitOfWork.mediosContactoEmpresaRepository.Update(entity);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }
    }
}
