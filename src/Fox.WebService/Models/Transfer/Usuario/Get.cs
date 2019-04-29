using System;
using System.ComponentModel.DataAnnotations;
using Fox.DataAccess.Validation;

namespace Fox.WebService.Models.Transfer.Usuario
{
	public class Get
	{
		[Range(0, UInt16.MaxValue, ErrorMessage = "Cantidad fuera del rango de valores")]
		public Int32? Cantidad { get; set; }

		[Range(0, UInt16.MaxValue, ErrorMessage = "Cantidad fuera del rango de valores")]
		public Int32? Rol { get; set; }
	}
}
