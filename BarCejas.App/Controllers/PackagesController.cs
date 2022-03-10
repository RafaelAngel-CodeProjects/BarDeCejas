//using AutoMapper.Configuration;
using BarCejas.App.Models;
using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BarCejas.App.Utils;
using BarCejas.Entities.Enumerations;
using BarCejas.App.Areas.Admin.Controllers;
using Microsoft.Extensions.Configuration;

namespace BarCejas.App.Controllers
{
    public class PackagesController : BaseController
    {
        private readonly IPaquetesService _packageService;
        private readonly IServicioService _serviceService;
        private readonly IProfesionalService _professionalService;
        private readonly IContactoLocalService _branchOfficeService;
        private readonly IOrderService _orderService;
        private readonly IUsuarioService _userService;
        private readonly IModalidadPagoService _paymentMethodsService;
        private readonly string _imgRuta;



        //MercadoPago
        private readonly ICredencialMercadoPagoService _serviceredencialMercadoPago;
        private readonly IConfiguration _configuration;

        public PackagesController(IPaquetesService packageService,
                                  IServicioService serviceService,
                                  IProfesionalService professionalService,
                                  IContactoLocalService branchOfficeService,
                                  IOrderService orderService,
                                  IUsuarioService userService,
                                  IModalidadPagoService modalidadPagoService,
                                  IConfiguration configuration)
        {
            _packageService = packageService;
            _serviceService = serviceService;
            _professionalService = professionalService;
            _branchOfficeService = branchOfficeService;
            _orderService = orderService;
            _userService = userService;
            _paymentMethodsService = modalidadPagoService;
            _configuration = configuration;
            _imgRuta = configuration.GetValue<string>("RutaImg");
        }

        //Metodo Index
        public IActionResult Index()
        {
            IEnumerable<Entities.Paquete> packages = _packageService.GetPaqueteAllClient();
            foreach (var item in packages)
            {
                foreach (var servicio in item.ServicioPaquete)
                {
                    servicio.IdServicioNavigation = _serviceService.GetServicioById(Convert.ToInt64(servicio.IdServicio)).Result;
                }
            }
            return View(packages);
        }




        [HttpGet]
        //[Route("SelectedPackage/{id}")]
        public async Task<IActionResult> SelectedPackage(long PackageId, int ServiceId, int BranchOfficeId, int? ProfessionalId, DateTime ServiceDate, string HourService, int OrderId)
        {
            string strUser = User.FindFirstValue(ClaimTypes.Name);
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(strUser);
            Paquete AuxPackage = _packageService.GetPaqueteById(PackageId).Result;

            PackageInfoViewModel packageInfoViewModel = new PackageInfoViewModel();
            packageInfoViewModel.Package = AuxPackage;
            PaymentMethodViewModel PaymentMethodViewModel = new PaymentMethodViewModel
            {
                PackageId = PackageId,
                ServiceId = ServiceId,
                BranchOfficeId = BranchOfficeId,
                ProfessionalId = Convert.ToInt32(ProfessionalId),
                ServiceDate = ServiceDate,
                HourService = HourService,
                OrderId = OrderId
            };
            foreach (var servicio in packageInfoViewModel.Package.ServicioPaquete)
            {
                servicio.IdServicioNavigation = _serviceService.GetServicioById(Convert.ToInt64(servicio.IdServicio)).Result;
            }

            if (PackageId > 0 && ServiceId > 0)
            {
                if (!string.IsNullOrEmpty(strUser))
                {
                    usuario = JsonConvert.DeserializeObject<Usuario>(strUser);
                    PaymentMethodViewModel = await paymentDataProcessOrder(PaymentMethodViewModel, usuario, false);
                }

            }

            if (PaymentMethodViewModel.OrderId > 0)
            {
                packageInfoViewModel.OrderItems = _orderService.GetOrdersItemByOrder(OrderId);
                packageInfoViewModel.OrderId = PaymentMethodViewModel.OrderId;
            }



            packageInfoViewModel.PackageId = PackageId;
            packageInfoViewModel.ServiceId = ServiceId;
            packageInfoViewModel.BranchOfficeId = BranchOfficeId;
            packageInfoViewModel.ProfessionalId = Convert.ToInt32(ProfessionalId);
            packageInfoViewModel.ServiceDate = ServiceDate;
            packageInfoViewModel.HourService = HourService;          


            return View("Package", packageInfoViewModel);

        }

