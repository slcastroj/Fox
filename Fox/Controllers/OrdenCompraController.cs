using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fox.DataAccess.Models;
using Fox.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Input = Fox.WebService.Models.Transfer.Input;

namespace Fox.WebService.Controllers
{
	[ApiController]
	[Route("api/v1/compra")]
	[Produces("application/json")]
	public class OrdenCompraController : ControllerBase
	{
		private IOrdenCompraRepository Ordenes { get; set; }

		public OrdenCompraController(IOrdenCompraRepository ordenes)
		{
			Ordenes = ordenes;
		}

		[Authorize(Roles = "Farmaceutico, Administrador")]
		[HttpGet]
		public async Task<IActionResult> Get([FromBody]Input.OrdenCompra.Get data)
		{
			try
			{
				AccessResult<IEnumerable<OrdenCompra>> result = await Ordenes.GetAll();
				if(!result.IsValid) { return BadRequest(result.Fault); }

				IEnumerable<OrdenCompra> products = result.Result;
				if(data.Usuario != null) { products = products.Where(x => x.Usuario == data.Usuario); }
				if(data.Asignado != null) { products = products.Where(x => x.Asignado == data.Asignado); }
				if(data.Estado != null) { products = products.Where(x => x.Estado == data.Estado); }
				if(data.Producto != null) { products = products.Where(x => x.Producto == data.Producto); }

				return Ok(products.Select(x => new
				{
					id = x.Id,
					usuario = x.Usuario,
					asignado = x.Asignado,
					producto = x.Producto,
					cantidad = x.Cantidad,
					estado = x.Estado
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
		public async Task<IActionResult> Post([FromBody]Input.OrdenCompra.Post data)
		{
			OrdenCompra item;
			try
			{
				item = new OrdenCompra
				{
					Usuario = data.Usuario,
					Producto = data.Producto.Value,
					Cantidad = data.Cantidad.Value
				};

				AccessResult<OrdenCompra> result = await Ordenes.Post(item);
				if(!result.IsValid) { return BadRequest(result.Fault); }

				item = result.Result;
				return CreatedAtAction(nameof(GetById), new { id = item.Id }, new
				{
					id = item.Id,
					usuario = item.Usuario,
					cantidad = item.Cantidad,
					producto = item.Producto,
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
				AccessResult<OrdenCompra> result = await Ordenes.Get(id);
				if(!result.IsValid && result.Result == null) { return NotFound(); }

				OrdenCompra item = result.Result;
				return User.IsInRole("Usuario") && item.Usuario != User.Identity.Name
					? Forbid()
					: (IActionResult)Ok(new
					{
						id = item.Id,
						usuario = item.Usuario,
						asignado = item.Asignado,
						producto = item.Producto,
						cantidad = item.Cantidad,
						estado = item.Estado
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
		public async Task<IActionResult> PatchById([FromRoute]Int32 id, [FromBody]Input.OrdenCompra.PatchById data)
		{
			try
			{
				AccessResult<OrdenCompra> result = await Ordenes.Get(id);
				if(!result.IsValid && result.Result == null) { return NotFound(); }

				OrdenCompra item = result.Result;

				if(User.IsInRole("Usuario") &&
					(item.Usuario != User.Identity.Name ||
					data.Estado != 2)) { return Forbid(); }

				result = await Ordenes.Patch(id, data.Estado);
				if(!result.IsValid) { return BadRequest(result.Fault); }

				item = result.Result;
				return Ok(new
				{
					id = item.Id,
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

		[Authorize(Roles = "Farmaceutico, Administrador")]
		[HttpPut("{id}")]
		public async Task<IActionResult> PutById([FromRoute]Int32 id, [FromBody]Input.OrdenCompra.PutById data)
		{
			try
			{
				AccessResult<OrdenCompra> result = await Ordenes.Get(id);
				if(!result.IsValid && result.Result == null) { return NotFound(); }

				result = await Ordenes.Put(new OrdenCompra
				{
					Id = id,
					Usuario = data.Usuario,
					Asignado = data.Asignado,
					Cantidad = data.Cantidad,
					Estado = data.Estado,
					Producto = data.Estado
				});
				if(!result.IsValid) { return BadRequest(result.Fault); }

				OrdenCompra item = result.Result;
				return Ok(new
				{
					id = item.Id,
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
				AccessResult<OrdenCompra> result = await Ordenes.Delete(id);
				if(!result.IsValid) { return BadRequest(result.Fault); }

				OrdenCompra item = result.Result;
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