using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Skeleton.ServiceName.Utils.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Skeleton.ServiceName.Utils.Security
{
    public class AccessManager : IAccessManager
    {
        private SigningConfigurations _signingConfigurations;
        private TokenConfigurations _tokenConfigurations;

        public AccessManager(
            SigningConfigurations signingConfigurations,
            TokenConfigurations tokenConfigurations)
        {
            _signingConfigurations = signingConfigurations;
            _tokenConfigurations = tokenConfigurations;
        }

        public string GenerateToken(SecurityUserModel model)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(JsonConvert.SerializeObject(model), "Login"),
                new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, model.Id.ToString()),
                        new Claim(ClaimTypes.Role, model.Profile)
                }
            );

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromHours(_tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });
            var token = handler.WriteToken(securityToken);

            return token;
        }

        public bool ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                ValidIssuer = _tokenConfigurations.Issuer,
                ValidAudience = _tokenConfigurations.Audience,
                IssuerSigningKey = _signingConfigurations.Key
            };

            SecurityToken validatedToken;
            try
            {
                tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            }
            catch (Exception)
            {
                return false;
            }

            return validatedToken != null;
        }
    }
}
