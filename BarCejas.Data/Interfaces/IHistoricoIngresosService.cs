using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface IHistoricoIngresosService
    {
        Task InsertHistoricoIngresos(HistoricoIngresos entity);
        Task<HistoricoIngresos> GetHistoricoIngresosById(int id);
        IEnumerable<HistoricoIngresos> GetHistoricoIngresosAll();
    }
}
