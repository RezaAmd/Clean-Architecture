using Application.Models;
using Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Common.Services
{
    public class JwtService : IJwtService
    {
        #region constructor
        private readonly ILogger<JwtService> logger;

        public JwtService(ILogger<JwtService> _logger)
        {
            logger = _logger;
        }
        #endregion

        public (Result Status, string Token) GenerateToken(List<Claim> claims, DateTime? expire)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(JwtConfig.secretKey);
                var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.encryptionKey)),
                    SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature),
                    EncryptingCredentials = encryptingCredentials
                };
                if (expire.HasValue)
                    tokenDescriptor.Expires = expire.Value;
                return (Result.Success, tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor)));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                return (Result.Failed(), null);
            }
        }
    }

    public static class JwtConfig
    {
        public static readonly string secretKey = "R6Z9a18$m27i9R63";
        public static readonly string encryptionKey = "9i6r3@48Kh3d96m3";
    }
}