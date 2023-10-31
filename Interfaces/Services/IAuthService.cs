using WebApplication1.Dtos;

namespace WebApplication1.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<Guid> SignUp(SignUpDto newUserData);

        public Task<string?> SignIn(SignInDto data, IResponseCookies cookies);

        public Task<string?> Refresh2TokenTypes(IResponseCookies cookies, string? refreshToken);
    }
}
