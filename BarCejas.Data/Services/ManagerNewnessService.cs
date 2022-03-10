using BarCejas.Data.Interfaces;
using BarCejas.Data.Repositories;
using BarCejas.Entities;

using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Services
{
    public class ManagerNewnessService: INewnessService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManagerNewnessService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Novedades> GetNewnessById(long id)
        {
            return await _unitOfWork.NewnessRepository.GetById(id);
        }

        public async Task<bool> InsertNewness(Novedades pnovedades)
        {
            await _unitOfWork.NewnessRepository.Add(pnovedades);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }

        public async Task<bool> UpdateNewness(Novedades pnovedades)
        {
            _unitOfWork.NewnessRepository.Update(pnovedades);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }

        public IEnumerable<Novedades> GetAll()
        {
            return _unitOfWork.NewnessRepository.GetAll();
           
        }

        public async Task<bool> ChangeStatus(long Id, bool Estatus) {

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

               var query = " EXECUTE [dbo].[spUpdateEstatusNovedades] @Id, @IndEstatus";
            #endregion

          return await _unitOfWork.NewnessRepository.ChangeStatus(query, param);

        }

        public async Task<bool> ActivateIndHome(long Id, bool pIndHome) {

            #region Parameters
            var param = new List<SqlParameter>() {
                        new SqlParameter() {
                            ParameterName = "@Id",
                            SqlDbType =  System.Data.SqlDbType.BigInt,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = Id
                        },
                        new SqlParameter() {
                            ParameterName = "@IndHome",
                            SqlDbType =  System.Data.SqlDbType.Bit,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = pIndHome
                        }};

            var query = " EXECUTE [dbo].[spUpdateIndHomeNovedades] @Id, @IndHome";
            #endregion

            return await _unitOfWork.NewnessRepository.ChangeStatus(query, param);

        }

        public async Task<IEnumerable<Novedades>> GetNewnessAllActive()
        {
            var children = new string[] { };

            IEnumerable<Novedades> objs = await _unitOfWork.NewnessRepository.GetByEagerLoad((x => x.IndEstatus == true), children);
            return objs;
        }

    }
}
