using BarCejas.App.Models;
using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using BarCejas.App.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MercadoPago.Resources;
using MercadoPago.DataStructures.Preference;
using MercadoPago.Common;
using MercadoPago;
using BarCejas.App.Areas.Admin.Controllers;
using BarCejas.Entities.Enumerations;

namespace BarCejas.App.Controllers
{
    public class PaymentMethodController : BaseController
    {
        private readonly IServicioService _serviceService;
        private readonly IPaquetesService _packageService;
        private readonly IProfesionalService _profesionalService;
        private readonly IContactoLocalService _contactoLocalService;
        private readonly IOrderService _orderService;
        private readonly IModalidadPagoService _modalidadPagoService;
        private readonly string _imgRuta;


        //MercadoPago
        private readonly ICredencialMercadoPagoService _serviceredencialMercadoPago;
        private readonly IConfiguration _configuration;


        public PaymentMethodController(IServicioService serviceService,
                                         IPaquetesService packageService,
                                         IProfesionalService profesionalService,
                                         IContactoLocalService contactoLocalService,
                                         IOrderService orderService,
                                         IModalidadPagoService modalidadPagoService,
                                         ICredencialMercadoPagoService serviceredencialMercadoPago,
                                         IConfiguration configuration)
        {
            _serviceService = serviceService;
            _packageService = packageService;
            _profesionalService = profesionalService;
            _contactoLocalService = contactoLocalService;
            _orderService = orderService;
            _modalidadPagoService = modalidadPagoService;

            _serviceredencialMercadoPago = serviceredencialMercadoPago;
            _configuration = configuration;
            _imgRuta = configuration.GetValue<string>("RutaImg");
        }

        #region Index (Initial Method)
        public IActionResult Index(long PackageId, int ServiceId, int BranchOfficeId, int ProfessionalId, DateTime ServiceDate, string HourService, int OrderId)
        {
            PaymentMethodViewModel model = new PaymentMethodViewModel
            {
                PackageId = PackageId,
                ServiceId = ServiceId,
                BranchOfficeId = BranchOfficeId,
                ProfessionalId = ProfessionalId,
                ServiceDate = ServiceDate,
                HourService = HourService,
                OrderId = OrderId
            };

            //Selected Service Data
            if (ServiceId > 0 && PackageId == 0)
            {
                Servicio SelectedService = _serviceService.GetServicioById(ServiceId).Result;
                model.TotalPrice = SelectedService.PrecioLista;
                model.PromotionalPrice = SelectedService.PrecioPromocioal;
            }

            //Selected Package Data
            if (PackageId > 0)
            {
                Paquete SelectedPackage = _packageService.GetPaqueteById(PackageId).Result;
                model.TotalPrice = SelectedPackage.PrecioFinal;
                model.PromotionalPrice = SelectedPackage.Descuento > 0 ? (SelectedPackage.PrecioFinal - ((SelectedPackage.PrecioFinal * SelectedPackage.Descuento) / 100)) : 0;
            }

            return View("PaymentMethod", model);
        }

        #endregion

        #region ONLINE PROCESS REDIRECTION

        //Redireccion Pago en Linea (Transferencia/MercadoPago) Solo Redireccion Sin Perder Data
        public async Task<IActionResult> ProcessOnlinePayment(long PackageId, int ServiceId, int BranchOfficeId, int ProfessionalId, DateTime ServiceDate, string HourService, int OrderId, decimal? PromotionalPrice, decimal? TotalPrice)
        {
            IEnumerable<ModalidadPago> AuxPaymentModes = _modalidadPagoService.GetModalidadPagoAll();
            PaymentMethodTransferViewModel proccessedPaymentData = new PaymentMethodTransferViewModel
            {
                PackageId = PackageId,
                ServiceId = ServiceId,
                BranchOfficeId = BranchOfficeId,
                ProfessionalId = ProfessionalId,
                ServiceDate = ServiceDate,
                HourService = HourService,
                OrderId = OrderId,
                PaymentModes = AuxPaymentModes,
                PaymentMode = AuxPaymentModes.First().Id
            };
            return View("Index", proccessedPaymentData);
        }


