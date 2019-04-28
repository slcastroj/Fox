using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fox.WebService.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
		[HttpGet]
		public IActionResult Get()
		{
			return View("Index");
		}
    }
}