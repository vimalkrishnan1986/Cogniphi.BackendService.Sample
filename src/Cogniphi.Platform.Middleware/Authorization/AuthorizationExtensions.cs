using Cogniphi.Platform.Middleware.Authorization.Policies;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Cogniphi.Platform.Middleware.Authorization
{
    public static class AuthorizationExtensions
    {
        public static void AddAuthroization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

        }).AddCookie().AddOpenIdConnect(options =>
        {
            options.ClientId = configuration["Oidc:ClientId"];
            options.ClientSecret = configuration["Oidc:ClientSecret"];
            options.Authority = configuration["Oidc:Authority"];
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;
            options.ResponseType = OpenIdConnectResponseType.Code;
            options.RequireHttpsMetadata = false;
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }
);
        }

        public static void AddVerbPolicy(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthPolices.VerbBasedPolicy,
                                  policy => policy.Requirements.Add(new AccountRequirement()));
            });
            services.AddSingleton<IAuthorizationHandler, AccountHandler>();
        }
    }


}
