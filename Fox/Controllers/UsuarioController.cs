using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fox.DataAccess;
using Fox.DataAccess.Models;
using Fox.DataAccess.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Input = Fox.WebService.Models.Transfer.Input;

namespace Fox.Controllers
{
	[ApiController]
	[Route("api/v1/usuario")]
	[Produces("application/json")]
	public class UsuarioController : ControllerBase
	{
		private IUsuarioRepository Usuarios { get; }

		public UsuarioController(IUsuarioRepository users)
		{
			Usuarios = users;
		}

		[HttpPost("autenticacion")]
		public async Task<IActionResult> Autenticacion([FromBody]Input.Usuario.Autenticacion data)
		{
			try
			{
				AccessResult<BearerUser> result = await Usuarios.Authenticate(data.Rut, data.Password);
				return !result.IsValid ? BadRequest(result.Fault) : (IActionResult)Ok(result.Result);
			}
			catch(ArgumentNullException)
			{
				return BadRequest(new AccessFault("Solicitud incompleta o inválida"));
			}
			catch(Exception e)
			{
				return BadRequest(new AccessFault($"Error no controlado: {e.GetType()}"));
			}
		}

		[Authorize(Roles = "Farmaceutico, Administrador")]
		[HttpGet]
		public async Task<IActionResult> Get([FromBody]Input.Usuario.Get data)
		{
			try
			{
				AccessResult<IEnumerable<Usuario>> result = await Usuarios.GetAll();
				if(!result.IsValid) { return BadRequest(result.Fault); }

				IEnumerable<Usuario> users = result.Result;
				if(data.Roles != null) { users = users.Where(x => x.Roles == data.Roles); }
				if(data.Cantidad != null) { users = users.Take(data.Cantidad.Value); }

				return Ok(users.Select(x => new
				{
					rut = x.Rut,
					email = x.Email,
					fecha_nacimiento = x.FechaNacimiento,
					roles = x.Roles
				}));
			}
			catch(ArgumentNullException)
			{
				return BadRequest(new AccessFault("Solicitud incompleta o inválida"));
			}
			catch(Exception e)
			{
				return BadRequest(new AccessFault($"Error no controlado: {e.GetType()}"));
			}
		}

		[Authorize(Roles = "Farmaceutico, Administrador")]
		[HttpPost]
		public async Task<IActionResult> Post([FromBody]Input.Usuario.Post data)
		{
			Usuario user;
			try
			{
				user = new Usuario
				{
					Rut = data.Rut,
					Password = data.Password,
					Email = data.Email,
					FechaNacimiento = data.FechaNacimiento,
					Roles = data.Roles
				};

				AccessResult<Usuario> result = await Usuarios.Post(user);
				if(!result.IsValid) { return BadRequest(result.Fault); }

				user = result.Result;
				return CreatedAtAction(nameof(GetByRut), new { rut = user.Rut }, new
				{
					rut = user.Rut,
					email = user.Email,
					roles = user.Roles,
					creacion = DateTime.UtcNow.ToString()
				});
			}
			catch(ArgumentNullException)
			{
				return BadRequest(new AccessFault("Datos incompletos o inválidos"));
			}
			catch(Exception e)
			{
				return BadRequest(new AccessFault($"Error no controlado: {e.GetType()}"));
			}
		}

		[Authorize]
		[HttpGet("{rut}")]
		public async Task<IActionResult> GetByRut([FromRoute]String rut)
		{
			try
			{
				AccessResult<Usuario> result = await Usuarios.Get(rut);
				if(!result.IsValid && result.Result == null) { return NotFound(); }

				Usuario user = result.Result;
				return User.IsInRole("Usuario") && user.Rut != User.Identity.Name
					? Forbid()
					: (IActionResult)Ok(new
					{
						rut = user.Rut,
						email = user.Email,
						fecha_nacimiento = user.FechaNacimiento
					});
			}
			catch(ArgumentNullException)
			{
				return BadRequest(new AccessFault("Solicitud incompleta o inválida"));
			}
			catch(Exception e)
			{
				return BadRequest(new AccessFault($"Error no controlado: {e.GetType()}"));
			}
		}

		[Authorize]
		[HttpPatch("{rut}")]
		public async Task<IActionResult> PatchByRut([FromRoute]String rut, [FromBody]Input.Usuario.PatchByRut data)
		{
			try
			{
				if(User.IsInRole("Usuario") && rut != User.Identity.Name) { return Forbid(); }

				AccessResult<Usuario> result = await Usuarios.Patch(rut, data.Password, data.Email, data.FechaNacimiento);
				if(!result.IsValid) { return BadRequest(result.Fault); }

				Usuario user = result.Result;

				return Ok(new
				{
					rut = user.Rut,
					email = user.Email,
					fecha_nacimiento = user.FechaNacimiento,
					modificacion = DateTime.UtcNow.ToString()
				});
			}
			catch(ArgumentNullException)
			{
				return BadRequest(new AccessFault("Solicitud incompleta o inválida"));
			}
			catch(Exception e)
			{
				return BadRequest(new AccessFault($"Error no controlado: {e.GetType()}"));
			}
		}

		[Authorize(Roles = "Administrador")]
		[HttpPut("{rut}")]
		public async Task<IActionResult> PutByRut([FromRoute]String rut, [FromBody]Input.Usuario.PutByRut data)
		{
			try
			{
				AccessResult<Usuario> result = await Usuarios.Put(new Usuario
				{
					Rut = rut,
					Email = data.Email,
					FechaNacimiento = data.FechaNacimiento,
					Roles = data.Roles,
					Password = data.Password
				});
				if(!result.IsValid) { return BadRequest(result.Fault); }

				Usuario user = result.Result;
				return Ok(new
				{
					rut = user.Rut,
					email = user.Email,
					fecha_nacimiento = user.FechaNacimiento,
					roles = user.Roles,
					modificacion = DateTime.UtcNow.ToString()
				});
			}
			catch(ArgumentNullException)
			{
				return BadRequest(new AccessFault("Solicitud incompleta o inválida"));
			}
			catch(Exception e)
			{
				return BadRequest(new AccessFault($"Error no controlado: {e.GetType()}"));
			}
		}

		[Authorize(Roles = "Administrador")]
		[HttpDelete("{rut}")]
		public async Task<IActionResult> DeleteByRut([FromRoute]String rut)
		{
			try
			{
				AccessResult<Usuario> result = await Usuarios.Delete(rut);
				if(!result.IsValid) { return BadRequest(result.Fault); }

				Usuario user = result.Result;
				return Ok(new
				{
					rut = user.Rut,
					email = user.Email,
					fecha = DateTime.UtcNow.ToString()
				});
			}
			catch(ArgumentNullException)
			{
				return BadRequest(new AccessFault("Solicitud incompleta o inválida"));
			}
			catch(Exception e)
			{
				return BadRequest(new AccessFault($"Error no controlado: {e.GetType()}"));
			}
		}
	}
}