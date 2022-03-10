using BarCejas.Entities;

using Microsoft.AspNetCore.Http;

using System.ComponentModel.DataAnnotations;

namespace BarCejas.App.Areas.Admin.Models
{
    public class CategoriaViewModel : Categoria
    {
        public CategoriaViewModel()
        {
        }

        public CategoriaViewModel(Categoria model)
        {
            Id = model.Id;
            Nombre = model.Nombre;
            RutaImagen = model.RutaImagen;
            EsActivo = model.EsActivo;
        }

        //[Required(ErrorMessage = "Campo requerido.")]
        public IFormFile ArchivoImagen { get; set; }
    }
}
