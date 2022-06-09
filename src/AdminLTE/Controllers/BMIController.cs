using AdminLTE.Data;
using AdminLTE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdminLTE.Controllers
{
    public class BMIController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public BMIController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(BMI model,IFormCollection collection)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var userId = claim.Value;
            model.UserId = userId;
            model.CreatedDate = DateTime.Now;
            model.Gender = Convert.ToString(collection["gender"]);
            _dbContext.BMI.Add(model);
            _dbContext.SaveChanges();

            return View(new BMI()) ;
        }
    }
}
