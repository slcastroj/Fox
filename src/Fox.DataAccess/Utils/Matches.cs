using System;
using System.Collections.Generic;
using System.Text;

namespace Fox.DataAccess.Utils
{
	public static class Matches
	{
		public const String Rut = @"^[0-9]{7,8}[0-9k]$";
		public const String Email = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,4})+)$";
		public const String Password = @"^[-\w./]{8,}$";
		public const String Nombre = @"^[-\p{L}./\s()\[\]ªº]{4,}$";
	}
}
