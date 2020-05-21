using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudMovie.Web.Helper;
using CloudMovie.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CloudMovie.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        IConfiguration Configuration { get; }

        IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IKeyVaultService, KeyVaultService>();
            services.AddScoped<JWTHelper>();

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(g =>
            {
                g.ClientId = Configuration["Authentication:Google:ClientId"];
                g.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                g.SaveTokens = true;
            })
            .AddMicrosoftAccount(ms =>
            {
                ms.ClientId = Configuration["Authentication:Microsoft:ClientId"];
                ms.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
                ms.SaveTokens = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}