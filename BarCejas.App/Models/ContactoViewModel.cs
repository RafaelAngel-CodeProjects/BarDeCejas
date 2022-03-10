using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Models
{
    public class ContactoViewModel : ContactoLocal
    {
        public ContactoViewModel(ContactoLocal pObj, List<Dia> plDia)
        {
            this.lDia = plDia;
            this.Id = pObj.Id;
            this.Nombre = pObj.Nombre;

            this.Direccion = pObj.Direccion;
            this.CoordenadaLatitud = pObj.CoordenadaLatitud;
            this.CoordenadaLongitud = pObj.CoordenadaLongitud;
            this.Telefono = pObj.Telefono;
            this.Email = pObj.Email;
            this.EsActivo = pObj.EsActivo;
            this.EsEliminado = pObj.EsEliminado;
            this.FechaAlta = pObj.FechaAlta;
            this.FechaModif = pObj.FechaModif;
            this.Orden = pObj.Orden;
            this.HorarioAtencionLocal = pObj.HorarioAtencionLocal;
        }

        public List<Dia> lDia { get; set; }

        public string HorioSemanal
        {
            get
            {
                string cadena = string.Empty;
                var lst = this.lDia.OrderBy(x => x.Id);

                foreach (var item in lst)
                {
                    cadena += item.Nombre + ": " + (this.HorarioAtencionLocal.Any(x => x.IdDia == item.Id) ?
                                                        this.HorarioAtencionLocal.FirstOrDefault(x => x.IdDia == item.Id).HoraInicio.ToString(@"hh\.mm") + " - " + this.HorarioAtencionLocal.FirstOrDefault(x => x.IdDia == item.Id).HoraFin.ToString(@"hh\.mm") : "Cerrado")
                                                        + (item.Id == lst.Last().Id? "" : " / "); 
                }

                return cadena;
            }
        }
    }
}
