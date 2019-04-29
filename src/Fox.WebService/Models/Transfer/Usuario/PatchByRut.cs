using System;
using System.ComponentModel.DataAnnotations;
using Fox.DataAccess.Utils;
using Fox.DataAccess.Validation;

namespace Fox.WebService.Models.Transfer.Usuario
{
	public class PatchByRut
	{
		[RegularExpression(Matches.Nombre, ErrorMessage = "Formato de nombre inválido")]
		public String Nombre { get; set; }

		[RegularExpression(Matches.Password, ErrorMessage = "Contraseña con formato inválido")]
		public String Password { get; set; }

		[RegularExpression(Matches.Email, ErrorMessage = "Email con formato inválido")]
		public String Email { get; set; }

		[Date(ErrorMessage = "Fecha con formato inválido")]
		public String FechaNacimiento { get; set; }
	}
}
