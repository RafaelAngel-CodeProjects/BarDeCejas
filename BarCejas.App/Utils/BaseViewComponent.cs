using BarCejas.Entities;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BarCejas.App.Utils
{
    public abstract class BaseViewComponent : ViewComponent
    {
        public Usuario GetUserIdentity()
        {
            Usuario usuario = new Usuario();
            string strUser = string.Empty;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    strUser = UserClaimsPrincipal.FindFirstValue(ClaimTypes.Name);
                    if (strUser.Length > 350)
                    {
                        usuario = JsonConvert.DeserializeObject<Usuario>(strUser);
                    }
                    //usuario = JsonConvert.DeserializeObject<Usuario>(strUser);
                };
                return usuario;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
