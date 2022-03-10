using AutoMapper;

using BarCejas.App.Utils;
using BarCejas.Data.Interfaces;
using BarCejas.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace BarCejas.App.Views.Shared.Components
{
    public class ClientMenuViewComponent : BaseViewComponent
    {
        private readonly IConfiguration _configuration;
        private readonly IUsuarioService _usuarioService;
        private readonly ITokenService _tokenService;
        private readonly ICategoriaService _categoriaService;

        public ClientMenuViewComponent(IConfiguration configuration, ITokenService tokenService, IUsuarioService usuarioService, ICategoriaService categoriaService, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _usuarioService = usuarioService;
            _categoriaService = categoriaService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Usuario obj = new Usuario();
            try
            {
                obj = GetUserIdentity();
                string avatar = obj.Avatar;
                //Creo que esta llamada sera necesaria. Para obtener los datos actualizados del cleinte
                if (obj != null && obj.Id > 0)
                {
                    obj = await _usuarioService.GetUsuarioById(obj.Id);
                    obj.Avatar = avatar;
                }
                ViewBag.Categories = Util.GetCategoriaSelectListItems(_categoriaService.GetCategoriaAllClient());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(obj);
        }
    }
}

