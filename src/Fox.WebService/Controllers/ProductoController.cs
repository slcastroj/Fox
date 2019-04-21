using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fox.DataAccess.Models;
using Fox.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Transfer = Fox.WebService.Models.Transfer;

namespace Fox.WebService.Controllers
{
	[ApiController]
	[Route("api/v1/producto")]
	[Produces("application/json")]
	public class ProductoController : ControllerBase
	{
		private IProductoRepository Productos { get; set; }

		public ProductoController(IProductoRepository products)
		{
			Productos = products;
		}

		[HttpGet]
		public async Task<IActionResult> Get([FromBody]Transfer.Producto.Get data)
		{
			try
			{
				AccessResult<IEnumerable<Producto>> result = await Productos.GetAll();
				if(!result.IsValid) { return BadRequest(result.Fault); }

				IEnumerable<Producto> products = result.Result;
				if(data.Indice != null) { products = products.Where(x => x.Id > data.Indice); }
				if(data.TieneStock != null) { products = products.Where(x => x.Stock > 0); }
				if(data.Laboratorio != null) { products = products.Where(x => x.Laboratorio == data.Laboratorio); }
				if(data.Tipo != null) { products = products.Where(x => x.Tipo == data.Tipo); }
				if(data.Nombre != null) { products = products.Where(x => Regex.IsMatch(x.Nombre, data.Nombre)); }
				if(data.Cantidad != null) { products = products.Take(data.Cantidad.Value); }

				return Ok(products.Select(x => new
				{
					id = x.Id,
					nombre = x.Nombre,
					necesita_receta = x.NecesitaReceta == "1",
					maximo_semanal = x.MaximoSemanal,
					peso_gr = x.PesoGr,
					precio_unidad = x.PrecioUnidad,
					stock = x.Stock,
					laboratorio = x.Laboratorio,
					tipo = x.Tipo,
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

		[Authorize(Roles = "Administrador")]
		[HttpPost]
		public async Task<IActionResult> Post([FromBody]Transfer.Producto.Post data)
		{
			Producto item;
			try
			{
				item = new Producto
				{
					Nombre = data.Nombre,
					Descripcion = data.Descripcion,
					MaximoSemanal = data.MaximoSemanal,
					NecesitaReceta = data.NecesitaReceta ? "1" : "0",
					PesoGr = data.PesoGr,
					Tipo = data.Tipo,
					PrecioUnidad = data.PrecioUnidad,
					Stock = data.Stock ?? 0,
					Laboratorio = data.Laboratorio
				};

				AccessResult<Producto> result = await Productos.Post(item);
				if(!result.IsValid) { return BadRequest(result.Fault); }

				item = result.Result;
				return CreatedAtAction(nameof(GetById), new { id = item.Id }, new
				{
					id = item.Id,
					nombre = item.Nombre,
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

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute]Int32 id)
		{
			try
			{
				AccessResult<Producto> result = await Productos.Get(id);
				if(!result.IsValid && result.Result == null) { return NotFound(); }

				Producto item = result.Result;
				return Ok(new
				{
					id = item.Id,
					nombre = item.Nombre,
					necesita_receta = item.NecesitaReceta == "1",
					maximo_semanal = item.MaximoSemanal,
					peso_gr = item.PesoGr,
					precio_unidad = item.PrecioUnidad,
					stock = item.Stock,
					laboratorio = item.Laboratorio,
					tipo = item.Tipo
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
		[HttpPut("{id}")]
		public async Task<IActionResult> PutById([FromRoute]Int32 id, [FromBody]Transfer.Producto.PutById data)
		{
			try
			{
				AccessResult<Producto> result = await Productos.Get(id);
				if(!result.IsValid && result.Result == null) { return NotFound(); }

				result = await Productos.Put(new Producto
				{
					Id = id,
					Nombre = data.Nombre,
					Descripcion = data.Descripcion,
					MaximoSemanal = data.MaximoSemanal,
					NecesitaReceta = data.NecesitaReceta ? "1" : "0",
					PesoGr = data.PesoGr,
					Tipo = data.Tipo,
					PrecioUnidad = data.PrecioUnidad,
					Stock = data.Stock,
					Laboratorio = data.Laboratorio
				});
				if(!result.IsValid) { return BadRequest(result.Fault); }

				Producto item = result.Result;
				return Ok(new
				{
					id = item.Id,
					nombre = item.Nombre,
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
				AccessResult<Producto> result = await Productos.Get(id);
				if(!result.IsValid && result.Result == null) { return NotFound(); }

				result = await Productos.Delete(id);
				if(!result.IsValid) { return BadRequest(result.Fault); }

				Producto item = result.Result;
				return Ok(new
				{
					id = item.Id,
					nombre = item.Nombre,
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