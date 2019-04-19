using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fox.DataAccess.Repositories
{
	public class AccessFault
	{
		public String Message { get; set; }

		public AccessFault(String msg)
		{
			Message = msg;
		}

		public static implicit operator AccessFault(String msg) => new AccessFault(msg);
	}
}
