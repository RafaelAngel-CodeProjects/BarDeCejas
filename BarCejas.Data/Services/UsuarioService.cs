using BarCejas.Data.Interfaces;
using BarCejas.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.Data.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Usuario> GetUsuarioByEmail(string email)
        {
            //var children = new string[] { "Profesional" };
            //var users = await _unitOfWork.usuarioRepository.GetByEagerLoad((x => x.Email == email), children);
            //if (users != null && users.Count() > 0)
            //    return users.FirstOrDefault();
            //return null;
            return await _unitOfWork.usuarioRepository.GetUsuarioByEmail(email);
        }

        public IEnumerable<Usuario> GetUsuarioAll()
        {
            return _unitOfWork.usuarioRepository.GetAll();
        }

        public IEnumerable<Usuario> GetUsuarioByTipo(int tipo)
        {
            return _unitOfWork.usuarioRepository.GetAll().Where(x => x.IdTipoUsuario == tipo);
        }

        public Task<Usuario> GetUsuarioById(int id)
        {
            return _unitOfWork.usuarioRepository.GetById(id);
        }

        public async Task<bool> InsertUsuario(Usuario entity)
        {
            try
            {
                var usuario = await _unitOfWork.usuarioRepository.GetUsuarioByEmail(entity.Email);

                if (usuario != null)
                    throw new Exception("El email ya se encuentra registrado");

                usuario = await _unitOfWork.usuarioRepository.GetUsuarioByDNI(string.Empty);
                if (usuario != null)
                    throw new Exception("El dni suministrado ya se encuentra registrado.");

                await _unitOfWork.usuarioRepository.Add(entity);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateUsuario(Usuario entity)
        {
            try
            {
                var usurio = await _unitOfWork.usuarioRepository.GetById(entity.Id);

                if (usurio is null)
                    throw new Exception("Usuarion no se encuentra registrado.");

                usurio.Nombre = entity.Nombre;
                usurio.Apellido = entity.Apellido;
                usurio.Dni = entity.Dni;
                usurio.FechaNacimiento = entity.FechaNacimiento;
                usurio.Genero = entity.Genero;
                //usurio.IdTipoUsuario = entity.IdTipoUsuario;
                usurio.EsActivo = entity.EsActivo;
                usurio.IdTipoGenero = entity.IdTipoGenero;
                usurio.Password = entity.Password;

                if (!string.IsNullOrEmpty(entity.Avatar))
                    usurio.Avatar = entity.Avatar;

                _unitOfWork.usuarioRepository.Update(usurio);
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
                var entity = await _unitOfWork.usuarioRepository.GetById(id);
                if (entity is null)
                    throw new Exception("Registro no encontrado");

                entity.EsActivo = !entity.EsActivo;
                _unitOfWork.usuarioRepository.Update(entity);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ChangePassword(int id, string password)
        {
            try
            {
                var entity = await _unitOfWork.usuarioRepository.GetById(id);
                if (entity is null)
                    throw new Exception("Registro no encontrado");

                entity.Password = password;
                _unitOfWork.usuarioRepository.Update(entity);
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
