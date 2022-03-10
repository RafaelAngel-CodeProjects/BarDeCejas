using BarCejas.App.Models;
using BarCejas.App.Utils;
using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using MercadoPago;
using MercadoPago.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using MercadoPago.Resources;
using MercadoPago.DataStructures.Preference;

namespace BarCejas.App.Controllers
{
    public class PasarelaController : Controller
    {
        private readonly IServicioService _serviceService;
        private readonly IProfesionalService _profesionalService;
        private readonly IContactoLocalService _contactoLocalService;
        private readonly IOrderService _orderService;
        
        private readonly ICredencialMercadoPagoService _serviceredencialMercadoPago;
        private readonly IConfiguration _configuration;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IUrlHelper _urlHelper;

        public PasarelaController(  IServicioService serviceService,
                                    IProfesionalService profesionalService,
                                    IContactoLocalService contactoLocalService,
                                    IOrderService orderService,

                                    ICredencialMercadoPagoService serviceredencialMercadoPago, 
                                    IConfiguration configuration
                                   /*, IHttpContextAccessor httpContextAccessor, IUrlHelper urlHelper*/)
        {
            _serviceService = serviceService;
            _profesionalService = profesionalService;
            _contactoLocalService = contactoLocalService;
            _orderService = orderService;

            _serviceredencialMercadoPago = serviceredencialMercadoPago;
            _configuration = configuration;
            /*_httpContextAccessor = httpContextAccessor;
            _urlHelper = urlHelper;*/
        }

        public async Task<IActionResult> Index()
        {
            CredencialMercadoPago credencial = await _serviceredencialMercadoPago.GetCredencialMercadoPagoById(Convert.ToInt32(_configuration["Setting:MedioDePagoMP"]));

            //TransaccionMercadoPagoViewModel resul = new UtilMercadoPago(_configuration, _httpContextAccessor, _urlHelper)
            this.obtenerDatosPasarelaMP("david.rosas@amedia.com.ar",
                    new List<MercadoPago.DataStructures.Preference.Item> { new MercadoPago.DataStructures.Preference.Item {
                         Id = "1234",
                      Title = "Synergistic Cotton Watch",
                      Quantity = 5,
                      CurrencyId = CurrencyId.ARS,
                      UnitPrice = (decimal)23.25
                    } }, DateTime.Now.ToString("yyyyMMdd-HHmm.ffff"), credencial);

            return View();

        }

        public async Task<IActionResult> Success(string external_reference, string IdOrden, string BranchOfficeId)
        {
            PaymentMethodViewModel proccessedPaymentData = new PaymentMethodViewModel();
            proccessedPaymentData.OrderId = Convert.ToInt32(IdOrden);
            proccessedPaymentData.BranchOfficeId = Convert.ToInt32(BranchOfficeId);

            List<SuccessOrderViewModel> successOrderViewModel = GetSuccessOrderInformation(proccessedPaymentData);

            return View("SuccessOrder", successOrderViewModel);
       
        }

        public async Task<IActionResult> Pending(string external_reference)
        {

            return View("SuccessOrder", new PaymentMethodViewModel());
        }

        public async Task<IActionResult> Failure(string external_reference)
        {

            return View("SuccessOrder", new PaymentMethodViewModel());
        }

        public TransaccionMercadoPagoViewModel obtenerDatosPasarelaMP(string pEmail, List<Item> pitems, string pIdReference, CredencialMercadoPago pCredencial)
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

                    Success = scheme + "://" + host + Url.Action("Success", "Pasarela", new { external_reference = pIdReference, IdOrden = pitems.First().Id }).ToString(),
                    Pending = scheme + "://" + host + Url.Action("Pending", "Pasarela", new { external_reference = pIdReference }).ToString(),
                    Failure = scheme + "://" + host + Url.Action("Failure", "Pasarela", new { external_reference = pIdReference }).ToString()
                };

                // Save and posting preference
                preference.Save();

            }
            catch (Exception e)
            {
                throw e;
            }

            return resul;
        }

        #region UTILS
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