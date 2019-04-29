using System;
using System.Collections.Generic;
using System.Text;

namespace Fox.DataAccess.Repositories
{
	public class AccessResult<T>
	{
		public T Result { get; }
		public AccessFault Fault { get; }
		public Boolean IsValid => Fault == null && Result != null;

		public AccessResult(T result) { Result = result; }
		public AccessResult(AccessFault fault) { Fault = fault; }

		public static implicit operator T(AccessResult<T> self) => self.Result;
		public static implicit operator AccessFault(AccessResult<T> self) => self.Fault;
		public static implicit operator AccessResult<T>(T self) => From(self);
		public static implicit operator AccessResult<T>(AccessFault self) => From(self);

		public static AccessResult<T> From(T result) => new AccessResult<T>(result);
		public static AccessResult<T> From(AccessFault fault) => new AccessResult<T>(fault);
	}
}
