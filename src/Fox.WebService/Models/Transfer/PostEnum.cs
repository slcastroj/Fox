using Fox.DataAccess.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fox.WebService.Models.Transfer
{
	public class PostEnum
	{
		[Required(ErrorMessage = "Se requiere campo nombre")]
		[RegularExpression(Matches.Nombre, ErrorMessage = "Nombre no tiene formato válido")]
		public String Nombre { get; set; }
	}
}
