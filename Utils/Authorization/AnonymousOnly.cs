using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace WebApplication1.Utils.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AnonymousOnly : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity != null && context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new ObjectResult(
                    new
                    {
                        StatusCode = HttpStatusCode.Forbidden,
                        Message = "Unauthenticated user only"
                    }
                )
                {
                    StatusCode = (int)HttpStatusCode.Forbidden
                };
            }
        }
    }
}
