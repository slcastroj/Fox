using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fox.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Fox.DataAccess.Repositories
{
	public interface ITipoProductoRepository
	{
		Task<AccessResult<IEnumerable<TipoProducto>>> GetAll();
		Task<AccessResult<TipoProducto>> Get(Int64 id);
		Task<AccessResult<TipoProducto>> Post(TipoProducto user);
		Task<AccessResult<TipoProducto>> Delete(Int64 id);
	}
	public class TipoProductoRepository : ITipoProductoRepository
	{
		private DeadlockContext Context { get; }
		private DbSet<TipoProducto> TipoProductos => Context.TipoProducto;

		public TipoProductoRepository(DeadlockContext ctx)
		{
			Context = ctx;
		}

		public async Task<AccessResult<IEnumerable<TipoProducto>>> GetAll()
		{
			return await TipoProductos.ToListAsync();
		}

		public async Task<AccessResult<TipoProducto>> Get(Int64 id)
		{
			try
			{
				return await TipoProductos.FindAsync(id);
			}
			catch { return new AccessFault("No se pudo obtener la información de tipo de producto"); }
		}

		public async Task<AccessResult<TipoProducto>> Post(TipoProducto item)
		{
			try
			{
				IEnumerable<TipoProducto> Estados = (await GetAll()).Result;
				var lastIndex = Estados.Count() > 0 ? Estados.Max(x => x.Id) : 0;
				item.Id = lastIndex + 1;
				if ((await Get(item.Id)).Result != null) { return new AccessFault("Tipo de producto ya existente"); }

				await Context.TipoProducto.AddAsync(item);
				await Context.SaveChangesAsync();

				return item;
			}
			catch { return new AccessFault("No se pudo publicar la información de tipo de producto"); }
		}

		public async Task<AccessResult<TipoProducto>> Delete(Int64 id)
		{
			try
			{
				TipoProducto user = await Get(id);
				if (user == null) { return new AccessFault("Tipo de producto no existente"); }

				TipoProductos.Remove(user);
				await Context.SaveChangesAsync();
				return user;
			}
			catch { return new AccessFault("No se pudo eliminar la información de tipo de producto"); }
		}
	}
}
