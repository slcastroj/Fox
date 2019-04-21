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
	[Route("api/v1/laboratorio")]
	[Produces("application/json")]
	public class LaboratorioController : ControllerBase
	{
		private ILaboratorioRepository Laboratorios { get; set; }

		public LaboratorioController(ILaboratorioRepository ordenes)
		{
			Laboratorios = ordenes;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				AccessResult<IEnumerable<Laboratorio>> result = await Laboratorios.GetAll();
				if (!result.IsValid) { return BadRequest(result.Fault); }

				return Ok(result.Result.Select(x => new
				{
					id = x.Id,
					nombre = x.Nombre
				}));
			}
			catch (ArgumentNullException)
			{
				return BadRequest(new AccessFault("Solicitud incompleta o inválida"));
			}
			catch (Exception e)
			{
				return BadRequest(new AccessFault($"Error no controlado: {e.GetType()}"));
			}
		}

		[Authorize(Roles = "Administrador")]
		[HttpPost]
		public async Task<IActionResult> Post([FromBody]Transfer.PostEnum data)
		{
			Laboratorio item;
			try
			{
				item = new Laboratorio { Nombre = data.Nombre };

				AccessResult<Laboratorio> result = await Laboratorios.Post(item);
				if (!result.IsValid) { return BadRequest(result.Fault); }

				item = result.Result;
				return CreatedAtAction(nameof(GetById), new { id = item.Id }, new
				{
					id = item.Id,
					nombre = item.Nombre,
					creacion = DateTime.UtcNow.ToString()
				});
			}
			catch (ArgumentNullException)
			{
				return BadRequest(new AccessFault("Datos incompletos o inválidos"));
			}
			catch (Exception e)
			{
				return BadRequest(new AccessFault($"Error no controlado: {e.GetType()}"));
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute]Int32 id)
		{
			try
			{
				AccessResult<Laboratorio> result = await Laboratorios.Get(id);
				if (!result.IsValid && result.Result == null) { return NotFound(); }

				return Ok(new
				{
					id = result.Result.Id,
					nombre = result.Result.Nombre
				});
			}
			catch (ArgumentNullException)
			{
				return BadRequest(new AccessFault("Solicitud incompleta o inválida"));
			}
			catch (Exception e)
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
				AccessResult<Laboratorio> result = await Laboratorios.Delete(id);
				if (!result.IsValid) { return BadRequest(result.Fault); }

				Laboratorio item = result.Result;
				return Ok(new
				{
					id = item.Id,
					nombre = item.Nombre,
					fecha = DateTime.UtcNow.ToString()
				});
			}
			catch (ArgumentNullException)
			{
				return BadRequest(new AccessFault("Solicitud incompleta o inválida"));
			}
			catch (Exception e)
			{
				return BadRequest(new AccessFault($"Error no controlado: {e.GetType()}"));
			}
		}
	}
}