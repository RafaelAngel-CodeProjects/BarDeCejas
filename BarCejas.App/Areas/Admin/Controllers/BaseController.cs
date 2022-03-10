using BarCejas.Entities;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BarCejas.App.Areas.Admin.Controllers
{
    public class BaseController: Controller
    {
        public Usuario GetUserIdentity()
        {
            Usuario usuario;
            string strUser = string.Empty;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    strUser = User.FindFirstValue(ClaimTypes.Name);
                    usuario = JsonConvert.DeserializeObject<Usuario>(strUser);
                    return usuario;
                }
                return null;
            }
            catch (Exception e)
            {
                throw e;
            }  
        }

        /// <summary>
        /// Metodo para guardar las imagenes de clientes en el proyecto
        /// </summary>
        /// <param name="pfile"> archivo jpg/png</param>
        /// <param name="pnameFolder">nombre de la carpeta a guardar el archivo</param>
        /// <returns></returns>
        public  async Task<string> SaveImgAsync(IFormFile pfile, string pnameFolder)
        {
            try
            {
               
                string nameFolderMaster = pnameFolder.Replace("/", @"\"); //pnameFolder.Replace("\\", @"/")
                var urlFile = DateTime.UtcNow.ToString("yyMMddHHmmss") + "_" + Path.GetFileName(pfile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory());
                string rutfinal = filePath + nameFolderMaster;
                string urlFolder = string.Empty;
                if (pfile != null && pfile.Length > 0)
                {

                    if (!Directory.Exists(rutfinal))
                    {
                       Directory.CreateDirectory(rutfinal);
                    }
                    //RutaImagen.
                   
                    using (var fileSrteam = new FileStream(Path.Combine(rutfinal, urlFile), FileMode.Create))
                    {
                        await pfile.CopyToAsync(fileSrteam);
                        
                    }
                    var urltem = nameFolderMaster.Split(@"\");
                    
                    var removestring = nameFolderMaster.Remove(0, urltem[1].Length + 1);
                   
                    urlFolder = "~" + removestring.Replace(@"\","/") + "/"+ urlFile;

                }
                else
                {
                    urlFolder = string.Empty;
                }
                return urlFolder;
            }
            catch (Exception ex)
            {

                return "Error/";
            }

        }

        public bool calcularZise(long pZiseImg, long ziseMax, long Conversion) {
            long cant = pZiseImg / Conversion;
            bool result = cant < ziseMax;
            return result;
        }
      
        //public  async Task<string> RenderViewAsync<TModel>(this Controller controller, string viewName, TModel model, bool isPartial = false)
        //{
        //    if (string.IsNullOrEmpty(viewName))
        //    {
        //        viewName = controller.ControllerContext.ActionDescriptor.ActionName;
        //    }

        //    controller.ViewData.Model = model;

        //    using (var writer = new StringWriter())
        //    {
        //        IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
        //        ViewEngineResult viewResult = GetViewEngineResult(controller, viewName, isPartial, viewEngine);

        //        if (viewResult.Success == false)
        //        {
        //            throw new System.Exception($"A view with the name {viewName} could not be found");
        //        }

        //        ViewContext viewContext = new ViewContext(
        //            controller.ControllerContext,
        //            viewResult.View,
        //            controller.ViewData,
        //            controller.TempData,
        //            writer,
        //            new HtmlHelperOptions()
        //        );

        //        await viewResult.View.RenderAsync(viewContext);

        //        return writer.GetStringBuilder().ToString();
        //    }
        //}

        //private  ViewEngineResult GetViewEngineResult(Controller controller, string viewName, bool isPartial, IViewEngine viewEngine)
        //{
        //    if (viewName.StartsWith("~/"))
        //    {
        //        var hostingEnv = controller.HttpContext.RequestServices.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;
        //        return viewEngine.GetView(hostingEnv.WebRootPath, viewName, !isPartial);
        //    }
        //    else
        //    {
        //        return viewEngine.FindView(controller.ControllerContext, viewName, !isPartial);

        //    }
        //}
    }
}