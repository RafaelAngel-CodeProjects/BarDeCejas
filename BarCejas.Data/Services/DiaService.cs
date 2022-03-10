using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using System.Collections.Generic;

namespace BarCejas.Data.Services
{
    public class DiaService : IDiaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DiaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Dia> GetDiasAll()
        {
            return _unitOfWork.diaRepository.GetAll();
        }
    }
}
