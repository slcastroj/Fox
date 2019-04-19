using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Fox.Models;
using Microsoft.EntityFrameworkCore;

namespace Fox.Models
{
    public interface IUsuarioService
    {
        TokenUser Authenticate(String user, String password);
        Usuario CreateUsuario(Usuario user);
        Usuario GetUsuarioByRut(String rut);
        IEnumerable<Usuario> GetAll();
        Usuario UpdateUsuarioByRut(String rut, Usuario user);
        Usuario DeleteUsuarioByRut(String rut);
    }

    public class UsuarioService : IUsuarioService
    {
        private AppSettings AppSettings { get; }
        private DeadlockContext Context { get; }
        public const UInt32 ExpirationSpan = 360;

        public UsuarioService(IOptions<AppSettings> settings, DeadlockContext ctx)
        {
            AppSettings = settings.Value;
            Context = ctx;
        }

        public TokenUser Authenticate(String rut, String pwd)
        {
            var user = Context.Usuario.SingleOrDefault(u => u.Rut.Equals(rut) && u.Password.Equals(Usuario.ComputeHash(pwd)));

            if(user == null) { return null; }

            var tokenHandle = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
            var date = DateTime.UtcNow;
            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Rut.ToString()),
                    new Claim(ClaimTypes.Role, user.Roles.ToString())
                }),
                IssuedAt = date,
                Expires = date.AddMinutes(ExpirationSpan),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandle.CreateToken(tokenDesc);
            var strToken = tokenHandle.WriteToken(token);
            var userDto = new TokenUser {
                Username = user.Rut,
                Token = strToken,
                Creacion = tokenDesc.IssuedAt.Value,
                Expiracion = (Int32)(tokenDesc.Expires.Value - tokenDesc.IssuedAt.Value).TotalSeconds
            };
            user.Token = strToken;
            Context.SaveChanges();

            return userDto;
        }

        public IEnumerable<Usuario> GetAll() => Context.Usuario.ToList();

        public Usuario CreateUsuario(Usuario user)
        {
            try
            {
                Context.Usuario.Add(user);
                Context.SaveChanges();
                return user;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Usuario GetUsuarioByRut(string rut)
        {
            try
            {
                return Context.Usuario.Where(u => u.Rut.Equals(rut)).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public Usuario UpdateUsuarioByRut(string rut, Usuario user)
        {
            try
            {
                Context.Entry(user).State = EntityState.Modified;
                Context.SaveChanges();
                return user;
            }
            catch
            {
                return null;
            }
        }

        public Usuario DeleteUsuarioByRut(string rut)
        {
            var u = GetUsuarioByRut(rut);
            if (u != null)
            {
                Context.Usuario.Remove(u);
                Context.SaveChanges();
            }
            return u;
        }
    }
}
