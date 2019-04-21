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

		[Required(ErrorMessage = "Se requiere campo contraseña")]
		[RegularExpression(Matches.Password, ErrorMessage = "Contraseña con formato inválido")]
		public String Password { get; set; }

		[Required(ErrorMessage = "Se requiere campo email")]
		[RegularExpression(Matches.Email, ErrorMessage = "Email con formato inválido")]
		public String Email { get; set; }

		[Required(ErrorMessage = "Se requiere campo fecha de nacimiento")]
		[Date(ErrorMessage = "Fecha con formato inválido")]
		public String FechaNacimiento { get; set; }

		[InSet("Usuario", "Farmaceutico", "Administrador")]
		public String Roles { get; set; }
	}
}
