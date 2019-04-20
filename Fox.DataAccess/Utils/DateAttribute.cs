using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fox.DataAccess.Utils
{
	public class DateAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(Object value, ValidationContext ctx)
		{
			var date = (String)value;
			if(!DateTime.TryParse(date, out _)) { return new ValidationResult(ErrorMessage); }

			return ValidationResult.Success;
		}
	}
}
