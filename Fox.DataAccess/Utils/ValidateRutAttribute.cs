using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fox.DataAccess.Utils
{
	public class ValidateRutAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(Object value, ValidationContext ctx)
		{
			var rut = (String)value;
			if(!CheckVerificationDigit(rut)) { return new ValidationResult(ErrorMessage); }

			return ValidationResult.Success;
		}

		public static Boolean CheckVerificationDigit(String rut)
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
