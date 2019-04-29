using System;
using System.ComponentModel.DataAnnotations;
using Fox.DataAccess.Utils;
using Fox.DataAccess.Validation;

namespace Fox.WebService.Models.Transfer.Usuario
{
	public class Post
	{
		[Required(ErrorMessage = "Se requiere campo RUT")]
		[Rut(ErrorMessage = "Dígito verificador inválido")]
		[RegularExpression(Matches.Rut, ErrorMessage = "RUT con formato inválido")]
		public String Rut { get; set; }

		[Required(ErrorMessage = "Se requiere campo nombre")]
		[RegularExpression(Matches.Nombre, ErrorMessage = "Formato de nombre inválido")]
		public String Nombre { get; set; }

		[Required(ErrorMessage = "Se requiere campo contraseña")]
		[RegularExpression(Matches.Password, ErrorMessage = "Contraseña con formato inválido")]
		public String Password { get; set; }

		[Required(ErrorMessage = "Se requiere campo email")]
		[RegularExpression(Matches.Email, ErrorMessage = "Email con formato inválido")]
		public String Email { get; set; }

		[Required(ErrorMessage = "Se requiere campo fecha de nacimiento")]
		[Date(ErrorMessage = "Fecha con formato inválido")]
		public String FechaNacimiento { get; set; }

		[Required(ErrorMessage = "Se requiere campo rol")]
		[Range(0, UInt16.MaxValue, ErrorMessage = "Cantidad fuera del rango de valores")]
		public Int32? Rol { get; set; }
	}
}
