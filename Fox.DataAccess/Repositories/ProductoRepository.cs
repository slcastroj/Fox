using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fox.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Fox.DataAccess.Repositories
{
	public interface IProductoRepository
	{
		Task<AccessResult<IEnumerable<Producto>>> GetAll();
		Task<AccessResult<Producto>> Get(Int64 id);
		Task<AccessResult<Producto>> Post(Producto user);
		Task<AccessResult<Producto>> Put(Producto user);
		Task<AccessResult<Producto>> Delete(Int64 id);
	}
	public class ProductoRepository : IProductoRepository
	{
		private DeadlockContext Context { get; }
		private DbSet<Producto> Products => Context.Producto;

		public ProductoRepository(DeadlockContext ctx)
		{
			Context = ctx;
		}

		public async Task<AccessResult<IEnumerable<Producto>>> GetAll()
		{
			return await Products.ToListAsync();
		}

		public async Task<AccessResult<Producto>> Get(Int64 id)
		{
			try
			{
				return await Products.FindAsync(id);
			}
			catch { return new AccessFault("No se pudo obtener la información de usuario"); }
		}

		public async Task<AccessResult<Producto>> Post(Producto item)
		{
			try
			{
				if((await Get(item.Id)).Result != null) { return new AccessFault("Producto ya existente"); }

				AccessResult<Producto> result = await EnsureIntegrity(item);
				if(!result.IsValid) { return result.Fault; }

				await Context.Producto.AddAsync(item);
				await Context.SaveChangesAsync();

				return item;
			}
			catch { return new AccessFault("No se pudo publicar la información de producto"); }
		}

		public async Task<AccessResult<Producto>> Put(Producto item)
		{
			try
			{
				AccessResult<Producto> result = await EnsureIntegrity(item);
				if(!result.IsValid) { return result.Fault; }

				Context.Entry(item).State = EntityState.Modified;
				await Context.SaveChangesAsync();
				return item;
			}
			catch { return new AccessFault("No se pudo actualizar la información de producto"); }
		}

		public async Task<AccessResult<Producto>> Delete(Int64 id)
		{
			try
			{
				Producto user = await Get(id);
				if(user == null) { return new AccessFault("Producto no existente"); }

				Products.Remove(user);
				await Context.SaveChangesAsync();
				return user;
			}
			catch { return new AccessFault("No se pudo eliminar la información de producto"); }
		}

		private async Task<AccessResult<Producto>> EnsureIntegrity(Producto item)
		{
			if(item.NecesitaReceta != "1") { item.NecesitaReceta = "0"; }
			if(await Context.Laboratorio.FindAsync(item.Laboratorio) == null ||
				await Context.TipoProducto.FindAsync(item.Tipo) == null)
			{
				return new AccessFault("Laboratorio o tipo inválidos");
			}

			return item;
		}
	}
}