        //Seleccion de Metodo de Pago
        [HttpPost]
        public async Task<IActionResult> SetPaymentMode(PaymentMethodViewModel paymentData)
        {
            IEnumerable<ModalidadPago> AuxPaymentModes = _modalidadPagoService.GetModalidadPagoAll();
            PaymentMethodTransferViewModel proccessedPaymentData = new PaymentMethodTransferViewModel
            {
                PackageId = paymentData.PackageId,
                ServiceId = paymentData.ServiceId,
                BranchOfficeId = paymentData.BranchOfficeId,
                ProfessionalId = paymentData.ProfessionalId,
                ServiceDate = paymentData.ServiceDate,
                HourService = paymentData.HourService,
                OrderId = paymentData.OrderId,
                PaymentModes = AuxPaymentModes,
                PaymentMode = paymentData.PaymentMode,
                PromotionalPrice = paymentData.PromotionalPrice,
                TotalPrice = paymentData.TotalPrice
            };
            return View("Index", proccessedPaymentData);
        }

        #endregion

        #region PROCESS IN STORE PAYMENT METHOD

        //Procesamiento de Pago en Tienda
        public async Task<IActionResult> ProcessLocal(long PackageId, int ServiceId, int BranchOfficeId, int ProfessionalId, DateTime ServiceDate, string HourService, int OrderId, decimal? PromotionalPrice, decimal? TotalPrice)
        {
            string strUser = User.FindFirstValue(ClaimTypes.Name);
            Usuario usuario = new Usuario();
            PaymentMethodViewModel proccessedPaymentData = new PaymentMethodViewModel
            {
                PackageId = PackageId,
                ServiceId = ServiceId,
                BranchOfficeId = BranchOfficeId,
                ProfessionalId = ProfessionalId,
                ServiceDate = ServiceDate,
                HourService = HourService,
                OrderId = OrderId,
                PaymentMethodId = (int)FormaDePago.En_local
            };
            if (PackageId > 0)
            {
                Orden AuxOrden = _orderService.GetOrdersById(OrderId);
                AuxOrden.IdEstatus = (int)EstatusOrden.procesando;
                AuxOrden.IdFormaPago = (int)FormaDePago.En_local;
                AuxOrden.MontoPaquete = PromotionalPrice > 0 ? PromotionalPrice : TotalPrice;
                bool result = _orderService.UpdateOrder(AuxOrden).Result;
            }
            else
            {
                if (!string.IsNullOrEmpty(strUser))
                {
                    usuario = JsonConvert.DeserializeObject<Usuario>(strUser);
                    proccessedPaymentData = await paymentDataProcessOrder(proccessedPaymentData, usuario, false);
                }
            }
           

            List<SuccessOrderViewModel> successOrderViewModel = GetSuccessOrderInformation(proccessedPaymentData);

            return View("SuccessOrder", successOrderViewModel);
        }

        #endregion

        #region PROCESS TRANSFER PAYMENT METHOD

        //Metodo Get Procesamiento Pago Transferencia
        public async Task<IActionResult> ProcessTransfer(long PackageId, int ServiceId, int BranchOfficeId, int ProfessionalId, DateTime ServiceDate, string HourService, int OrderId, int PaymentMode, decimal? PromotionalPrice, decimal? TotalPrice)
        {
            string strUser = User.FindFirstValue(ClaimTypes.Name);
            Usuario usuario = new Usuario();
            PaymentMethodViewModel PaymentMethodViewModel = new PaymentMethodViewModel
            {
                PackageId = PackageId,
                ServiceId = ServiceId,
                BranchOfficeId = BranchOfficeId,
                ProfessionalId = ProfessionalId,
                ServiceDate = ServiceDate,
                HourService = HourService,
                OrderId = OrderId,
                PaymentMode = PaymentMode,
                PromotionalPrice = PromotionalPrice,
                TotalPrice = TotalPrice
            };
            return View("processTransfer", PaymentMethodViewModel);
        }

