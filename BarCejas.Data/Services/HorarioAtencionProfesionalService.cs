using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarCejas.Data.Services
{
    public class HorarioAtencionProfesionalService : IHorariosAtencionProfesionalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HorarioAtencionProfesionalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<HorarioAtencionProfesional> GetHorarioAtencionProfesionalsAll()
        {
            return _unitOfWork.horarioAtencionProfesionalRepository.GetAll();
        }
    }
}
