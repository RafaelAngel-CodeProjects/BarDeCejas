using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Services
{
    public class TestimonialService: ITestimonial
    {

        private readonly IUnitOfWork _unitOfWork;

        public TestimonialService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Testimonios> GetTestimonialById(long pId)
        {
            return await _unitOfWork.TestimonialRepository.GetById(pId);
        }

        public async Task<bool> InsertTestimonial(Testimonios pTestimonios)
        {
            var Testimonial = GetAll();
            int idOrden = Testimonial.Count() + 1;
            pTestimonios.Orden = idOrden;
            await _unitOfWork.TestimonialRepository.Add(pTestimonios);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }

        public async Task<bool> UpdateTestimonial(Testimonios pTestimonios)
        {
            _unitOfWork.TestimonialRepository.Update(pTestimonios);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }

        public IEnumerable<Testimonios> GetAll()
        {
            return _unitOfWork.TestimonialRepository.GetAll().Where(x => !x.EsEliminado); 

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

            var query = " EXECUTE [dbo].[spUpdateEstatusTestimonios] @Id, @IndEstatus";
            #endregion

            return await _unitOfWork.TestimonialRepository.ChangeStatus(query, param);

        }

        public async Task<bool> Reorder(List<Testimonios> pPreguntas)
        {

            var pQuery = " EXECUTE [dbo].[spUpdateOrdenTestimonio] @Data";

            return await _unitOfWork.TestimonialRepository.Reorder(pQuery, pPreguntas, null);
        }

        public async Task<bool> DeleteTestimonial(long pId, int pNrOrden)
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

            var query = " EXECUTE [dbo].[spDeleteTestimonio] @Id, @NrOrden";
            #endregion
            return await _unitOfWork.TestimonialRepository.ExecuteSentence(query, param);

        }


        public async Task<IEnumerable<Testimonios>> GetTestimonialAllActive() {

            var children = new string[] { };

            IEnumerable<Testimonios> objs = await _unitOfWork.TestimonialRepository.GetByEagerLoad((x => !x.EsEliminado && x.IndActivo == true), children);
            return objs;
        }
       
    }
}