        //Metodo POST Para Procesamiento Pago Transferencias
        [HttpPost]
        public async Task<IActionResult> ProcessTransferPost(PaymentMethodViewModel PaymentMethodViewModel)
        {
            string strUser = User.FindFirstValue(ClaimTypes.Name);
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(strUser);
            PaymentMethodViewModel.PaymentMethodId = (int)FormaDePago.Transferencia;
            if (PaymentMethodViewModel.PaymentVoucherFile == null || PaymentMethodViewModel.PaymentVoucherFile.Length == 0 || PaymentMethodViewModel.PaymentVoucherFilePath == "Required")
            {
                PaymentMethodViewModel.PaymentVoucherFilePath = "Required";
                return View("processTransfer", PaymentMethodViewModel);
            }
            if (!string.IsNullOrEmpty(strUser))
            {
                usuario = JsonConvert.DeserializeObject<Usuario>(strUser);
                PaymentMethodViewModel = await paymentDataProcessOrder(PaymentMethodViewModel, usuario, false);
            }

            List<SuccessOrderViewModel> successOrderViewModel = GetSuccessOrderInformation(PaymentMethodViewModel);

            return View("SuccessOrder", successOrderViewModel);
        }

        #endregion

        #region PROCESS MERCADOPAGO PAYMENT METHOD

        //Invocacion a metodo Mercado Pago
        public async Task<IActionResult> ProcessMercadoPago(PaymentMethodViewModel PaymentData)
        {
            string strUser = User.FindFirstValue(ClaimTypes.Name);
            Usuario UserApp = JsonConvert.DeserializeObject<Usuario>(strUser);

            CredencialMercadoPago MercadoPagoCredential = await _serviceredencialMercadoPago.GetCredencialMercadoPagoById(Convert.ToInt32(_configuration["Setting:MedioDePagoMP"]));
            PaymentData.PaymentMethodId = (int)FormaDePago.Mercado_Pago;
            PaymentData = await paymentDataProcessOrder(PaymentData, UserApp, PaymentData.PackageId > 0);

            string Title = PaymentData.PackageId > 0 ? _packageService.GetPaqueteById(PaymentData.PackageId).Result.Nombre : _serviceService.GetServicioById(PaymentData.ServiceId).Result.Nombre;

            TransaccionMercadoPagoViewModel resul = this.obtenerDatosPasarelaMP(
                    "david.rosas@amedia.com.ar",
                   //UserApp.Email
                   new List<MercadoPago.DataStructures.Preference.Item> {
                       new MercadoPago.DataStructures.Preference.Item {
                      Id = PaymentData.OrderId.ToString(),
                      Title = Title,
                      Quantity = 1,
                      CurrencyId = CurrencyId.ARS,
                      UnitPrice = PaymentData.PromotionalPrice > 0 ? Convert.ToDecimal(PaymentData.PromotionalPrice) : Convert.ToDecimal(PaymentData.TotalPrice),
                    } }, DateTime.Now.ToString("yyyyMMdd-HHmm.ffff"), MercadoPagoCredential, PaymentData.OrderId.ToString(),PaymentData.BranchOfficeId.ToString());

            if (resul.EsProcesado)
                return Redirect(resul.urlReturn);
            else
                return View();
        }
        #endregion

