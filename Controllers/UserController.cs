using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using oneri_sikayet.Models;
using System.Data.SqlClient;
namespace oneri_sikayet.Controllers
{
    public class UserController : Controller
    {
        private readonly string connString;
        public UserController(IConfiguration config)
        {
            this.connString = config.GetConnectionString("DefaultConnectionString");
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("id") == null)
                return RedirectToAction("Index", "Login");
            return View();
        }

        [HttpPost]
        public IActionResult Send(Discussion obj)
        {
            if(obj.Title == null || obj.Subject == null || obj.Benefit == null || obj.Description == null)
            {
                obj.Message = "Lütfen Tüm Alanları Doldurun.";
                return View("Index", obj);
            }
            if(HttpContext.Session.GetString("passive") == "True")
            {
                obj.Message = "Hesabınız Pasifleştirilmiş.";
                return View("Index", obj);
            }
            SqlConnection sqlConnection = new SqlConnection(connString);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("INSERT INTO Discussion(title,subject,benefit,description,userId,decision,decisionDescription,points,decisionMade) VALUES('" + obj.Title + "','" + obj.Subject + "','" + obj.Benefit + "','" + obj.Description + "'," + HttpContext.Session.GetString("id") + ",NULL,NULL,NULL,0)", sqlConnection);
            int c = command.ExecuteNonQuery();
            if(c == 0)
            {
                obj.Message = "Ekleme Başarısız Oldu.";
                return View("Index", obj);
            }
            else
            {
                obj.Message = "Başarıyla Eklendi.";
                return View("Index", obj);
            }
        }
    }
}
