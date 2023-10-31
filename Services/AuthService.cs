using AutoMapper;
using System.Security.Claims;
using System.Security.Cryptography;
using WebApplication1.Dtos;
using WebApplication1.Entities;
using WebApplication1.Interfaces.Repositories;
using WebApplication1.Interfaces.Services;
using WebApplication1.UnitOfWork;
using WebApplication1.Utils.Encryption;
using WebApplication1.Utils.Jwt;

namespace WebApplication1.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshRepository;
        private readonly string _secretKey;

        public AuthService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IConfiguration configuration
        ) : base(mapper, unitOfWork)
        {
            _userRepository = unitOfWork.UserRepository;
            _refreshRepository = unitOfWork.RefreshTokenRepository;
            _secretKey = configuration.GetSection("JwtSecretKey").Value!;
        }

        public async Task<Guid> SignUp(SignUpDto newUserData)
        {
            var user = _mapper.Map<UserEntity>(newUserData);
            var id = Guid.NewGuid();

            user.Id = id;
            user.Password = Bcrypt.HashPassword(user.Password);

            _userRepository.Create(user);
            await _unitOfWork.SaveChangesAsync();

            return id;
        }

        public async Task<string?> SignIn(SignInDto data, IResponseCookies cookies)
        {
            var user = _userRepository.GetOne((user) => user.Username == data.Username);

            if (user == null)
            {
                return null;
            }

            if (Bcrypt.Verify(data.Password, user.Password))
            {
                return null;
            }

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var refreshToken = await GenerateRefreshToken(user);
            SetRefresh2Token(cookies, refreshToken);

            return Jwt.GenerateToken(claims, _secretKey);
        }

        private async Task<RefreshTokenEntity> GenerateRefreshToken(UserEntity user)
        {
            var refreshToken = new RefreshTokenEntity()
            {
                User = user,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expiry = DateTime.Now.AddMonths(1)
            };

            _refreshRepository.Create(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return refreshToken;
        }

        private void SetRefresh2Token(IResponseCookies cookies, RefreshTokenEntity refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expiry,
                Secure = true
            };

            cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }

        public async Task<string?> Refresh2TokenTypes(IResponseCookies cookies, string? refreshToken)
        {
            if (refreshToken == null)
            {
                return null;
            }

            var token = _refreshRepository.GetOne(
                (rt) => rt.Token == refreshToken && rt.IsRevoked != true && rt.Expiry > DateTime.Now,
                (rt) => new RefreshTokenEntity() { User = rt.User }
            );

            if (token == null)
            {
                return null;
            }

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, token.User.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var newRefreshToken = await GenerateRefreshToken(token.User);
            SetRefresh2Token(cookies, newRefreshToken);

            return Jwt.GenerateToken(claims, _secretKey);
        }
    }
}
