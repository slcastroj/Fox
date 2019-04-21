using System;
using System.ComponentModel.DataAnnotations;

namespace Fox.WebService.Models.Transfer.OrdenCompra
{
	public class PatchById
	{
		[Required(ErrorMessage = "Se requiere campo estado")]
		[Range(0, Int16.MaxValue, ErrorMessage = "Id estado de compra inválido")]
		public Int32 Estado { get; set; }
	}
}
