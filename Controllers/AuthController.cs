using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Constants.Enums;
using WebApplication1.Dtos;
using WebApplication1.Interfaces.Services;
using WebApplication1.Utils.Authorization;
using WebApplication1.Utils.HttpExceptionResponse;
using WebApplication1.Utils.HttpResponse;

namespace WebApplication1.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        [AnonymousOnly]
        public async Task<ActionResult<HttpResponse<Guid>>> SignUp(SignUpDto data)
        {
            var id = await _authService.SignUp(data);

            return CreatedResponse(id);
        }

        [HttpPost]
        [AllowAnonymous]
        [AnonymousOnly]
        public async Task<ActionResult<HttpResponse<string>>> SignIn(SignInDto data)
        {
            var accessToken = await _authService.SignIn(data, Response.Cookies);

            return accessToken == null
                ? throw new HttpExceptionResponse(HttpExceptionTypes.BadRequest, "Invalid username or password")
                : SuccessResponse(accessToken);
        }

        [HttpGet]
        [AllowAnonymous]
        [AnonymousOnly]
        public async Task<ActionResult<HttpResponse<string>>> RefreshTokens()
        {
            var accessToken = await _authService.Refresh2TokenTypes(Response.Cookies, Request.Cookies["refreshToken"]);

            return accessToken == null
                ? throw new HttpExceptionResponse(HttpExceptionTypes.BadRequest, "Invalid refresh token")
                : SuccessResponse(accessToken);
        }
    }
}
