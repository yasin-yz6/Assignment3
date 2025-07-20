using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using University.Core.DTOs;

namespace University.Api.Helpers
{
    public class JwtTokenHelper : IJwtTokenHelper
    {
        //let's take the configuration from our app settings
        private readonly IConfiguration _config;

        public JwtTokenHelper(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(UserDTO user)
        {
            //like we did in Program.cs we get the settings from configuration
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);//another algorithm

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Role, "Teacher")
            };

            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),//we put the claims and create new claimsIdentity
                Expires = DateTime.UtcNow.AddDays(1),//Expires in one day
                SigningCredentials = creds,//takes the secret key and the algorithm that gonna be used
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(TokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }

    public interface IJwtTokenHelper
    {
        string GenerateToken(UserDTO user);
    }
}
//to generate tokens to the user