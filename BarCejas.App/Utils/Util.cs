using BarCejas.Entities;
using BarCejas.Entities.Enumerations;

using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BarCejas.App.Utils
{
    public static class Util
    {
        public static List<SelectListItem> GetItemsByEnumGenere()
        {
            return Enum.GetValues(typeof(Genere)).Cast<Genere>().Where(x => (int)x != 0).Select(v => new SelectListItem
            {
                Text = v.ToString().ToUpper(),
                Value = ((int)v).ToString()
            }).ToList();
        }

        public static List<SelectListItem> GetItemsByEnumEstatusPago()
        {
            return Enum.GetValues(typeof(EstatusPago)).Cast<EstatusPago>().Where(x => (int)x != 0).Select(v => new SelectListItem
            {
                Text = v.ToString().Replace("_", " ").ToUpper(),
                Value = ((int)v).ToString()
            }).ToList();
        }

        public static List<SelectListItem> GetItemsByEnumEstatusOrden()
        {
            return Enum.GetValues(typeof(EstatusOrden)).Cast<EstatusOrden>().Where(x => x != EstatusOrden.preOrden && (int)x != 0).Select(v => new SelectListItem
            {
                Text = v.ToString().Replace("_", " ").ToUpper(),
                Value = ((int)v).ToString()
            }).ToList();
        }

        public static List<SelectListItem> GetItemsByEnumFormaPago(int idFormaPagoExcluida = 0)
        {
            return Enum.GetValues(typeof(FormaDePago)).Cast<FormaDePago>().Where(x => (int)x != 0 && (idFormaPagoExcluida > 0 ? (int)x != idFormaPagoExcluida : true)).Select(v => new SelectListItem
            {
                Text = v.ToString().Replace("_", " ").ToUpper(),
                Value = ((int)v).ToString()
            }).ToList();
        }

        public static List<SelectListItem> GetItemsByEnumMedioPago()
        {
            return Enum.GetValues(typeof(MedioDePago)).Cast<MedioDePago>().Where(x => (int)x != 0).Select(v => new SelectListItem
            {
                Text = v.ToString().Replace("_", " ").Replace("porc", "%").ToUpper(),
                Value = ((int)v).ToString()
            }).ToList();
        }

     
      

        public static List<SelectListItem> GetModalidadPagoSelectListItems(IEnumerable<ModalidadPago> model, ICollection<ModalidadPagoServicio> modelDetail = null)
        {
            return model.Where(x => x.EsActivo == true).Select(v => new SelectListItem
            {
                Text = v.Nombre.ToString().ToUpper(),
                Value = v.Id.ToString(),
                Selected = (modelDetail != null && modelDetail.Count > 0 && modelDetail.Any(x => x.IdModalidadPago == v.Id)) ? true : false
            }).ToList();
        }

        public static List<SelectListItem> GetFormaPagoSelectListItems(IEnumerable<FormaPago> model, ICollection<FormaPagoServicio> modelDetail = null)
        {
            return model.Where(x => x.EsActivo == true).Select(v => new SelectListItem
            {
                Text = v.Nombre.ToString().ToUpper(),
                Value = v.Id.ToString(),
                Selected = (modelDetail != null && modelDetail.Count > 0 && modelDetail.Any(x => x.IdFormaPago == v.Id)) ? true : false
            }).ToList();
        }

        public static List<SelectListItem> GetCategoriaSelectListItems(IEnumerable<Categoria> model)
        {
            return model.Where(x => x.EsActivo == true).Select(v => new SelectListItem
            {
                Text = v.Nombre.ToString().ToUpper(),
                Value = v.Id.ToString()
            }).ToList();
        }

        public static List<SelectListItem> GetContactoLocalSelectListItems(IEnumerable<ContactoLocal> model, ICollection<ServicioContactoLocal> modelDetail = null)
        {

            return model.Where(x => x.EsActivo == true).Select(v => new SelectListItem
            {
                Text = v.Nombre.ToString().ToUpper(),
                Value = v.Id.ToString(),
                Selected = (modelDetail != null && modelDetail.Count > 0 && modelDetail.Any(x => x.IdContactoLocal == v.Id)) ? true : false
            }).ToList();
        }

        public static List<SelectListItem> GetServicioSelectListItems(IEnumerable<Servicio> model, ICollection<ServicioProfesional> modelDetail = null)
        {
            return model.Where(x => x.EsActivo == true).Select(v => new SelectListItem
            {
                Text = v.Nombre.ToString().ToUpper(),
                Value = v.Id.ToString(),
                Selected = (modelDetail != null && modelDetail.Count > 0 && modelDetail.Any(x => x.IdServicio == v.Id)) ? true : false
            }).ToList();
        }

        public static List<SelectListItem> GetProfesionalSelectListItems(IEnumerable<Profesional> model, ICollection<ServicioProfesional> modelDetail = null)
        {
            return model.Where(x => x.EsActivo == true).Select(v => new SelectListItem
            {
                Text = $"{v.IdUsuarioNavigation.Nombre} {v.IdUsuarioNavigation.Apellido}".ToUpper(),
                Value = v.Id.ToString(),
                Selected = (modelDetail != null && modelDetail.Count > 0 && modelDetail.Any(x => x.IdProfesional == v.Id)) ? true : false
            }).ToList();
        }

        public static List<SelectListItem> GetContactoLocal_X_ServicioSelectListItems(IEnumerable<ServicioContactoLocal> model)
        {
            return model.Where(x => /*x.EsActivo == true && */x.IdContactoLocalNavigation != null).Select(v => new SelectListItem
            {
                Text = v.IdContactoLocalNavigation.Nombre.ToString().ToUpper(),
                Value = v.IdContactoLocalNavigation.Id.ToString()
            }).Distinct().ToList();
        }

        public static List<SelectListItem> GetProfesionales_X_ServicioSelectListItems(IEnumerable<ServicioProfesional> model)
        {
            return model.Where(x => x.EsActivo == true && x.IdProfesionalNavigation.IdUsuarioNavigation != null).Select(v => new SelectListItem
            {
                Text = $"{v.IdProfesionalNavigation.IdUsuarioNavigation.Nombre} {v.IdProfesionalNavigation.IdUsuarioNavigation.Apellido}".ToUpper(),
                Value = v.Id.ToString()
            }).Distinct().ToList();
        }
    }
}
