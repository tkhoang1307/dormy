using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.ResponseModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dormy.WebService.Api.Infrastructure.TokenRetriever
{
    public class TokenRetriever : ITokenRetriever
    {
        private readonly IConfiguration _configuration;

        public TokenRetriever(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(JwtResponseModel jwtReponseModel)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var jwtKeyString = _configuration["Jwt:Key"];

            var jwtIssuer = _configuration["Jwt:Issuer"];

            var jwtAudience = _configuration["Jwt:Audience"];

            var jwtKeyBytes = Encoding.UTF8.GetBytes(jwtKeyString ?? string.Empty);

            var tokenDesc = new SecurityTokenDescriptor
            {
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, jwtReponseModel.UserId.ToString()),
                    new Claim(ClaimTypes.GivenName, jwtReponseModel.FirstName),
                    new Claim(ClaimTypes.Surname, jwtReponseModel.LastName),
                    new Claim(ClaimTypes.Email, jwtReponseModel.Email),
                    new Claim(ClaimTypes.Role, jwtReponseModel.Role),
                    new Claim(ClaimTypes.Name, jwtReponseModel.UserName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(60 * 24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDesc);

            return jwtTokenHandler.WriteToken(token);
        }
    }
}
