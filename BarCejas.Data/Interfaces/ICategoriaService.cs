using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface ICategoriaService
    {
        Task<bool> InsertCategoria(Categoria entity);
        Task<bool> UpdateCategoria(Categoria entity);
        Categoria GetCategoriaById(int id);
        IEnumerable<Categoria> GetCategoriaAll();
        IEnumerable<Categoria> GetCategoriaAllClient();
        Task<bool> ChangeStatus(int id);
    }
}
