using Cogniphi.Platform.Middleware.Authorization.Policies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cogniphi.Platform.Middleware.Authorization
{
    public static class AuthorizationExtensions
    {
        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddCookie()
              .AddJwtBearer(option =>
              {
                  option.RequireHttpsMetadata = false;
                  option.SaveToken = true;
                  option.Authority = configuration["Jwt:Authority"];
                  option.Audience = configuration["Jwt:Audience"];
                  //option.TokenValidationParameters = new TokenValidationParameters
                  //{
                  //    ValidateIssuerSigningKey = false,
                  //    ValidateAudience = false,
                  //    ValidateLifetime = false,
                  //};

                  option.Events = new JwtBearerEvents
                  {
                      OnAuthenticationFailed = (authContext) =>
                       {
                           string message = authContext.Exception.Message;

                           return Task.CompletedTask;
                       }
                  };
              });
        }

        public static void RegisterVerbPolicy(this AuthorizationOptions options)
        {
            options.AddPolicy(AuthPolices.VerbBasedPolicy, configure =>
                         {
                             configure.Requirements.Add(new PolicyRequirement());
                         });
        }

        public static void RegisterHandlers(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, PolicyHandler>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }


}
