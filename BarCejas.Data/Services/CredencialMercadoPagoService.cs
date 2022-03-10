using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Services
{
    public class CredencialMercadoPagoService : ICredencialMercadoPagoService
    {

        private readonly IUnitOfWork _unitOfWork;

        public CredencialMercadoPagoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<CredencialMercadoPago> GetCredencialMercadoPagoAll()
        {
            return _unitOfWork.credencialMercadoPagoRepository.GetAll();
        }

        public async Task<CredencialMercadoPago> GetCredencialMercadoPagoById(int id)
        {
            return await _unitOfWork.credencialMercadoPagoRepository.GetById(id);
        }

        public async Task InsertCredencialMercadoPago(CredencialMercadoPago entity)
        {
            await _unitOfWork.credencialMercadoPagoRepository.Add(entity);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<bool> UpdateCredencialMercadoPago(CredencialMercadoPago entity)
        {
            _unitOfWork.credencialMercadoPagoRepository.Update(entity);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }

    }
}
