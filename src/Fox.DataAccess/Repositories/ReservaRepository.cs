using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fox.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Fox.DataAccess.Repositories
{
	public interface IReservaRepository
	{
		Task<AccessResult<IEnumerable<Reserva>>> GetAll();
		Task<AccessResult<Reserva>> Get(Int64 id);
		Task<AccessResult<Reserva>> Post(Reserva item, IEnumerable<ReservaStock> reservas);
		Task<AccessResult<Reserva>> Delete(Int64 id);
	}
	public class ReservaRepository : IReservaRepository
	{
		private DeadlockContext Context { get; }
		private DbSet<Reserva> Reservas => Context.Reserva;

		public ReservaRepository(DeadlockContext ctx)
		{
			Context = ctx;
		}

		public async Task<AccessResult<IEnumerable<Reserva>>> GetAll()
		{
			return await Reservas.ToListAsync();
		}

		public async Task<AccessResult<Reserva>> Get(Int64 id)
		{
			try
			{
				return await Reservas.FindAsync(id);
			}
			catch { return new AccessFault("No se pudo obtener la información de reserva"); }
		}

		public async Task<AccessResult<Reserva>> Post(Reserva item, IEnumerable<ReservaStock> reservas)
		{
			try
			{
				IEnumerable<Reserva> ordenes = (await GetAll()).Result;
				var lastIndex = ordenes.Count() > 0 ? ordenes.Max(x => x.Id) : 0;
				item.Id = lastIndex + 1;
				if((await Get(item.Id)).Result != null) { return new AccessFault("Reserva ya existente"); }

				AccessResult<(Reserva,Compra)> result = await EnsureIntegrity(item, reservas);
				if(!result.IsValid) { return result.Fault; }

				await Context.Reserva.AddAsync(item);
				await Context.ReservaStock.AddRangeAsync(reservas);
				var compra = result.Result.Item2;
				if (compra != null) { await Context.Compra.AddAsync(result.Result.Item2); }
				await Context.SaveChangesAsync();

				return item;
			}
			catch { return new AccessFault("No se pudo publicar la información de reserva"); }
		}


		public async Task<AccessResult<Reserva>> Delete(Int64 id)
		{
			try
			{
				Reserva user = await Get(id);
				if(user == null) { return new AccessFault("Orden no existente"); }

				Reservas.Remove(user);
				await Context.SaveChangesAsync();
				return user;
			}
			catch { return new AccessFault("No se pudo eliminar la información de reserva"); }
		}

		private async Task<AccessResult<(Reserva, Compra)>> EnsureIntegrity(Reserva item, IEnumerable<ReservaStock> reservas)
		{
			Usuario usuario = await Context.Usuario.FindAsync(item.Usuario);
			IEnumerable < ReservaStock > aR = await Context.ReservaStock.ToListAsync();
			var lastIndex = aR.Count() > 0 ? aR.Max(x => x.Id) : 0;
			var necesitaReceta = false;

			foreach (ReservaStock r in reservas)
				{
					r.Id = ++lastIndex;
				r.Reserva = item.Id;

				if (Context.Producto.Find(r.Producto) == null) { return new AccessFault("Claves foráneas inválidas"); }

				if ((await Context.Producto.FindAsync(r.Producto)).Stock - r.Cantidad < 0)
				{
					return new AccessFault("Stock no es suficiente");
				}

				if((await Context.Producto.FindAsync(r.Producto)).NecesitaReceta == "1") { necesitaReceta = true; }
			}

			if(usuario == null)
			{
				return new AccessFault("Claves foráneas inválidas");
			}

			Compra compra = null;
			if(!necesitaReceta)
			{
				IEnumerable<Compra> aC = await Context.Compra.ToListAsync();
				lastIndex = aC.Count() > 0 ? aC.Max(x => x.Id) : 0;
				compra = new Compra
				{
					Id = lastIndex + 1,
					Estado = 1,
					Reserva = item.Id
				};
			}

			return (item, compra);
		}
	}
}
