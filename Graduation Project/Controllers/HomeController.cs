using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using GraduationProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<User> users; 
        public HomeController(ILogger<HomeController> logger, IRepository<User> users)
        {
            _logger = logger;
            this.users = users; 
        }

        public IActionResult Index()
        {
            //var list = users.List(); 
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
