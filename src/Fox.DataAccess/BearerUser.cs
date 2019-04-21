using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fox.DataAccess
{
	public class BearerUser
	{
		public String Rut { get; set; }
		public String Token { get; set; }
		public DateTime Creacion { get; set; }
		public UInt32 Expiracion { get; set; }
	}
}
