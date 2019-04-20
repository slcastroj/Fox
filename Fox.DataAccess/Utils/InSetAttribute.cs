using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fox.DataAccess.Utils
{
	public class InSetAttribute : ValidationAttribute
	{
		public String[] ValidValues { get; }

		public InSetAttribute(params String[] validValues)
		{
			ValidValues = validValues;
		}

		protected override ValidationResult IsValid(Object value, ValidationContext ctx)
		{
			var rol = (String)value;
			if(String.IsNullOrEmpty(rol)) { return ValidationResult.Success; }
			foreach(var val in ValidValues)
			{
				if(rol == val) { return ValidationResult.Success; }
			}

			return new ValidationResult(ErrorMessage);
		}
	}
}
