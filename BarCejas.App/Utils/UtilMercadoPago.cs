using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MercadoPago;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using MercadoPago.Resources;
using MercadoPago.DataStructures.Preference;
using MercadoPago.Common;
using BarCejas.App.Models;
using BarCejas.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BarCejas.App.Utils
{
    public class UtilMercadoPago
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelper _urlHelper;

        public UtilMercadoPago(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IUrlHelper urlHelper)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _urlHelper = urlHelper;
        }

        public TransaccionMercadoPagoViewModel obtenerDatosPasarelaMP(string pEmail, List<Item> pitems, string pIdReference, CredencialMercadoPago pCredencial)
        {
            SDK sdk = new SDK();
            TransaccionMercadoPagoViewModel resul = new TransaccionMercadoPagoViewModel();
            string host = _httpContextAccessor.HttpContext.Request.Host.Value,
                scheme = _httpContextAccessor.HttpContext.Request.Scheme;

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
                preference.Items.Add(
                  new Item()
                  {
                      Id = "1234",
                      Title = "Synergistic Cotton Watch",
                      Quantity = 5,
                      CurrencyId = CurrencyId.ARS,
                      UnitPrice = (decimal)23.25
                  }
                );

                // Setting a payer object as value for Payer property
                preference.Payer = new Payer()
                {
                    Email = pEmail
                };

                preference.BackUrls = new BackUrls()
                {
                    Success = scheme + "://" + host + "/" + _urlHelper.Action("Success", "Pasarela", new { external_reference = pIdReference }).ToString(),
                    Pending = scheme + "://" + host + "/" + _urlHelper.Action("Pending", "Pasarela", new { external_reference = pIdReference }).ToString(),
                    Failure = scheme + "://" + host + "/" + _urlHelper.Action("Failure", "Pasarela", new { external_reference = pIdReference }).ToString()
                };

                if (!host.Contains("localhost"))
                {
                    preference.NotificationUrl = scheme + "://" + host + "/" + _urlHelper.Action("Notification", "Pasarela", new { ReferenciaExterna = pIdReference }).ToString();
                }
                else
                {
                    preference.NotificationUrl = _configuration["Setting:UrlQA"].ToString() + _urlHelper.Action("NotificationUrl", "Pasarela", new { ReferenciaExterna = pIdReference }).ToString();
                }

                // Save and posting preference
                preference.Save();

            }
            catch (Exception e)
            {
                throw e;
            }

            /*string Uri = "";
            try
            {
                //preferencia.
                preferencia.ExternalReference = idReference;
                preferencia.AutoReturn = Common.AutoReturnType.all;
                if (!Request.Url.Authority.Contains("localhost"))
                {
                    preferencia.NotificationUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + Url.Action("DetallePagoNotificacion", "PagoHandicap", new { ReferenciaExterna = idReference }).ToString();
                }
                else
                {
                    preferencia.NotificationUrl = obtenerTagDelWebConfig("UrlAlternativa") + Url.Action("DetallePagoNotificacion", "PagoHandicap", new { ReferenciaExterna = idReference }).ToString();
                }
                preferencia.BackUrls = new DataStructures.Preference.BackUrls()
                {
                    Success = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + Url.Action("DetalleDePago", "PagoHandicap", new { external_reference = idReference }).ToString(),
                    Pending = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + Url.Action("DetalleDePagoPending", "PagoHandicap", new { external_reference = idReference }).ToString(),
                    Failure = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + Url.Action("DetalleDePagoError", "PagoHandicap", new { external_reference = idReference }).ToString()
                };

                preferencia.Items = new List<DataStructures.Preference.Item>();

                foreach (var item in model)
                {
                    preferencia.Items.Add(new DataStructures.Preference.Item()
                    {
                        Title = item.texto,
                        CurrencyId = Common.CurrencyId.ARS,
                        Quantity = 1, //un paquete,
                        UnitPrice = item.importe
                    });
                }

                if (model.Any(s => s.importe_seguro > 0))
                {
                    preferencia.Marketplace_fee = (float)model.Sum(s => s.importe_seguro);
                }

                preferencia.Payer = new DataStructures.Preference.Payer()
                {
                    Name = DatosUsuario().Nombre,
                    Surname = DatosUsuario().Apellido,
                    Date_created = DateTime.UtcNow.AddHours(Convert.ToInt16(obtenerTagDelWebConfig("UtcArgentina"))),
                    Identification = new DataStructures.Preference.Identification()
                    {
                        Type = "DNI",
                        Number = DatosUsuario().DocumentoIdentidad
                    }

                };

                preferencia.Save();

                Uri = preferencia.InitPoint;
                resul.EsSatisfactorio = String.IsNullOrEmpty(Uri) ? false : true;
                resul.Cadena = preferencia.CollectorId.ToString();
                resul.strCodigoResultado = Uri;

            }
            catch (Exception e)
            {
                resul.EsSatisfactorio = false;
                resul.strCodigoResultado = e.Message;
            }*/

            return resul;
        }
    }
}
