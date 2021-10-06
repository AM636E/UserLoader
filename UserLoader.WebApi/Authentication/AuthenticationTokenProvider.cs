using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UserLoader.WebApi.Authentication
{
    public class AuthenticationTokenProvider : IAuthenticationTokenProvider
    {
        private readonly JwtConfiguration _jwtConfiguration;

        public AuthenticationTokenProvider(IOptions<JwtConfiguration> jwtOptions)
        {
            _jwtConfiguration = jwtOptions.Value;
        }

        public string GenerateAccessToken(string name)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, name),
            };

            var token = new JwtSecurityToken(_jwtConfiguration.Issuer,
                _jwtConfiguration.Issuer,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_jwtConfiguration.Lifetime)),
                signingCredentials: creds, claims: claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
