using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface IUsuarioService
    {
        Task<bool> InsertUsuario(Usuario entity);
        Task<bool> UpdateUsuario(Usuario entity);
        Task<Usuario> GetUsuarioByEmail(string email);
        Task<Usuario> GetUsuarioById(int id);
        IEnumerable<Usuario> GetUsuarioAll();
        IEnumerable<Usuario> GetUsuarioByTipo(int tipo);
        Task<bool> ChangeStatus(int id);
        Task<bool> ChangePassword(int id, string password);
    }
}
