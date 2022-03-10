using BarCejas.Entities;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Models
{
    public class TestimonioViewModel: Testimonios
    {

        public TestimonioViewModel()
        {

        }
        public TestimonioViewModel(Testimonios ptestimonio)
        {
            this.Id = ptestimonio.Id;
            this.IndActivo = ptestimonio.IndActivo;
            this.Autor = ptestimonio.Autor;
            this.Orden = ptestimonio.Orden;
            this.Testimonio = ptestimonio.Testimonio;
            this.RutaImagen = ptestimonio.RutaImagen;
            this.EsEliminado = ptestimonio.EsEliminado;
        }
    public IFormFile Archivo { get; set; }

    }
}
