using BarCejas.Data.Interfaces;
using BarCejas.Data.Repositories;
using BarCejas.Data.Services;
using BarCejas.Data.DataContext;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BarCejas.Data.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Facebook;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.Security.Claims;

namespace BarCejas.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
       
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddAutoMapper(typeof(Startup));
            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(90);
            });

            services.AddControllersWithViews();
            services.AddRazorPages();

          
            services.AddDbContext<BardecejasContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DbContext"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                }));

            services.Configure<PasswordOptions>(Configuration.GetSection("PasswordOptions"));

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

            services.AddTransient<ITokenService, TokenService>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IPasswordHasher, PasswordService>();

            services.AddTransient<IDiaService, DiaService>();
            services.AddTransient<IContactoLocalService, ContactoLocalService>();
            services.AddTransient<IUsuarioService, UsuarioService>();

            services.AddTransient<ICategoriaService, CategoriaService>();
            services.AddTransient<IFormaPagoService, FormaPagoService>();
            services.AddTransient<ICredencialMercadoPagoService, CredencialMercadoPagoService>();
            services.AddTransient<IModalidadPagoService, ModalidadPagoService>();
            services.AddTransient<IServicioService, ServicioService>();
            services.AddTransient<IProfesionalService, ProfesionalService>();
            services.AddTransient<IPaquetesService, PaqueteService>();
            services.AddTransient<ICredencialMercadoPagoService, CredencialMercadoPagoService>();
            services.AddTransient<IHistoricoIngresosService, HistoricoIngresosService>();

            //--------------------------------------------------------------------------
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.  
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;

            //});

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });

            _ = services.AddAuthentication(options => options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme)
             .AddGoogle(options =>
             {
                 options.ClientId = Configuration["Authentication:Google:ClientId"];
                 options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                 //options.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
                 //options.Scope.Add("https://www.googleapis.com/auth/userinfo.email");
                 //options.Scope.Add("https://www.googleapis.com/auth/plus.me");
                 options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                 options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
             }).
             AddFacebook(facebookOptions =>
             {
                 facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                 facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                 facebookOptions.Fields.Add("picture");
                 //facebookOptions.ClaimActions.MapJsonKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender", "gender", "url");
                 facebookOptions.Events = new OAuthEvents
                 {
                     OnCreatingTicket = (context) =>
                     {
                         ClaimsIdentity identity = (ClaimsIdentity)context.Principal.Identity;
                         string profileImg = context.User.GetProperty("picture").GetProperty("data").GetProperty("url").ToString();
                         identity.AddClaim(new Claim("urn:facebook:picture", profileImg));
                         return Task.CompletedTask;
                     }
                 };
                 facebookOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
             }).AddCookie();
            services.AddMvc(options => options.EnableEndpointRouting = false);

            // Newness 
            _ = services.AddTransient<INewnessService, ManagerNewnessService>();
            _ = services.AddTransient<IOrderService, OrderService>();

            // FrequentQuestion 
            _ = services.AddTransient<IFrequentQuestion, FrequentQuestionService>();

            //Testimonial 
            _ = services.AddTransient<ITestimonial, TestimonialService>();

            //Render View
            services.AddScoped<IViewRenderService, ViewRenderService>();

            services.AddTransient<IMediosContactoEmpresaService, MediosContactoEmpresaService>();

            services.AddTransient<IUsuarioService, UsuarioService>();

            services.AddTransient<ICategoriaService, CategoriaService>();
            services.AddTransient<IFormaPagoService, FormaPagoService>();
            services.AddTransient<IModalidadPagoService, ModalidadPagoService>();
            services.AddTransient<IServicioService, ServicioService>();
            services.AddTransient<IProfesionalService, ProfesionalService>();
            services.AddTransient<IPaquetesService, PaqueteService>();
            services.AddTransient<IOrderService, OrderService>();

            //Banner
            _ = services.AddTransient<IBannerService, BannerService>();

            //MensajeMasivo
            _ = services.AddTransient<IMensajeMasivoService, MensajeMasivoService>();

            services.AddTransient<IReporteService, ReporteService>();
            services.AddControllersWithViews();
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddServerSideBlazor();
            //services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddControllersWithViews().AddSessionStateTempDataProvider().AddRazorRuntimeCompilation();
            services.AddDataProtection();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var supportedCultures = new[]
              {
               new CultureInfo("es-AR"),

            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("es-AR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();


            app.UseRouting();
            app.UseCookiePolicy(); // Before UseAuthentication or anything else that writes cookies. 
            app.UseAuthentication();

            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areaRoeute",
                    pattern: "{area:exists}/{controller}/{action=Index}/{id?}"
                    );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
