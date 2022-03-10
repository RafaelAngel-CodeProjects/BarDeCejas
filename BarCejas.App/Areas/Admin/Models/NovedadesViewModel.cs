using BarCejas.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Models
{
    public class NovedadesViewModel : Novedades
    {
        public NovedadesViewModel()
        {

        }

        public NovedadesViewModel(Novedades obj)
        {
            this.Id = obj.Id;
            this.Titulo = obj.Titulo;
            this.Copete = obj.Copete;
            this.Contenido = obj.Contenido;
            this.Medio = obj.Medio;
            this.Link = obj.Link;
            this.Fecha = obj.Fecha;
            this.IndEstatus = obj.IndEstatus;
            this.IndHome = obj.IndHome;
            this.RutaImagen = obj.RutaImagen;
        }

        public IFormFile Archivo { get; set; }
        
    }
}
