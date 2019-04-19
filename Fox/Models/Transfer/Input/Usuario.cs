using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fox.Models.Transfer.Input.Usuario
{
	public class Autenticacion
	{
		public String Rut { get; set; }
		public String Password { get; set; }
	}

	public class Get
	{
		public Int32? Cantidad { get; set; }
		public String Roles { get; set; }
	}

	public class Post
	{
		public String Rut { get; set; }
		public String Password { get; set; }
		public String Email { get; set; }
		public String FechaNacimiento { get; set; }
		public String Roles { get; set; }
	}

	public class PatchByRut
	{
		public String Password { get; set; }
		public String Email { get; set; }
		public String FechaNacimiento { get; set; }
	}

	public class PutByRut
	{
		public String Password { get; set; }
		public String Email { get; set; }
		public String FechaNacimiento { get; set; }
		public String Roles { get; set; }
	}
}
