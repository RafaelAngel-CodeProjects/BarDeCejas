using BarCejas.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Models
{
    public class BannerViewModel : Banner
    {
        public BannerViewModel()
        {

        }

        public BannerViewModel(Banner pBanner)
        {
            this.Id = pBanner.Id;
            this.Titulo = pBanner.Titulo;
            this.Texto = pBanner.Texto;
            this.TextoBoton = pBanner.TextoBoton;
            this.LinkBoton = pBanner.LinkBoton;
            this.RutaImagenDestok = pBanner.RutaImagenDestok;
            this.RutaImagenMobile = pBanner.RutaImagenMobile;
            this.Orden = pBanner.Orden;
            this.FechaAlta = pBanner.FechaAlta;
            this.FechaModif = pBanner.FechaModif;
            this.EsActivo = pBanner.EsActivo;
        }

        public IFormFile ImagenDestok { get; set; }

        public IFormFile ImagenMobile { get; set; }
    }
}
