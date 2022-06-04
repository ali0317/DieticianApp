using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.Controllers
{
    public class BMIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
