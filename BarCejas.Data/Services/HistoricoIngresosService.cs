using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Services
{
    public class HistoricoIngresosService : IHistoricoIngresosService
    {

        private readonly IUnitOfWork _unitOfWork;

        public HistoricoIngresosService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task InsertHistoricoIngresos(HistoricoIngresos entity)
        {
            await _unitOfWork.historicoIngresosRepository.Add(entity);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<HistoricoIngresos> GetHistoricoIngresosById(int id)
        {
            return await _unitOfWork.historicoIngresosRepository.GetById(id);
        }

        public IEnumerable<HistoricoIngresos> GetHistoricoIngresosAll()
        {
            return _unitOfWork.historicoIngresosRepository.GetAll();
        }

    }
}
