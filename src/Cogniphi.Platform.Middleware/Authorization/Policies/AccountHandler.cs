using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Cogniphi.Platform.Middleware.Authorization.Policies
{
    public static class AuthPolices
    {
        public const string VerbBasedPolicy = "verb";
    }
    public class AccountRequirement : IAuthorizationRequirement
    {
        private string _account;

        public AccountRequirement(string account)
        {
            _account = account;
        }

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
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}