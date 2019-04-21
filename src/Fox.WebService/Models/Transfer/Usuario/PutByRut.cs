using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fox.DataAccess.Utils;
using Fox.DataAccess.Validation;

namespace Fox.WebService.Models.Transfer.Usuario
{

	public class PutByRut
	{
		[Required(ErrorMessage = "Se requiere campo contraseña")]
		[RegularExpression(Matches.Password, ErrorMessage = "Contraseña con formato inválido")]
		public String Password { get; set; }

		[Required(ErrorMessage = "Se requiere campo email")]
		[RegularExpression(Matches.Email, ErrorMessage = "Email con formato inválido")]
		public String Email { get; set; }

		[Required(ErrorMessage = "Se requiere campo fecha de nacimiento")]
		[Date(ErrorMessage = "Fecha con formato inválido")]
		public String FechaNacimiento { get; set; }

		[Required(ErrorMessage = "Se requiere campo roles")]
		[InSet("Usuario", "Farmaceutico", "Administrador")]
		public String Roles { get; set; }
	}
}
