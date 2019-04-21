using System;
using System.ComponentModel.DataAnnotations;
using Fox.DataAccess.Utils;

namespace Fox.WebService.Models.Transfer.Producto
{
	public class Get
	{
		[Range(0, Int16.MaxValue, ErrorMessage = "Cantidad fuera de rango")]
		public Int32? Cantidad { get; set; }

		[Range(0, Int16.MaxValue, ErrorMessage = "Indice fuera de rango")]
		public Int32? Indice { get; set; }

		[Range(0, Int16.MaxValue, ErrorMessage = "Id tipo fuera de rango")]
		public Int32? Tipo { get; set; }

		[RegularExpression(Matches.Nombre, ErrorMessage = "Nombre con formato inválido o menor a 4 carácteres")]
		public String Nombre { get; set; }

		public Boolean? TieneStock { get; set; }

		[Range(0, Int16.MaxValue, ErrorMessage = "Id laboratorio fuera de rango")]
		public Int32? Laboratorio { get; set; }
	}
}
