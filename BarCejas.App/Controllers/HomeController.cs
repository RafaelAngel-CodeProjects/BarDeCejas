using BarCejas.App.Models;
using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Newtonsoft.Json;

using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;
using BarCejas.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BarCejas.App.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBannerService _serviceBanner;
        private readonly IContactoLocalService _serviceContacto;
        private readonly IDiaService _serviceDia;
        private readonly INewnessService _serviceNewness;
        private readonly ITestimonial _serviceTestimonio;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly ICategoriaService _categoriaService;
        private readonly IPaquetesService _paquetesService;

        public HomeController(ILogger<HomeController> logger, IBannerService serviceBanner, IContactoLocalService serviceContacto, IDiaService serviceDia,
            INewnessService serviceNewness, ITestimonial serviceTestimonio, ICategoriaService categoriaService, IPaquetesService paquetesService, ITokenService tokenService, IConfiguration configuration)
        {
            _logger = logger;
            _serviceBanner = serviceBanner;
            _serviceContacto = serviceContacto;
            _serviceDia = serviceDia;
            _serviceNewness = serviceNewness;
            _serviceTestimonio = serviceTestimonio;
            _tokenService = tokenService;

            _categoriaService = categoriaService;
            _paquetesService = paquetesService;
        }
         
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var u = User.Identity.IsAuthenticated;
            IEnumerable<ContactoLocal> lContact = await _serviceContacto.GetContactoLocalAllActive();
            List<Dia> lDia = _serviceDia.GetDiasAll().OrderBy(x => x.Id).ToList();

            List<ContactoViewModel> vm = new List<ContactoViewModel>();
            lContact.OrderBy(x => x.Orden).ToList().ForEach(x => vm.Add(new ContactoViewModel(x, lDia)));

            ViewBag.lBanner = _serviceBanner.GetBannerAllActive().Result.OrderBy(x => x.Orden).ToList();
            ViewBag.lContacto = vm;
            ViewBag.lNovedades = _serviceNewness.GetNewnessAllActive().Result.OrderByDescending(x => x.Fecha).Take(2).ToList();
            ViewBag.ltestimonios = _serviceTestimonio.GetTestimonialAllActive().Result.OrderBy(x=>x.Orden).ToList();
            ViewBag.lCategorias = _categoriaService.GetCategoriaAllClient();
            ViewBag.lPaquetes = _paquetesService.GetPaqueteAllClient().Take(3).ToList();

            if (TempData.ContainsKey("User"))
            {
                var userView = JsonConvert.DeserializeObject<RegistreViewModel>(TempData["User"].ToString());
                ViewBag.user = userView;//
                ViewBag.IsLogin = (bool)TempData["IsLogin"];
            }
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> _ListadoBanner()
        {
            List<Banner> lBanner = _serviceBanner.GetBannerAllActive().Result.OrderBy(x => x.Orden).ToList();
            return PartialView(lBanner);
        }

        [AllowAnonymous]
        public IActionResult UrlRedirect(string pUrl)
        {
            if (pUrl.Substring(0, 2).Equals("~/"))
                pUrl = pUrl.Replace("~", Request.Scheme + "://" + Request.Host.Value);
            return Redirect(pUrl);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
