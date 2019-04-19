using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Fox.DataAccess.Models;

namespace Fox.DataAccess.Utils
{
	public static class Validation
	{
		public static Boolean CheckUsuario(Usuario user)
		{
			if(user.Rut == null ||
				user.Email == null ||
				user.FechaNacimiento == null ||
				user.Password == null) { return false; }

			user.Rut = user.Rut.ToLower();
			user.Email = user.Email.ToLower();
			user.Roles = user.Roles ?? "Usuario";

			const String rutMatch = @"^[0-9]{7,8}[0-9k]$";
			const String emailMatch = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,4})+)$";
			const String passwordMatch = @"^[-0-9a-zA-Z/.]{8,}$";

			if(!DateTime.TryParse(user.FechaNacimiento, out DateTime fecNac)) { return false; }
			user.FechaNacimiento = fecNac.ToString("yyyy-MM-dd");

			if(Regex.IsMatch(user.Rut, rutMatch) &&
				checkRut(user.Rut) &&
				Regex.IsMatch(user.Email, emailMatch) &&
				Regex.IsMatch(user.Password, passwordMatch) &&
				(user.Roles == "Usuario" || user.Roles == "Farmaceutico" || user.Roles == "Administrador"))
			{
				user.Password = Hashing.ComputeSha256(user.Password);
				return true;
			}

			return false;

			Boolean checkRut(String rut)
			{
				var dv = rut.Substring(rut.Length - 1);
				rut = rut.Substring(0, rut.Length - 1);
				var sum = 0;

				for(Int32 i = rut.Length, mult = 2; i > 0; i--, mult++)
				{
					if(mult == 8) { mult = 2; }

					var d = Int32.Parse(rut.Substring(rut.Length - 1));
					rut = rut.Substring(0, rut.Length - 1);

					sum += d * mult;
				}

				sum = 11 - (sum % 11);

				if(dv == "k" && sum != 10) { return true; }
				if(sum == 10 && Int32.Parse(dv) == 0) { return true; }
				if(sum == Int32.Parse(dv)) { return true; }

				return false;
			}
		}
	}
}
