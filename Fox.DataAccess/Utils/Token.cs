using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fox.DataAccess.Models;
using Microsoft.IdentityModel.Tokens;

namespace Fox.DataAccess.Utils
{
	public static class Token
	{
		public static BearerUser GetFor(String secret, Usuario user, UInt32 expiration = 1800)
		{
			if(secret == null || user == null) { return null; }

			try
			{
				var tokenHandle = new JwtSecurityTokenHandler();
				var key = Encoding.ASCII.GetBytes(secret);
				var date = DateTime.UtcNow;

				var tokenDesc = new SecurityTokenDescriptor
				{
					Subject = new ClaimsIdentity(new[]
					{
						new Claim(ClaimTypes.Name, user.Rut.ToString()),
						new Claim(ClaimTypes.Role, user.Roles.ToString())
					}),
					IssuedAt = date,
					Expires = date.AddSeconds(expiration),
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
				};

				return new BearerUser()
				{
					Rut = user.Rut,
					Token = tokenHandle.WriteToken(tokenHandle.CreateToken(tokenDesc)),
					Creacion = date,
					Expiracion = expiration
				};
			}
			catch { return null; }
		}
	}
}
