using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fox.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Fox.DataAccess.Repositories
{
	public interface ICompraRepository
	{
		Task<AccessResult<IEnumerable<Compra>>> GetAll();
		Task<AccessResult<Compra>> Get(Int64 id);
		Task<AccessResult<Compra>> Patch(Int64 id, Int64? estado);
		Task<AccessResult<Compra>> Delete(Int64 id);
	}
	public class CompraRepository : ICompraRepository
	{
		private DeadlockContext Context { get; }
		private DbSet<Compra> Compras => Context.Compra;

		public CompraRepository(DeadlockContext ctx)
		{
			Context = ctx;
		}

		public async Task<AccessResult<IEnumerable<Compra>>> GetAll()
		{
			return await Compras.ToListAsync();
		}

		public async Task<AccessResult<Compra>> Get(Int64 id)
		{
			try
			{
				return await Compras.FindAsync(id);
			}
			catch { return new AccessFault("No se pudo obtener la información de compra"); }
		}

		public async Task<AccessResult<Compra>> Patch(Int64 id, Int64? estado)
		{
			try
			{
				Compra item = await Get(id);
				if (item == null) { return new AccessFault("Compra no existente"); }

				if(estado != null) { item.Estado = estado.Value; }

				AccessResult<Compra> result = await EnsureIntegrity(item);
				if(!result.IsValid) { return result.Fault; }

				await Context.SaveChangesAsync();

				return item;
			}
			catch { return new AccessFault("No se pudo actualizar la información de usuario"); }
		}

		public async Task<AccessResult<Compra>> Delete(Int64 id)
		{
			try
			{
				Compra user = await Get(id);
				if (user == null) { return new AccessFault("Orden no existente"); }

				Compras.Remove(user);
				await Context.SaveChangesAsync();
				return user;
			}
			catch { return new AccessFault("No se pudo eliminar la información de compra"); }
		}

		private async Task<AccessResult<Compra>> EnsureIntegrity(Compra item)
		{
			item.Estado = (await Context.EstadoCompra.FindAsync(item.Id))?.Id ?? 1;
			if ((await Context.Reserva.FindAsync(item.Reserva)) == null) { return new AccessFault("Claves foráneas inválidas"); }

			return item;
		}
	}
}
