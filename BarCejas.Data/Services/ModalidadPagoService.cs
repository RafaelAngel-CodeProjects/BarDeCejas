using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarCejas.Data.Services
{
    public class ModalidadPagoService : IModalidadPagoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ModalidadPagoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ModalidadPago> GetModalidadPagoAll()
        {
            return _unitOfWork.modalidadPagoRepository.GetAll();
        }
    }
}
