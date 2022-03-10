using BarCejas.Data.Interfaces;
using BarCejas.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Models
{
    public class OrderViewModel
    {
        public int IdOrden { get; set; }
        public int IdOrdenItem { get; set; }
        public int IdCliente { get; set; }
        public string Cliente { get; set; }
        public int IdProfesional { get; set; }
        public string Profesional { get; set; }
        public long IdServicio { get; set; }
        public string Servicio { get; set; }
        public int IdContactoLocal { get; set; }
        public string ContactoLocal { get; set; }
        public DateTime FechaCita { get; set; }
        public string Hora { get; set; }
        public int IdModalidadPago { get; set; }
        public string ModalidadPago { get; set; }
        public int IdFormaPago { get; set; }
        public string FormaPago { get; set; }
        public decimal Monto { get; set; }
        public int IdEstatus { get; set; }
        public int IdEstatusPago { get; set; }

        public OrderViewModel()
        {
        }

        public OrderViewModel(OrdenesVigentes orden)
        {
            IdOrden = orden.IdOrden;
            IdOrdenItem = orden.IdOrdenItem;
            IdCliente = orden.IdCliente;
            Cliente = orden.Cliente;
            IdEstatus = orden.IdEstatus;
            IdFormaPago = orden.IdFormaPago;
            IdEstatusPago = orden.IdEstatusPago;
            IdServicio = orden.IdServicio;
            Servicio = orden.Servicio;
            IdProfesional = orden.IdProfesional;
            IdContactoLocal = orden.IdContacto;
            Profesional = orden.Profesional;
            IdModalidadPago = (int)orden.IdModalidadPago;
            Hora = orden.Hora;
            Monto = (decimal)orden.Monto;
            FechaCita = orden.FechaDeCita;

        }
    }
}
