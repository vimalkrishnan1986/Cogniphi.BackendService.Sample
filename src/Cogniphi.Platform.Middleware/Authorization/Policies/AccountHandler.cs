using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Cogniphi.Platform.Middleware.Authorization.Policies
{
    public static class AuthPolices
    {
        public static string VerbBasedPolicy => "verb";
    }
    public class AccountRequirement : IAuthorizationRequirement
    {

    }

    public class AccountHandler : AuthorizationHandler<AccountRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AccountRequirement requirement)
        {
            // check the claims here and if  current method available in the ciams .mark succed;
            var httpMethod = _httpContextAccessor.HttpContext.Request.Method;
            if (httpMethod == "POST")
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}