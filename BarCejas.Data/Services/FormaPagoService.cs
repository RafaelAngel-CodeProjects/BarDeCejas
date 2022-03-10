using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarCejas.Data.Services
{
    public class FormaPagoService : IFormaPagoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FormaPagoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<FormaPago> GetFormaPagoAll()
        {
            return _unitOfWork.formaPagoRepository.GetAll();
        }
    }
}
