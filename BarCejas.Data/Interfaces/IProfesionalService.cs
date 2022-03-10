using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface IProfesionalService
    {
        Task<bool> InsertProfesional(Profesional entity);
        Task<bool> UpdateProfesional(Profesional entity);
        Task<bool> InsertUpdateHorarioProfesional(Profesional entity);
        Task<Profesional> GetProfesionalById(int id);
        Task<Profesional> GetProfesionalByIdUsuario(int id);
        Task<IEnumerable<Profesional>> GetProfesionalAll();
        Task<IEnumerable<Profesional>> GetProfesionalAllActive();
        Task<bool> ChangeStatus(int id);

        Task<IEnumerable<HorarioBloqueado>> GetBlockScheduleBySchedule(DateTime serviceDate);
        Task<IEnumerable<HorarioBloqueado>> GetBlockScheduleAll(int idProfessional);

        Task<bool> BlockSchedule(DateTime startDate, DateTime endDate, int idProfessional);
        Task<bool> BlockScheduleMassive(List<DateTime> startDate, List<DateTime> endDate, int idProfessional);
        Task<bool> UnBlockSchedule(long idBlockSchedule);
    }
}
