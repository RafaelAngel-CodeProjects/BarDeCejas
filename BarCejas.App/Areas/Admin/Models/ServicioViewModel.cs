using BarCejas.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Models
{
    public class ServicioViewModel : Servicio
    {
        public ServicioViewModel()
        {
        }

        public ServicioViewModel(Servicio model)
        {
            Id = model.Id;
            IdCategoria = model.IdCategoria;
            IdUsuarioAlta = model.IdUsuarioAlta;
            Nombre = model.Nombre;
            Duracion = model.Duracion;
            RutaFormulario = model.RutaFormulario;
            RutaImagenPaagina = model.RutaImagenPaagina;
            RutaImagenTarjeta = model.RutaImagenTarjeta;
            RutaVideoPagina = model.RutaVideoPagina;
            PrecioLista = model.PrecioLista;
            PrecioPromocioal = model.PrecioPromocioal;
            Descuento = model.Descuento;
            FechaCrecion = model.FechaCrecion;
            EsActivo = model.EsActivo;
            FechaModificacion = model.FechaModificacion;
            FechaDescuentoInicio = model.FechaDescuentoFin;
            FechaDescuentoFin = model.FechaDescuentoFin;
            DescripcionCorta = model.DescripcionCorta;
            DescripcionLarga = model.DescripcionLarga;
            IndAplicarDescuento = Descuento > 0 ? true : false;
        }

        //[Required(ErrorMessage = "Campo requerido.")]
        public IFormFile ArchivoImagenTarjeta { get; set; }
        public IFormFile ArchivoImagenPaagina { get; set; }
        public IFormFile ArchivoVideoPagina { get; set; }
        //[Required(ErrorMessage = "Campo requerido.")]
        public IFormFile ArchivoFormulario { get; set; }
        public bool IndAplicarDescuento { get; set; }
    }
}
