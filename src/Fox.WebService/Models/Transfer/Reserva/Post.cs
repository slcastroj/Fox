using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fox.DataAccess.Utils;

namespace Fox.WebService.Models.Transfer.Reserva
{
	public class Post
	{
		[Required(ErrorMessage = "Se requiere campo usuario")]
		[RegularExpression(Matches.Rut, ErrorMessage = "RUT con formato inválido")]
		[Rut(ErrorMessage = "RUT inválido")]
		public String Usuario { get; set; }

		[Required(ErrorMessage = "Se requiere campo reservas")]
		public List<StockData> Reservas { get; set; }

		public class StockData
		{
			[Required(ErrorMessage = "Se requiere campo cantidad")]
			[Range(0, Int16.MaxValue, ErrorMessage = "Cantidad inválida")]
			public Int32 Cantidad { get; set; }

			[Required(ErrorMessage = "Se requiere campo producto")]
			[Range(0, Int16.MaxValue, ErrorMessage = "Id productoo inválida")]
			public Int32 Producto { get; set; }
		}
	}
}
