using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TourManagementSystem.Migrations;
using TourManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TourManagementSystem.Models.Entities;


namespace TourManagementSystem.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _Key;
        private readonly UserManager<User> _userManager;
        public TokenService(IConfiguration configuration, UserManager<User> userManager)
        {
            _userManager = userManager;
            _config = configuration;
            _Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
        }

        public async Task<string> CreateToken(User user)
        {

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            var claims = new List<Claim> {
            new  Claim(JwtRegisteredClaimNames.Sub,user.UserID.ToString()),
            new  Claim(JwtRegisteredClaimNames.GivenName,user.UserName),
            new  Claim(JwtRegisteredClaimNames.Email,user.Email),
            new  Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role)
            };


            var creds = new SigningCredentials(_Key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(60),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
