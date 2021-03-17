using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cogniphi.Platform.Middleware.Authorization.Policies
{
    public static class AuthPolices
    {
        public const string VerbBasedPolicy = "verb";
    }
    public class PolicyRequirement : IAuthorizationRequirement
    {

        public PolicyRequirement()
        {
        }
        public bool Contains(string currentmethod, IEnumerable<string> allowedMethods)
        {
            return allowedMethods.Contains(currentmethod);
        }
    }

    public class PolicyHandler : AuthorizationHandler<PolicyRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PolicyHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override Task HandleRequirementAsync(
    AuthorizationHandlerContext context,
    PolicyRequirement requirement)
        {
            if (context.User == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            if (context.User.Claims == null || !context.User.Claims.Any())
            {
                context.Fail();
                return Task.CompletedTask;
            }

            List<string> allowedMethds = new List<string> { "GET", "POST" };// this needs to be pulled from user claims
            if (!requirement.Contains(_httpContextAccessor.HttpContext.Request.Method, allowedMethds))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}