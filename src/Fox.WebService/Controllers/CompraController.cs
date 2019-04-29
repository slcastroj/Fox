using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fox.DataAccess.Models;
using Fox.DataAccess.Repositories;
using Fox.WebService.Models.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Transfer = Fox.WebService.Models.Transfer;

namespace Fox.WebService.Controllers
{
	[ApiController]
	[Route("api/v1/compra")]
	[Produces("application/json")]
	public class CompraController : ControllerBase
	{
		private ICompraRepository Compras { get; set; }

		public CompraController(ICompraRepository ordenes)
		{
			Compras = ordenes;
		}

		[Authorize(Roles = "Farmaceutico, Administrador")]
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery]Transfer.Compra.Get data)
		{
			try
			{
				AccessResult<IEnumerable<Compra>> result = await Compras.GetAll();
				if(!result.IsValid) { return BadRequest(result.Fault); }

				IEnumerable<Compra> products = result.Result;
				if(data.Usuario != null) { products = products.Where(x => x.ReservaNavigation.Usuario == data.Usuario); }
				if(data.Estado != null) { products = products.Where(x => x.Estado == data.Estado); }
				if(data.Producto != null) { products = products.Where(x => x.ReservaNavigation.ReservaStock.Any(y => y.Producto == data.Producto)); }

				return Ok(products.Select(x => new
				{
					id = x.Id,
					estado = x.Estado,
					reserva = new
					{
						id = x.ReservaNavigation.Id,
						usuario = x.ReservaNavigation.Usuario,
						reservas = x.ReservaNavigation.ReservaStock.Select(y => new
						{
							cantidad = y.Cantidad,
							producto = y.Producto
						})
					}
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

		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute]Int32 id)
		{
			try
			{
				AccessResult<Compra> result = await Compras.Get(id);
				if(!result.IsValid && result.Result == null) { return NotFound(); }

				Compra item = result.Result;
				return User.IsInRole("Usuario") && item.ReservaNavigation.Usuario != User.Identity.Name
					? Forbid()
					: (IActionResult)Ok(new
					{
						id = item.Id,
						estado = item.Estado,
						reserva = new
						{
							id = item.ReservaNavigation.Id,
							usuario = item.ReservaNavigation.Usuario,
							reservas = item.ReservaNavigation.ReservaStock.Select(y => new
							{
								cantidad = y.Cantidad,
								producto = y.Producto
							})
						}
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
		[HttpPatch("{id}")]
		public async Task<IActionResult> PatchById([FromRoute]Int32 id, [FromBody]Transfer.Compra.PatchById data)
		{
			try
			{
				AccessResult<Compra> result = await Compras.Get(id);
				if(!result.IsValid && result.Result == null) { return NotFound(); }

				Compra item = result.Result;

				if(User.IsInRole("Usuario") &&
					(item.ReservaNavigation.Usuario != User.Identity.Name ||
					data.Estado != 2)) { return Forbid(); }

				result = await Compras.Patch(id, data.Estado);
				if(!result.IsValid) { return BadRequest(result.Fault); }

				item = result.Result;
				return Ok(new
				{
					id = item.Id,
					reserva = item.Reserva,
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
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteById([FromRoute]Int32 id)
		{
			try
			{
				AccessResult<Compra> result = await Compras.Delete(id);
				if(!result.IsValid) { return BadRequest(result.Fault); }

				Compra item = result.Result;
				return Ok(new
				{
					id = item.Id,
					reserva = item.Reserva,
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