        #region UTILS METHODS
        //Metodo Mercado Pago
        public TransaccionMercadoPagoViewModel obtenerDatosPasarelaMP(string pEmail, List<Item> pitems, string pIdReference, CredencialMercadoPago pCredencial, string orderId, string BranchOfficeId)
        {
            SDK sdk = new SDK();
            TransaccionMercadoPagoViewModel resul = new TransaccionMercadoPagoViewModel();
            string host = Request.Host.Value,
                scheme = HttpContext.Request.Scheme;

            if (string.IsNullOrEmpty(pCredencial.AccessToken))
            {
                sdk.ClientId = pCredencial.ClientId;
                sdk.ClientSecret = pCredencial.ClientSecret;
            }
            else
            {
                sdk.CleanConfiguration();
                sdk.SetAccessToken(pCredencial.AccessToken);
            }

            Preference preference = new Preference(sdk);
            try
            {
                preference.Items = pitems;

                // Setting a payer object as value for Payer property
                preference.Payer = new Payer()
                {
                    Email = pEmail
                };

                preference.BackUrls = new BackUrls()
                {
                    Success = scheme + "://" + host + Url.Action("Success", "Pasarela", new { external_reference = pIdReference, IdOrden = orderId, BranchOfficeId = BranchOfficeId }).ToString(),
                    Pending = scheme + "://" + host + Url.Action("Pending", "Pasarela", new { external_reference = pIdReference }).ToString(),
                    Failure = scheme + "://" + host + Url.Action("Failure", "Pasarela", new { external_reference = pIdReference }).ToString()
                };

                if (!host.Contains("localhost"))
                {
                    preference.NotificationUrl = scheme + "://" + host + Url.Action("Notification", "Pasarela", new { ReferenciaExterna = pIdReference }).ToString();
                }
                else
                {
                    preference.NotificationUrl = _configuration["Setting:UrlQA"].ToString() + Url.Action("NotificationUrl", "Pasarela", new { ReferenciaExterna = pIdReference }).ToString();
                }

                // Save and posting preference
                preference.Save();
                if (!string.IsNullOrEmpty(preference.InitPoint))
                {
                    resul.EsProcesado = true;
                    resul.urlReturn = preference.InitPoint;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            return resul;
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
                await _orderService.InsertOrderItem(OrderItems);
            }

            //Manteniendo la Data Registrada
            PaymentData.OrderId = PaymentData.OrderId > 0 ? PaymentData.OrderId : OrdenInserted.Id;
            return PaymentData;
        }

        public List<SuccessOrderViewModel> GetSuccessOrderInformation(PaymentMethodViewModel proccessedPaymentData)
        {
            List<SuccessOrderViewModel> successOrderItems = new List<SuccessOrderViewModel>();
            if (proccessedPaymentData.OrderId > 0)
            {
                IEnumerable<OrdenItem> OrderItems = _orderService.GetOrdersItemByOrder(proccessedPaymentData.OrderId);

                foreach (var OrdenItem in OrderItems)
                {
                    //Obtencion de Data
                    Servicio AuxService = _serviceService.GetServicioById(OrdenItem.IdServicio).Result;
                    Profesional AuxProfessional = _profesionalService.GetProfesionalById(OrdenItem.IdProfesional).Result;
                    ContactoLocal AuxBranchOffice = _contactoLocalService.GetContactoLocalById(proccessedPaymentData.BranchOfficeId).Result;
                    //Asignacion
                    SuccessOrderViewModel successOrderItem = new SuccessOrderViewModel();
                    successOrderItem.OrderId = proccessedPaymentData.OrderId;
                    successOrderItem.ServicePageImagePath = AuxService.RutaImagenPaagina;
                    successOrderItem.ServiceName = AuxService.Nombre;
                    successOrderItem.ServiceDate = OrdenItem.FechaDeCita.ToShortDateString();
                    successOrderItem.HourService = OrdenItem.Hora;
                    successOrderItem.DurationService = AuxService.Duracion;
                    successOrderItem.BranchOfficeAddress = AuxBranchOffice.Direccion;
                    successOrderItem.ProfessionalProfileImagePath = "";
                    successOrderItem.ProfessionalDescription = AuxProfessional.Descripcion;
                    successOrderItem.PromotionalPrice = AuxService.PrecioPromocioal;
                    successOrderItem.TotalPrice = AuxService.PrecioLista;
                    successOrderItems.Add(successOrderItem);
                }
            }
            return successOrderItems;
        }
        #endregion
    }
}