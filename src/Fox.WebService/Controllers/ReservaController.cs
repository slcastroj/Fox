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
	[Route("api/v1/reserva")]
	[Produces("application/json")]
	public class ReservaController : ControllerBase
	{
		private IReservaRepository Reservas { get; set; }

		public ReservaController(IReservaRepository ordenes)
		{
			Reservas = ordenes;
		}

		[Authorize(Roles = "Farmaceutico, Administrador")]
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery]Transfer.Reserva.Get data)
		{
			try
			{
				AccessResult<IEnumerable<Reserva>> result = await Reservas.GetAll();
				if(!result.IsValid) { return BadRequest(result.Fault); }

				IEnumerable<Reserva> products = result.Result;	
				if(data.Usuario != null) { products = products.Where(x => x.Usuario == data.Usuario); }
				if(data.Estado != null) {
					products = products.Where(
						x => x.Compra.FirstOrDefault().Estado == data.Estado
					);
				}
				if(data.Producto != null) { products = products.Where(x => x.ReservaStock.Any(y => y.Producto == data.Producto)); }

				return Ok(products.Select(x => new
				{
					id = x.Id,
					usuario = x.Usuario,
					compra = x.Compra.FirstOrDefault()?.Id,
					reservas = x.ReservaStock.Select(y => new { cantidad = y.Cantidad, producto = y.Producto})
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
		[HttpPost]
		public async Task<IActionResult> Post([FromBody]Transfer.Reserva.Post data)
		{
			try
			{
				var item = new Reserva{Usuario = data.Usuario};

				var reservas = new List<ReservaStock>(data.Reservas.Select(x => new ReservaStock()
				{
					Producto = x.Producto,
					Cantidad = x.Cantidad
				}));

				AccessResult<Reserva> result = await Reservas.Post(item, reservas);
				if(!result.IsValid) { return BadRequest(result.Fault); }

				item = result.Result;
				return CreatedAtAction(nameof(GetById), new { id = item.Id }, new
				{
					id = item.Id,
					usuario = item.Usuario,
					reservas = item.ReservaStock.Select(y => new { cantidad = y.Cantidad, producto = y.Producto }),
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
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute]Int32 id)
		{
			try
			{
				AccessResult<Reserva> result = await Reservas.Get(id);
				if(!result.IsValid && result.Result == null) { return NotFound(); }

				Reserva item = result.Result;
				return User.IsInRole("Usuario") && item.Usuario != User.Identity.Name
					? Forbid()
					: (IActionResult)Ok(new
					{
						id = item.Id,
						usuario = item.Usuario,
						compra = item.Compra.FirstOrDefault()?.Id,
						reservas = item.ReservaStock.Select(y => new { cantidad = y.Cantidad, producto = y.Producto })
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
				AccessResult<Reserva> result = await Reservas.Delete(id);
				if(!result.IsValid) { return BadRequest(result.Fault); }

				Reserva item = result.Result;
				return Ok(new
				{
					id = item.Id,
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