using BarCejas.Data.Interfaces;
using BarCejas.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.Data.Services
{
    public class ProfesionalService : IProfesionalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfesionalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Profesional>> GetProfesionalAll()
        {
            var children = new string[] { "IdUsuarioNavigation", "IdContactoLocalNavigation", "ServicioProfesional.IdServicioNavigation" };
            IEnumerable<Profesional> contact = await _unitOfWork.profesionalRepository.GetByEagerLoad(children);
            return contact;
        }

        public async Task<IEnumerable<Profesional>> GetProfesionalAllActive()
        {
            var children = new string[] { "IdUsuarioNavigation" };
            IEnumerable<Profesional> contact = await _unitOfWork.profesionalRepository.GetByEagerLoad((x => (bool)x.EsActivo), children);
            return contact;
        }

        public async Task<Profesional> GetProfesionalByIdUsuario(int id)
        {
            IEnumerable<Profesional> professional = await _unitOfWork.profesionalRepository.GetByEagerLoad((x => x.IdUsuario == id));
            return professional.FirstOrDefault();
        }

        public async Task<Profesional> GetProfesionalById(int id)
        {
            var children = new string[] { "IdUsuarioNavigation", "HorarioAtencionProfesional", "IdContactoLocalNavigation", "ServicioProfesional" };
            IEnumerable<Profesional> contact = await _unitOfWork.profesionalRepository.GetByEagerLoad((d => d.Id == id), children);
            return contact.First();
        }

        public async Task<bool> InsertProfesional(Profesional entity)
        {
            try
            {
                var usuario = await _unitOfWork.usuarioRepository.GetUsuarioByEmail(entity.IdUsuarioNavigation.Email);
                if (usuario != null)
                    throw new Exception("El email suministrado ya se encuentra registrado.");

                usuario = await _unitOfWork.usuarioRepository.GetUsuarioByDNI(entity.IdUsuarioNavigation.Dni);
                if (usuario != null)
                    throw new Exception("El dno suministrado ya se encuentra registrado.");

                entity.EsActivo = true;
                entity.IdContactoLocalNavigation = null;
                entity.IdUsuarioNavigation.EsActivo = true;
                await _unitOfWork.profesionalRepository.Add(entity);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateProfesional(Profesional entity)
        {
            try
            {
                Profesional model = await _unitOfWork.profesionalRepository.GetById(entity.Id);

                #region Asignacion de modelo
                model.Descripcion = entity.Descripcion;
                model.EsActivo = entity.EsActivo;
                model.IdContactoLocal = entity.IdContactoLocal;
                model.ServicioProfesional = entity.ServicioProfesional;
                model.IdUsuarioNavigation.Nombre = entity.IdUsuarioNavigation.Nombre;
                model.IdUsuarioNavigation.Apellido = entity.IdUsuarioNavigation.Apellido;
                model.IdUsuarioNavigation.FechaNacimiento = entity.IdUsuarioNavigation.FechaNacimiento;
                model.IdUsuarioNavigation.Password = entity.IdUsuarioNavigation.Password;
                model.IdUsuarioNavigation.Dni = entity.IdUsuarioNavigation.Dni;
                model.IdUsuarioNavigation.Telefono = entity.IdUsuarioNavigation.Telefono;
                model.IdUsuarioNavigation.Genero = entity.IdUsuarioNavigation.Genero;
                model.IdUsuarioNavigation.IdTipoGenero = entity.IdUsuarioNavigation.IdTipoGenero;
                #endregion

                _unitOfWork.profesionalRepository.Update(model);
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
                var entity = await _unitOfWork.profesionalRepository.GetById(id);
                if (entity is null)
                    throw new Exception("Registro no encontrado");

                entity.EsActivo = !entity.EsActivo;
                _unitOfWork.profesionalRepository.Update(entity);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> InsertUpdateHorarioProfesional(Profesional entity)
        {
            try
            {
                var newEntity = await _unitOfWork.profesionalRepository.GetById(entity.Id);
                if (newEntity is null)
                    throw new Exception("Registro no encontrado");

                newEntity.HorarioAtencionProfesional = entity.HorarioAtencionProfesional;
                _unitOfWork.profesionalRepository.Update(newEntity);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<HorarioBloqueado>> GetBlockScheduleBySchedule(DateTime serviceDate)
        {
            return _unitOfWork.horarioBloqueadoRepository.GetAll().Where(x => serviceDate.Date >= x.FechaInicio.Date && serviceDate.Date <= x.FechaFin.Date);
        }


        public async Task<IEnumerable<HorarioBloqueado>> GetBlockScheduleAll(int idProfessional)
        {
            return _unitOfWork.horarioBloqueadoRepository.GetAll().Where(x => x.IdProfesional == idProfessional);
        }

        public async Task<bool> BlockSchedule(DateTime startDate, DateTime endDate, int idProfessional)
        {
            try
            {
                var entity = new HorarioBloqueado
                {
                    Id = 0,
                    IdProfesional = idProfessional,
                    FechaInicio = startDate,
                    FechaFin = endDate,
                    HoraInicio = startDate.TimeOfDay,
                    HoraFin = endDate.TimeOfDay
                };
                await _unitOfWork.horarioBloqueadoRepository.Add(entity);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> BlockScheduleMassive(List<DateTime> startDates, List<DateTime> endDates, int idProfessional)
        {
            try
            {
                for (int i = 0; i < startDates.Count; i++)
                {
                    var entity = new HorarioBloqueado
                    {
                        Id = 0,
                        IdProfesional = idProfessional,
                        FechaInicio = startDates[i],
                        FechaFin = endDates[i],
                        HoraInicio = startDates[i].TimeOfDay,
                        HoraFin = endDates[i].TimeOfDay
                    };
                    await _unitOfWork.horarioBloqueadoRepository.Add(entity);
                    await _unitOfWork.SaveChangeAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UnBlockSchedule(long idBlockSchedule)
        {
            try
            {
                await _unitOfWork.horarioBloqueadoRepository.Delete(idBlockSchedule);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
