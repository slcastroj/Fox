using System;
using System.ComponentModel.DataAnnotations;
using Fox.DataAccess.Utils;

namespace Fox.WebService.Models.Transfer.OrdenCompra
{
	public class Post
	{
		[Required(ErrorMessage = "Se requiere campo usuario")]
		[RegularExpression(Matches.Rut, ErrorMessage = "RUT con formato inválido")]
		[Rut(ErrorMessage = "RUT inválido")]
		public String Usuario { get; set; }

		[Required(ErrorMessage = "Se requiere campo cantidad")]
		[Range(1, Int16.MaxValue, ErrorMessage = "Cantidad fuera de rango")]
		public Int32? Cantidad { get; set; }

		[Required(ErrorMessage = "Se requiere campo producto")]
		[Range(0, Int16.MaxValue, ErrorMessage = "Id producto fuera de rango")]
		public Int32? Producto { get; set; }
	}
}
