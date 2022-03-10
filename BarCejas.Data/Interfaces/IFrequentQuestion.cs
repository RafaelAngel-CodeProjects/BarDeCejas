using BarCejas.Entities;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
   public interface IFrequentQuestion
    {
        Task<bool> InsertFrequentQuestion(PreguntasFrecuentes FrequentQuestion);

        Task<bool> UpdateFrequentQuestion(PreguntasFrecuentes FrequentQuestion);

        Task<PreguntasFrecuentes> GetFrequentQuestionById(long id);

        //PagedList<Novedades> GetFrequentQuestionsAll(QueryFilter filters);

        public IEnumerable<PreguntasFrecuentes> GetAll();

        public Task<bool> ChangeStatus(long Id, bool Estatus);

        public Task<bool> DeleteFrequentQuestion(long pId, int pOrden);

        public Task<bool> Reorder(List<PreguntasFrecuentes> pPreguntas);

        public Task<IEnumerable<PreguntasFrecuentes>> GetFrequentAllActive();

        public IEnumerable<PreguntasFrecuentes> GetAllNoTracking();

    }
}
