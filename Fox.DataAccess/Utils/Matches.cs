using System;
using System.Collections.Generic;
using System.Text;

namespace Fox.DataAccess.Utils
{
	public static class Matches
	{
		public const String Rut = @"^[0-9]{7,8}[0-9k]$";
		public const String Email = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,4})+)$";
		public const String Password = @"^[-0-9a-zA-Z/.]{8,}$";
	}
}
