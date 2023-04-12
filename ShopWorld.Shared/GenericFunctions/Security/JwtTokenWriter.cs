using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.Shared
{
    public class JwtTokenWriter
    {
        public static JwtSecurityToken WriteToken(string Secret, string ValidIssuer, string ValidAudience, DateTime Expires, List<Claim> Claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: ValidIssuer,
                audience: ValidAudience,
                expires: Expires,
                claims: Claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return token;
        }

        public static string WriteTokenAsString(string Secret, string ValidIssuer, string ValidAudience, DateTime Expires, List<Claim> Claims)
        {
            JwtSecurityToken token = WriteToken(Secret, ValidIssuer, ValidAudience, Expires, Claims);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
