using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using oneri_sikayet.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace oneri_sikayet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration config;


        public HomeController(IConfiguration configuration)
        {
            this.config = configuration;
        }



        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("id") == null)
                return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("permission") == "0")
                return RedirectToAction("Index", "User");
            else
                return RedirectToAction("Index", "DiscussionManagement");
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
