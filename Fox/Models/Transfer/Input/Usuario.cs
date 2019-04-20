using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fox.DataAccess.Utils;
using Fox.DataAccess.Validation;

namespace Fox.WebService.Models.Transfer.Input.Usuario
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

	public class Get
	{
		[Range(0, UInt16.MaxValue, ErrorMessage = "Cantidad fuera del rango de valores")]
		public Int32? Cantidad { get; set; }

		[InSet("Usuario", "Farmaceutico", "Administrador", ErrorMessage = "Rol no coincide con ninguno válido")]
		public String Roles { get; set; }
	}

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

	public class PatchByRut
	{
		[RegularExpression(Matches.Password, ErrorMessage = "Contraseña con formato inválido")]
		public String Password { get; set; }

		[RegularExpression(Matches.Email, ErrorMessage = "Email con formato inválido")]
		public String Email { get; set; }

		[Date(ErrorMessage = "Fecha con formato inválido")]
		public String FechaNacimiento { get; set; }
	}

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
