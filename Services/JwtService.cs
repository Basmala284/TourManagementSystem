using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TourManagementSystem.DTOs;
using TourManagementSystem.Models.Entities;

namespace TourManagementSystem.Services;


    public class JwtService(UserManager<User> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        ILogger<JwtService> logger,
        IConfiguration config)
    {
        public async Task<AuthResponseDto> GenerateJWTokenAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            var claims = new List<Claim>
        {
            new Claim("ID", userId),
            new Claim(ClaimTypes.Name, user!.UserName!),
        };

            var userClaims = await userManager.GetClaimsAsync(user);

            claims.AddRange(userClaims);

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));

                var role = await roleManager.FindByNameAsync(userRole);


                var roleClaims = await roleManager.GetClaimsAsync(role);

                foreach (Claim roleClaim in roleClaims)
                {
                    claims.Add(roleClaim);
                }
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(config["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(config["JWT:TokenExpireInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature),
                Issuer = config["JWT:Issuer"],
                Audience = config["JWT:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            logger.LogInformation($"Generated JWT for user {user.UserName}:");
            logger.LogInformation($"Token: {tokenHandler.WriteToken(token)}");
            logger.LogInformation($"Expires at: {tokenDescriptor.Expires}");

            return new AuthResponseDto()
            {
                Token = tokenHandler.WriteToken(token),
                UserName = user.UserName,
            };
        }

    }




