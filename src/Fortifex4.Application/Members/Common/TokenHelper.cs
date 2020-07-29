using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fortifex4.Domain.Constants;
using Fortifex4.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Fortifex4.Application.Members.Common
{
    public static class TokenHelper
    {
        public static string GenerateToken(Member member, string securityKey)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            if (string.IsNullOrEmpty(member.PictureURL))
            {
                member.PictureURL = "fortifex-user.png";
            }

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, member.MemberUsername));
            claims.Add(new Claim(ClaimType.PictureUrl, member.PictureURL));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }
}