        //Metodo Procesar orden / Mantener Data
        public async Task<PaymentMethodViewModel> paymentDataProcessOrder(PaymentMethodViewModel PaymentData, Usuario Usuario, bool onlyKeepData)
        {
            //Principal Variables
            Orden OrdenInserted = new Orden(); //Cabecera de orden
            List<OrdenItem> OrderItems = new List<OrdenItem>(); //Listado de Productos/Servicios / items de la orden

            //Cabecera de la Orden
            OrderViewModel OrderViewModel = new OrderViewModel();
            OrderViewModel.Id = PaymentData.OrderId;
            OrderViewModel.IdEstatus = PaymentData.PackageId > 0 ? (int)EstatusOrden.preOrden : (int)EstatusOrden.pendiente;
            OrderViewModel.IdEstatusPago = (int)EstatusPago.pendiente;
            OrderViewModel.IdModalidadPago = PaymentData.PaymentMode;
            OrderViewModel.IdFormaPago = PaymentData.PaymentMethodId;
            OrderViewModel.IdCliente = Usuario.Id;
            OrderViewModel.IdPaquete = Convert.ToInt32(PaymentData.PackageId);
            OrderViewModel.IdContactoLocal = PaymentData.BranchOfficeId;
            OrderViewModel.MontoPaquete = PaymentData.PackageId > 0 ? PaymentData.PromotionalPrice > 0 ? PaymentData.PromotionalPrice : PaymentData.TotalPrice : 0;
            OrderViewModel.FechaDeCreacion = DateTime.Now;

            if (PaymentData.PaymentVoucherFile != null && PaymentData.PaymentVoucherFile.Length > 0)
                OrderViewModel.ComprobantePago = await SaveImgAsync(PaymentData.PaymentVoucherFile, _imgRuta + "Servicios");

            //Genera la cabecera de la orden
            if (!onlyKeepData && PaymentData.OrderId == 0)
            {
                OrdenInserted = await _orderService.InsertOrder(OrderViewModel);
            }

            //Items de la Orden
            OrdenItem OrderItem = new OrdenItem();
            OrderItem.IdOrden = PaymentData.OrderId == 0 ? OrdenInserted.Id : PaymentData.OrderId;
            OrderItem.IdServicio = PaymentData.ServiceId;
            OrderItem.IdProfesional = PaymentData.ProfessionalId;
            OrderItem.Hora = PaymentData.HourService;

            //MontoServicio
            Entities.Servicio auxServ = new Servicio();
            auxServ = _serviceService.GetServicioById(OrderItem.IdServicio).Result;
            OrderItem.Monto = auxServ.PrecioPromocioal > 0 ? auxServ.PrecioPromocioal : auxServ.PrecioLista;

            OrderItem.FechaDeCita = PaymentData.ServiceDate;

            //Asginacion de Lisstado de Productos
            OrderItems.Add(OrderItem);

            if (!onlyKeepData)
            {
                if (!_orderService.GetOrdersItemByOrder(OrderItem.IdOrden).Any(x=>x.IdServicio == OrderItem.IdServicio))
                {
                    await _orderService.InsertOrderItem(OrderItems);
                }
            }

            //Manteniendo la Data Registrada
            PaymentData.OrderId = PaymentData.OrderId > 0 ? PaymentData.OrderId : OrdenInserted.Id;
            return PaymentData;
        }


    }
}
