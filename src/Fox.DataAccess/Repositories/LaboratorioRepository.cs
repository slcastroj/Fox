using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fox.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Fox.DataAccess.Repositories
{
	public interface ILaboratorioRepository
	{
		Task<AccessResult<IEnumerable<Laboratorio>>> GetAll();
		Task<AccessResult<Laboratorio>> Get(Int64 id);
		Task<AccessResult<Laboratorio>> Post(Laboratorio user);
		Task<AccessResult<Laboratorio>> Delete(Int64 id);
	}
	public class LaboratorioRepository : ILaboratorioRepository
	{
		private DeadlockContext Context { get; }
		private DbSet<Laboratorio> Laboratorios => Context.Laboratorio;

		public LaboratorioRepository(DeadlockContext ctx)
		{
			Context = ctx;
		}

		public async Task<AccessResult<IEnumerable<Laboratorio>>> GetAll()
		{
			return await Laboratorios.ToListAsync();
		}

		public async Task<AccessResult<Laboratorio>> Get(Int64 id)
		{
			try
			{
				return await Laboratorios.FindAsync(id);
			}
			catch { return new AccessFault("No se pudo obtener la información de laboratorio"); }
		}

		public async Task<AccessResult<Laboratorio>> Post(Laboratorio item)
		{
			try
			{
				IEnumerable<Laboratorio> Estados = (await GetAll()).Result;
				var lastIndex = Estados.Count() > 0 ? Estados.Max(x => x.Id) : 0;
				item.Id = lastIndex + 1;
				if ((await Get(item.Id)).Result != null) { return new AccessFault("Laboratorio ya existente"); }

				await Context.Laboratorio.AddAsync(item);
				await Context.SaveChangesAsync();

				return item;
			}
			catch { return new AccessFault("No se pudo publicar la información de laboratorio"); }
		}

		public async Task<AccessResult<Laboratorio>> Delete(Int64 id)
		{
			try
			{
				Laboratorio user = await Get(id);
				if (user == null) { return new AccessFault("Laboratorio no existente"); }

				Laboratorios.Remove(user);
				await Context.SaveChangesAsync();
				return user;
			}
			catch { return new AccessFault("No se pudo eliminar la información de laboratorio"); }
		}
	}
}
