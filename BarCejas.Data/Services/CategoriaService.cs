using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.Data.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Categoria> GetCategoriaAll()
        {
            try
            {
                var children = new string[] { "Servicio" };
                IEnumerable<Categoria> model = _unitOfWork.categoriaRepository.GetByEagerLoad(children).Result;
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<Categoria> GetCategoriaAllClient()
        {
            try
            {
                var children = new string[] { "Servicio" };
                IEnumerable<Categoria> model = _unitOfWork.categoriaRepository.GetByEagerLoad((d => (bool)d.EsActivo), children).Result;
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Categoria GetCategoriaById(int id)
        {
            var children = new string[] { "Servicio" };
            IEnumerable<Categoria> model = _unitOfWork.categoriaRepository.GetByEagerLoad((d => d.Id == id), children).Result;
            return model.FirstOrDefault();
        }

        public async Task<bool> InsertCategoria(Categoria entity)
        {
            try
            {
                entity.EsActivo = true;
                await _unitOfWork.categoriaRepository.Add(entity);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateCategoria(Categoria entity)
        {
            try
            {
                _unitOfWork.categoriaRepository.Update(entity);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ChangeStatus(int id)
        {
            try
            {
                var entity = await _unitOfWork.categoriaRepository.GetById(id);
                if (entity is null)
                    throw new Exception("Registro no encontrado");

                entity.EsActivo = !entity.EsActivo;
                _unitOfWork.categoriaRepository.Update(entity);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
