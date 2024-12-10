using DBLayer.Models;
using DBLayer.Repositories.Interfaces;
using DBLayer.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.Constants.Auth;
using ServiceLayer.DTOs;
using ServiceLayer.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServiceLayer.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public TokenService(IConfiguration config, IUnitOfWork uow, IRoleService roleService, IUserService userService)
        {
            _config = config;

            _uow = uow;
            _roleService = roleService;
            _userService = userService;
        }

        public bool SaveRefreshToken(RefreshToken token)
        {
            bool result = _uow.RefreshTokenRepo.CreateOrUpdate(token);

            if (result) _uow.Save();

            return result;
        }

        public string GenerateJwtAccessToken(User user, IdentityRole role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, role.Name!)
            };

            JwtSecurityToken token = new JwtSecurityToken(
                _config["JWT:Issuer"],
                _config["JWT:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["AccessTokenExpirationInMinutes"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken(User user)
        {
            return new RefreshToken()
            {
                //ExpirationDate = DateTime.UtcNow.AddHours(Convert.ToDouble(_config["RefreshTokenExpiryInDays"])),
                ExpirationDate = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["RefreshTokenExpiryInMinutes"])),
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                UserId = user.Id
            };
        }

        public async Task<Tokens> RefreshToken(RefreshToken currentRefreshToken)
        {
            User? currentUser = await _uow.UserRepo.FindUserById(currentRefreshToken.UserId);
            IdentityRole? userRole = _roleService.GetHighestRoleOfUser(await _userService.GetRolesAsync(currentUser));

            string jwtToken = GenerateJwtAccessToken(currentUser, userRole);
            return new Tokens() { AccessToken = jwtToken, RefreshToken = currentRefreshToken.Token };
        }

        public RefreshToken? GetTokenByStringValue(string refreshToken)
        {
            return _uow.RefreshTokenRepo.GetByTokenString(refreshToken);
        }

        public bool DeleteToken(string refreshToken)
        {
            RefreshToken token = GetTokenByStringValue(refreshToken)!;

            bool result = _uow.RefreshTokenRepo.Delete(token);

            if (result) _uow.Save();

            return result;
        }
    }
}
