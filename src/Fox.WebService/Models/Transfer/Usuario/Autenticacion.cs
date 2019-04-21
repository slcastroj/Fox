using System;
using System.ComponentModel.DataAnnotations;
using Fox.DataAccess.Utils;

namespace Fox.WebService.Models.Transfer.Usuario
{
	public class Autenticacion
	{
		[Required(ErrorMessage = "Se requiere campo RUT")]
		[Rut(ErrorMessage = "Dígito verificador inválido")]
		[RegularExpression(Matches.Rut, ErrorMessage = "RUT con formato inválido")]
		public String Rut { get; set; }

		[Required(ErrorMessage = "Se requiere campo contraseña")]
		[RegularExpression(Matches.Password, ErrorMessage = "Contraseña con formato inválido")]
		public String Password { get; set; }
	}
}
