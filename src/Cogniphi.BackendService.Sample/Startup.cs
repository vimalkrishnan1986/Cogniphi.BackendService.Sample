using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;

namespace Cogniphi.BackendService.Sample
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

            services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
  
}).AddCookie().AddOpenIdConnect(options =>
{
    options.ClientId = Configuration["Oidc:ClientId"];
    options.ClientSecret = Configuration["Oidc:ClientSecret"];
    options.Authority = Configuration["Oidc:Authority"];
    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.RequireHttpsMetadata = false;
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}
);
            services.Configure<OpenIdConnectOptions>(Configuration.GetSection("Oidc"));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("openid", policy => policy.RequireClaim("scope", "openid"));

            });
            services.AddServiceDiscovery(options => options.UseEureka());
            services.AddMvc();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            IdentityModelEventSource.ShowPII = true;
            loggerFactory.AddLog4Net();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDiscoveryClient();
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
