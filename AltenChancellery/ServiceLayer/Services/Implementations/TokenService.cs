﻿using AutoMapper;
using DBLayer.Models;
using DBLayer.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.DTOs;
using ServiceLayer.DTOs.Common;
using ServiceLayer.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServiceLayer.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public TokenService(IConfiguration config, IUnitOfWork uow, IRoleService roleService, IUserService userService, IMapper mapper)
        {
            _config = config;
            _mapper = mapper;

            _uow = uow;
            _roleService = roleService;
            _userService = userService;
        }

        public bool GenerateAndSaveTokens(User currentUser, out string accessToken, out string refreshToken)
        {
            IList<string> roles = _userService.GetRolesAsync(currentUser).Result;

            IdentityRole role = _roleService.GetHighestRoleOfUser(roles);

            // generate tokens
            string jwtToken = GenerateJwtAccessToken(currentUser, role);
            RefreshToken refToken = GenerateRefreshToken(currentUser);
            //

            // setting outputs via OUT parameter
            accessToken = jwtToken;
            refreshToken = refToken.Token;

            // save refresh token to DB
            return SaveRefreshToken(refToken);
        }

        public async Task<Tokens> RefreshToken(RefreshTokenDTO currentRefreshToken)
        {
            User? currentUser = await _uow.UserRepo.FindUserById(currentRefreshToken.UserId);

            IList<string> roleList = await _userService.GetRolesAsync(currentUser);

            IdentityRole? userRole = _roleService.GetHighestRoleOfUser(roleList);

            string jwtToken = GenerateJwtAccessToken(currentUser, userRole);
            return new Tokens() { AccessToken = jwtToken, RefreshToken = currentRefreshToken.Token };
        }

        public RefreshTokenDTO? GetTokenByStringValue(string refreshToken)
        {
            return _mapper.Map<RefreshTokenDTO>(_uow.RefreshTokenRepo.GetByTokenString(refreshToken));
        }

        public bool DeleteToken(string refreshToken)
        {
            RefreshTokenDTO tokenDTO = GetTokenByStringValue(refreshToken)!;

            RefreshToken token = _mapper.Map<RefreshToken>(tokenDTO);

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

        public async Task<UserDTO> GetUserByRefreshToken(string refreshToken)
        {
            string userId = _uow.RefreshTokenRepo.GetUserIdByTokenString(refreshToken);

            return _mapper.Map<UserDTO>(await _uow.UserRepo.FindUserById(userId));
        }

        private string GenerateJwtAccessToken(User user, IdentityRole role)
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

        private RefreshToken GenerateRefreshToken(User user)
        {
            return new RefreshToken()
            {
                ExpirationDate = DateTime.UtcNow.AddHours(Convert.ToDouble(_config["JWT:RefreshTokenExpiryInHours"])),
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                UserId = user.Id
            };
        }

        private bool SaveRefreshToken(RefreshToken token)
        {
            bool result = _uow.RefreshTokenRepo.CreateOrUpdate(token);

            if (result) _uow.SaveAsync();

            return result;
        }
    }
}
