using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Services
{
   public class FrequentQuestionService: IFrequentQuestion
    {

        private readonly IUnitOfWork _unitOfWork;

        public FrequentQuestionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PreguntasFrecuentes> GetFrequentQuestionById(long pId)
        {
            return await _unitOfWork.FrequentQuestionRepository.GetById(pId);
        }

        public async Task<bool> InsertFrequentQuestion(PreguntasFrecuentes pPreguntasFrecuentes)
        {
            try
            {
                var FrequentQuestion = GetAll();
                int idOrden = FrequentQuestion.Count() + 1;
                pPreguntasFrecuentes.Orden = idOrden;
                await _unitOfWork.FrequentQuestionRepository.Add(pPreguntasFrecuentes);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public async Task<bool> UpdateFrequentQuestion(PreguntasFrecuentes pPreguntasFrecuentes)
        {
            try
            {
                _unitOfWork.FrequentQuestionRepository.Update(pPreguntasFrecuentes);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }
       

      public IEnumerable<PreguntasFrecuentes> GetAll()
        {
            return _unitOfWork.FrequentQuestionRepository.GetAll().Where(x => !x.EsEliminado);

        }

        public async Task<bool> ChangeStatus(long Id, bool Estatus)
        {

            #region Parameters
            var param = new List<SqlParameter>() {
                        new SqlParameter() {
                            ParameterName = "@Id",
                            SqlDbType =  System.Data.SqlDbType.BigInt,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = Id
                        },
                        new SqlParameter() {
                            ParameterName = "@IndEstatus",
                            SqlDbType =  System.Data.SqlDbType.Bit,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = Estatus
                        }};
            
            var query = " EXECUTE [dbo].[spUpdateEstatusPreguntasFrecuentes] @Id, @IndEstatus";
            #endregion

            return await _unitOfWork.FrequentQuestionRepository.ChangeStatus(query, param);

        }

        public async Task<bool> Reorder(List<PreguntasFrecuentes> pPreguntas) {
            
            var pQuery = " EXECUTE [dbo].[spUpdateOrdenPreguntas] @Data";
           
            return await _unitOfWork.FrequentQuestionRepository.Reorder(pQuery, pPreguntas, null);
        }

        public async Task<bool> DeleteFrequentQuestion(long pId, int pNrOrden) 
        {
            #region Parameters
            var param = new List<SqlParameter>() {
                        new SqlParameter() {
                            ParameterName = "@Id",
                            SqlDbType =  System.Data.SqlDbType.BigInt,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = pId
                        },
                        new SqlParameter() {
                            ParameterName = "@NrOrden",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = pNrOrden
                        }};

            var query = " EXECUTE [dbo].[spDeletePreguntasFrecuentes] @Id, @NrOrden";
            #endregion
            return await _unitOfWork.FrequentQuestionRepository.ExecuteSentence(query, param);
    
        }

        public async Task<IEnumerable<PreguntasFrecuentes>> GetFrequentAllActive()
        {
            var children = new string[] { };
            IEnumerable<PreguntasFrecuentes> frequeQ = await _unitOfWork.FrequentQuestionRepository.GetByEagerLoad((x => !x.EsEliminado && x.IndEstatus == true), children);
            return frequeQ;
        }

        public IEnumerable<PreguntasFrecuentes> GetAllNoTracking()
        {
            return _unitOfWork.FrequentQuestionRepository.GetAllNoTracking().Where(x=>!x.EsEliminado);

        }

    }
}
