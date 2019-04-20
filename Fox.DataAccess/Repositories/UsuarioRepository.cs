using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fox.DataAccess.Models;
using Fox.DataAccess.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Fox.DataAccess.Repositories
{
	public interface IUsuarioRepository
	{
		Task<AccessResult<BearerUser>> Authenticate(String user, String password);
		Task<AccessResult<IEnumerable<Usuario>>> GetAll();
		Task<AccessResult<Usuario>> Get(String rut);
		Task<AccessResult<Usuario>> Post(Usuario user);
		Task<AccessResult<Usuario>> Patch(String rut, String password, String email, String fechaNacimiento);
		Task<AccessResult<Usuario>> Put(Usuario user);
		Task<AccessResult<Usuario>> Delete(String rut);
	}

	public class UsuarioRepository : IUsuarioRepository
	{
		private AppSettings Settings { get; }
		private DeadlockContext Context { get; }
		private DbSet<Usuario> Users => Context.Usuario;
		public const UInt32 ExpirationSpan = 86400;

		public UsuarioRepository(IOptions<AppSettings> settings, DeadlockContext ctx)
		{
			Settings = settings.Value;
			Context = ctx;
		}

		public async Task<AccessResult<BearerUser>> Authenticate(String rut, String pwd)
		{
			try
			{
				Usuario user = await Get(rut);

				if(user == null) { return new AccessFault("Usuario no existente"); }
				if(user.Password != Hashing.ComputeSha256(pwd)) { return new AccessFault("Credenciales inválidas"); }

				BearerUser token = Token.GetFor(Settings.Secret, user, ExpirationSpan);
				if(token == null) { return new AccessFault("No se pudo crear token de acceso"); }

				return token;
			}
			catch { return new AccessFault("No se pudo autenticar al usuario"); }
		}

		public async Task<AccessResult<IEnumerable<Usuario>>> GetAll()
		{
			return await Users.ToListAsync();
		}

		public async Task<AccessResult<Usuario>> Get(String rut)
		{
			try
			{
				return await Users.FindAsync(rut);
			}
			catch { return new AccessFault("No se pudo obtener la información de usuario"); }
		}

		public async Task<AccessResult<Usuario>> Post(Usuario user)
		{
			try
			{
				if((await Get(user.Rut)).Result != null) { return new AccessFault("Usuario ya existente"); }

				EnsureIntegrity(user);

				await Context.Usuario.AddAsync(user);
				await Context.SaveChangesAsync();

				return user;
			}
			catch { return new AccessFault("No se pudo publicar la información de usuario"); }
		}

		public async Task<AccessResult<Usuario>> Patch(String rut, String password, String email, String fechaNacimiento)
		{
			try
			{
				Usuario user = await Get(rut);
				if(user == null) { return new AccessFault("Usuario no existente"); }

				if(password != null) { user.Password = password; }
				if(email != null) { user.Email = email; }
				if(fechaNacimiento != null) { user.FechaNacimiento = fechaNacimiento; }

				EnsureIntegrity(user);

				await Context.SaveChangesAsync();

				return user;
			}
			catch { return new AccessFault("No se pudo actualizar la información de usuario"); }
		}

		public async Task<AccessResult<Usuario>> Put(Usuario user)
		{
			try
			{
				EnsureIntegrity(user);

				Context.Entry(user).State = EntityState.Modified;
				await Context.SaveChangesAsync();
				return user;
			}
			catch { return new AccessFault("No se pudo actualizar la información de usuario"); }
		}

		public async Task<AccessResult<Usuario>> Delete(String rut)
		{
			try
			{
				Usuario user = await Get(rut);
				if(user == null) { return new AccessFault("Usuario no existente"); }

				Users.Remove(user);
				await Context.SaveChangesAsync();
				return user;
			}
			catch { return new AccessFault("No se pudo eliminar la información de usuario"); }
		}

		private AccessResult<Usuario> EnsureIntegrity(Usuario user)
		{
			user.Rut = user.Rut.ToLower();
			user.Email = user.Email.ToLower();
			user.Roles = user.Roles ?? "Usuario";
			user.Password = Hashing.ComputeSha256(user.Password);
			user.FechaNacimiento = DateTime.Parse(user.FechaNacimiento).ToString("yyyy-MM-dd");

			return user;
		}
	}
}
