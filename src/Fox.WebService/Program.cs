using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Fox
{
    public class Program
    {
        public static void Main(String[] args)
        {
            BuildWebHost(args).Run();
        }
		public static IWebHost BuildWebHost(String[] args)

		{
			return WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.UseKestrel()
				.UseUrls("http://*:80/")
				.Build();
		}
    }
}
