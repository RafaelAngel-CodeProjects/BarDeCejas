using BarCejas.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Models
{
    public class PaqueteViewModel : Paquete
    {
        public PaqueteViewModel()
        {
        }

        public PaqueteViewModel(Paquete model)
        {
            Id = model.Id;
            Nombre = model.Nombre;
            EsActivo = model.EsActivo;
            Descuento = model.Descuento;
            PrecioFinal = model.PrecioFinal;
            RutaImagenHome = model.RutaImagenHome;
            ServicioPaquete = model.ServicioPaquete;
            DescripcionCorta = model.DescripcionCorta;
            DescripcionLarga = model.DescripcionLarga;
            FechaVigenciaFin = model.FechaVigenciaFin;
            RutaImagenTarjeta = model.RutaImagenTarjeta;
            FechaVigenciaInicio = model.FechaVigenciaInicio;
        }

        //[Required(ErrorMessage = "Campo requerido.")]
        public IFormFile ArchivoImagenTarjeta { get; set; }
        public IFormFile ArchivoImagenHome { get; set; }
    }
}
