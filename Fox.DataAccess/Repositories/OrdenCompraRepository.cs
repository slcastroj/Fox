using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fox.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Fox.DataAccess.Repositories
{
	public interface IOrdenCompraRepository
	{
		Task<AccessResult<IEnumerable<OrdenCompra>>> GetAll();
		Task<AccessResult<OrdenCompra>> Get(Int64 id);
		Task<AccessResult<OrdenCompra>> Post(OrdenCompra user);
		Task<AccessResult<OrdenCompra>> Patch(Int64 id, Int32 estado);
		Task<AccessResult<OrdenCompra>> Put(OrdenCompra user);
		Task<AccessResult<OrdenCompra>> Delete(Int64 id);
	}
	public class OrdenCompraRepository : IOrdenCompraRepository
	{
		private DeadlockContext Context { get; }
		private DbSet<OrdenCompra> Ordenes => Context.OrdenCompra;

		public OrdenCompraRepository(DeadlockContext ctx)
		{
			Context = ctx;
		}

		public async Task<AccessResult<IEnumerable<OrdenCompra>>> GetAll()
		{
			return await Ordenes.ToListAsync();
		}

		public async Task<AccessResult<OrdenCompra>> Get(Int64 id)
		{
			try
			{
				return await Ordenes.FindAsync(id);
			}
			catch { return new AccessFault("No se pudo obtener la información de orden"); }
		}

		public async Task<AccessResult<OrdenCompra>> Post(OrdenCompra item)
		{
			try
			{
				IEnumerable<OrdenCompra> ordenes = (await GetAll()).Result;
				var lastIndex = ordenes.Count() > 0 ? ordenes.Max(x => x.Id) : 0;
				item.Id = lastIndex + 1;
				if((await Get(item.Id)).Result != null) { return new AccessFault("Orden ya existente"); }

				AccessResult<OrdenCompra> result = await EnsureIntegrity(item);
				if(!result.IsValid) { return result.Fault; }

				await Context.OrdenCompra.AddAsync(item);
				await Context.SaveChangesAsync();

				return item;
			}
			catch { return new AccessFault("No se pudo publicar la información de orden"); }
		}

		public async Task<AccessResult<OrdenCompra>> Patch(Int64 id, Int32 estado)
		{
			try
			{
				OrdenCompra orden = await Get(id);
				if(orden == null) { return new AccessFault("Orden no existente"); }

				orden.Estado = estado;

				AccessResult<OrdenCompra> result = await EnsureIntegrity(orden);
				if(!result.IsValid) { return result.Fault; }

				await Context.SaveChangesAsync();

				return orden;
			}
			catch { return new AccessFault("No se pudo actualizar la información de usuario"); }
		}

		public async Task<AccessResult<OrdenCompra>> Put(OrdenCompra item)
		{
			try
			{
				AccessResult<OrdenCompra> result = await EnsureIntegrity(item);
				if(!result.IsValid) { return result.Fault; }

				OrdenCompra apt = await Ordenes.FindAsync(item.Id);
				apt.Producto = item.Producto;
				apt.Usuario = item.Usuario;
				apt.Asignado = item.Asignado;
				apt.Cantidad = item.Cantidad;
				apt.Estado = item.Estado;

				await Context.SaveChangesAsync();
				return item;
			}
			catch { return new AccessFault("No se pudo actualizar la información de orden"); }
		}

		public async Task<AccessResult<OrdenCompra>> Delete(Int64 id)
		{
			try
			{
				OrdenCompra user = await Get(id);
				if(user == null) { return new AccessFault("Orden no existente"); }

				Ordenes.Remove(user);
				await Context.SaveChangesAsync();
				return user;
			}
			catch { return new AccessFault("No se pudo eliminar la información de orden"); }
		}

		private async Task<AccessResult<OrdenCompra>> EnsureIntegrity(OrdenCompra item)
		{
			item.Cantidad = item.Cantidad <= 0 ? 1 : item.Cantidad;
			item.Estado = item.Estado <= 0 ? 1 : item.Estado;

			Usuario asignado = await Context.Usuario.FindAsync(item.Asignado);
			Usuario usuario = await Context.Usuario.FindAsync(item.Usuario);
			Producto producto = await Context.Producto.FindAsync(item.Producto);
			EstadoCompra estado = await Context.EstadoCompra.FindAsync(item.Estado);

			if(usuario == null ||
				 producto == null ||
				 estado == null ||
				(item.Asignado != null && (asignado == null || !(asignado.Roles == "Administrador" || asignado.Roles == "Farmaceutico"))))
			{
				return new AccessFault("Claves foráneas inválidas");
			}

			if((await Context.Producto.FindAsync(item.Producto)).Stock - item.Cantidad < 0)
			{
				return new AccessFault("Stock no es suficiente");
			}

			return item;
		}
	}
}
