using Blazored.SessionStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Super_Mercado.Areas.Identity;
using Super_Mercado.Data;
using System;
using System.Collections.Generic;
using Super_Mercado.Handlers;
using System.Net.Http;
using System.Threading.Tasks;
using MudBlazor.Services;
using Blazored.LocalStorage;
using Super_Mercado.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft;
using Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Super_Mercado.Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Super_Mercado
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddTransient<ValidateHeaderHandler>();


            var appSettingSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingSection);
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            services.AddBlazoredLocalStorage();


            services.AddHttpClient<ISuperMercadoService<Categoria>, SuperMercadoServicio<Categoria>>().
                        AddHttpMessageHandler<ValidateHeaderHandler>();


            services.AddHttpClient<ISuperMercadoService<Producto>, SuperMercadoServicio<Producto>>().
                AddHttpMessageHandler<ValidateHeaderHandler>();


            services.AddHttpClient<ISuperMercadoService<Role>, SuperMercadoServicio<Role>>().
                AddHttpMessageHandler<ValidateHeaderHandler>();


            services.AddHttpClient<ISuperMercadoService<Usuario>, SuperMercadoServicio<Usuario>>().
                AddHttpMessageHandler<ValidateHeaderHandler>();


            services.AddHttpClient<ISuperMercadoService<Ordenes>, SuperMercadoServicio<Ordenes>>().
                AddHttpMessageHandler<ValidateHeaderHandler>();


            services.AddHttpClient<ISuperMercadoService<Orden_Detalle>, SuperMercadoServicio<Orden_Detalle>>().
                AddHttpMessageHandler<ValidateHeaderHandler>();


            services.AddHttpClient<ISuperMercadoService<UsuarioDireccion>, SuperMercadoServicio<UsuarioDireccion>>().
                AddHttpMessageHandler<ValidateHeaderHandler>();

            services.AddHttpClient<ISuperMercadoService<ImagenWebPage>, SuperMercadoServicio<ImagenWebPage>>().
            AddHttpMessageHandler<ValidateHeaderHandler>();

        

            services.AddHttpClient<IUsuarioServicio, UsuarioServicio>();





            services.AddSingleton<HttpClient>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                    policy.RequireClaim("isUserAdmin", "true"));
            });


            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDbContext<POSDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 2147483647;
            });

            //services.AddAuthentication("BasicAuth").AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuth", null);

            var jwtSection = Configuration.GetSection("JWTSettings");

            services.Configure<Entidades.JWTSettings>(jwtSection);
            services.AddMudServices();




            var appSettings = jwtSection.Get<JWTSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);


            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });







            services.AddControllers();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();



            app.UseStaticFiles();


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();






            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action}");

                endpoints.MapControllers();

                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
