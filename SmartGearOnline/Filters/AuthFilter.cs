using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartGearOnline.Services;

namespace SmartGearOnline.Filters
{
    public class AuthFilter : IAuthorizationFilter
    {
        private readonly IAuthService _authService;

        public AuthFilter(IAuthService authService)
        {
            _authService = authService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var username = context.HttpContext.Request.Headers["Username"].FirstOrDefault();
            var password = context.HttpContext.Request.Headers["Password"].FirstOrDefault();

            if (!_authService.Authenticate(username, password))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}