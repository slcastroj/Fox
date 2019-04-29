using System;
using System.ComponentModel.DataAnnotations;
using Fox.DataAccess.Utils;

namespace Fox.WebService.Models.Transfer.Reserva
{
	public class Get
	{
		[RegularExpression(Matches.Rut, ErrorMessage = "RUT con formato inválido")]
		[Rut(ErrorMessage = "RUT inválido")]
		public String Usuario { get; set; }

		[Range(0, Int16.MaxValue, ErrorMessage = "Id producto fuera de rango")]
		public Int32? Producto { get; set; }

		[Range(0, Int16.MaxValue, ErrorMessage = "Id estado de compra inválido")]
		public Int32? Estado { get; set; }
	}
}
