using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fox.DataAccess.Utils;

namespace Fox.WebService.Models.Transfer.Input.OrdenCompra
{
	public class Get
	{
		[RegularExpression(Matches.Rut, ErrorMessage = "RUT con formato inválido")]
		[Rut(ErrorMessage = "RUT inválido")]
		public String Usuario { get; set; }

		[RegularExpression(Matches.Rut, ErrorMessage = "RUT con formato inválido")]
		[Rut(ErrorMessage = "RUT inválido")]
		public String Asignado { get; set; }

		[Range(0, Int16.MaxValue, ErrorMessage = "Id producto fuera de rango")]
		public Int32? Producto { get; set; }

		[Range(0, Int16.MaxValue, ErrorMessage = "Id estado de compra inválido")]
		public Int32? Estado { get; set; }
	}

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

	public class PatchById
	{
		[Required(ErrorMessage = "Se requiere campo estado")]
		[Range(0, Int16.MaxValue, ErrorMessage = "Id estado de compra inválido")]
		public Int32 Estado { get; set; }
	}

	public class PutById
	{
		[Required(ErrorMessage = "Se requiere campo id")]
		[Range(0, Int16.MaxValue, ErrorMessage = "Id de compra inválido")]
		public Int32 Id { get; set; }

		[Required(ErrorMessage = "Se requiere campo usuario")]
		[RegularExpression(Matches.Rut, ErrorMessage = "RUT con formato inválido")]
		[Rut(ErrorMessage = "RUT inválido")]
		public String Usuario { get; set; }

		[Required(ErrorMessage = "Se requiere campo asignado")]
		[RegularExpression(Matches.Rut, ErrorMessage = "RUT con formato inválido")]
		[Rut(ErrorMessage = "RUT inválido")]
		public String Asignado { get; set; }

		[Required(ErrorMessage = "Se requiere campo cantidad")]
		[Range(1, Int16.MaxValue, ErrorMessage = "Cantidad fuera de rango")]
		public Int32 Cantidad { get; set; }

		[Required(ErrorMessage = "Se requiere campo producto")]
		[Range(0, Int16.MaxValue, ErrorMessage = "Id producto fuera de rango")]
		public Int32 Producto { get; set; }

		[Required(ErrorMessage = "Se requiere campo estado")]
		[Range(0, Int16.MaxValue, ErrorMessage = "Id estado de compra inválido")]
		public Int32 Estado { get; set; }
	}
}
