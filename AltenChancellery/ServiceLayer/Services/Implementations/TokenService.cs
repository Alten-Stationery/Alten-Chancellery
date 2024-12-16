using DBLayer.Models;
using DBLayer.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["JWT:AccessTokenExpirationInMinutes"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken(User user)
        {
            return new RefreshToken()
            {
                ExpirationDate = DateTime.UtcNow.AddHours(Convert.ToDouble(_config["JWT:RefreshTokenExpiryInHours"])),
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                UserId = user.Id
            };
        }

        public async Task<Tokens> RefreshToken(RefreshToken currentRefreshToken)
        {
            User? currentUser = await _uow.UserRepo.FindUserById(currentRefreshToken.UserId);

            IList<string> roleList = await _userService.GetRolesAsync(currentUser);

            IdentityRole? userRole = _roleService.GetHighestRoleOfUser(roleList);

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

            if (result) _uow.SaveAsync();

            return result;
        }

        public bool IsRefreshTokenValid(string refreshToken)
        {
            RefreshToken? token = _uow.RefreshTokenRepo.GetByTokenString(refreshToken);

            if (token == null) return false;

            if (token.ExpirationDate < DateTime.UtcNow) return false;

            return true;
        }

        public bool IsAccessTokenValid(string accessToken)
        {
            JwtSecurityToken token = new JwtSecurityToken(accessToken);

            return token.ValidTo > DateTime.UtcNow;
        }

        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            string userId = _uow.RefreshTokenRepo.GetUserIdByTokenString(refreshToken);

            return await _uow.UserRepo.FindUserById(userId);
        }
    }
}
