using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fox.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Fox.DataAccess.Repositories
{
	public interface IEstadoCompraRepository
	{
		Task<AccessResult<IEnumerable<EstadoCompra>>> GetAll();
		Task<AccessResult<EstadoCompra>> Get(Int64 id);
		Task<AccessResult<EstadoCompra>> Post(EstadoCompra user);
		Task<AccessResult<EstadoCompra>> Delete(Int64 id);
	}
	public class EstadoCompraRepository : IEstadoCompraRepository
	{
		private DeadlockContext Context { get; }
		private DbSet<EstadoCompra> Estados => Context.EstadoCompra;

		public EstadoCompraRepository(DeadlockContext ctx)
		{
			Context = ctx;
		}

		public async Task<AccessResult<IEnumerable<EstadoCompra>>> GetAll()
		{
			return await Estados.ToListAsync();
		}

		public async Task<AccessResult<EstadoCompra>> Get(Int64 id)
		{
			try
			{
				return await Estados.FindAsync(id);
			}
			catch { return new AccessFault("No se pudo obtener la información de estado de compra"); }
		}

		public async Task<AccessResult<EstadoCompra>> Post(EstadoCompra item)
		{
			try
			{
				IEnumerable<EstadoCompra> Estados = (await GetAll()).Result;
				var lastIndex = Estados.Count() > 0 ? Estados.Max(x => x.Id) : 0;
				item.Id = lastIndex + 1;
				if ((await Get(item.Id)).Result != null) { return new AccessFault("Estado de compra ya existente"); }

				await Context.EstadoCompra.AddAsync(item);
				await Context.SaveChangesAsync();

				return item;
			}
			catch { return new AccessFault("No se pudo publicar la información de estado de compra"); }
		}

		public async Task<AccessResult<EstadoCompra>> Delete(Int64 id)
		{
			try
			{
				EstadoCompra user = await Get(id);
				if (user == null) { return new AccessFault("Estado de compra no existente"); }

				Estados.Remove(user);
				await Context.SaveChangesAsync();
				return user;
			}
			catch { return new AccessFault("No se pudo eliminar la información de estado de compra"); }
		}
	}
}
