using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Services
{
    public class MensajeMasivoService: IMensajeMasivoService
    {

        private readonly IUnitOfWork _unitOfWork;

        public MensajeMasivoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MensajeMasivo>> GetMensajeMasivoAll()
        {
            var children = new string[] {
                "IdUsuarioAltaNavigation",
                "IdUsuarioModificacionNavigation",
                "RolTipoUsuarioMensajeMasivo"
            };

            return await _unitOfWork.mensajeMasivoRepository.GetByEagerLoad((x => !x.EsEliminado), children);
        }

        public async Task<MensajeMasivo> GetMensajeMasivoById(int id)
        {
            var children = new string[] {
                "IdUsuarioAltaNavigation",
                "IdUsuarioModificacionNavigation",
                "RolTipoUsuarioMensajeMasivo"
            };

            IEnumerable<MensajeMasivo> lmensaje = await _unitOfWork.mensajeMasivoRepository.GetByEagerLoad((x => !x.EsEliminado && x.Id == id), children);
            return lmensaje.First();

        }

        public async Task InsertMensajeMasivo(MensajeMasivo entity)
        {
            await _unitOfWork.mensajeMasivoRepository.Add(entity);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<bool> UpdateMensajeMasivo(MensajeMasivo entity)
        {
            _unitOfWork.mensajeMasivoRepository.Update(entity);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }

    }
}
