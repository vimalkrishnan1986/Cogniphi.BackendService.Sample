using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddOpenIdConnect(options =>
{
    options.ClientId = Configuration["Oidc:ClientId"];
    options.ClientSecret = Configuration["Oidc:ClientSecret"];
    options.Authority = Configuration["Oidc:Authority"];
    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;
    options.ResponseType = Configuration["Oidc:ResponseType"];
    options.Scope.Add("claims");
    options.ClaimActions.Clear();
    options.ClaimActions.MapUniqueJsonKey("roles", "roles");
}
); ;

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy => policy.RequireClaim("user_roles", "[Administrator]"));
            });
            services.AddServiceDiscovery(options => options.UseEureka());
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
