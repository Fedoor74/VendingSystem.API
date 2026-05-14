using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VendingFranchisee.Desktop.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using VendingFranchisee.Desktop.Models;

namespace VendingFranchisee.Desktop.Helpers
{
    public static class JwtHelper
    {
        public static ApiUser DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            return new ApiUser
            {
                Token = token,
                UserId = int.Parse(jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value),
                FullName = jwtToken.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value ?? "Unknown",
                Email = jwtToken.Claims.First(c => c.Type == ClaimTypes.Email).Value,
                Role = jwtToken.Claims.First(c => c.Type == ClaimTypes.Role).Value
            };
        }
    }
}