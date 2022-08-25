using DRRealState.Core.Application.DTOS.Account;
using DRRealState.Core.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace DRRealState.WebApp.Middlewares
{
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _httpContext;

        public ValidateUserSession(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public bool IsLogin() {

            var user = _httpContext.HttpContext.Session.Get<AuthenticationResponse>("user");

            return user != null ? true : false;

        }

    }
}
