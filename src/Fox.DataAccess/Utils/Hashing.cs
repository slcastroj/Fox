using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Fox.DataAccess.Utils
{
	public class Hashing
	{
		public static String ComputeSha256(String data)
		{
			using(var sha = SHA256.Create())
			{
				var hashedPwd = sha.ComputeHash(Encoding.UTF8.GetBytes(data));
				return BitConverter.ToString(hashedPwd).Replace("-", "").ToLower();
			}
		}
	}
}
