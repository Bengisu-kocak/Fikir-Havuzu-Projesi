using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace oneri_sikayet.Models
{
    public class LoginController : Controller
    {
        private readonly IConfiguration configuration;
        
        public LoginController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }

        [HttpPost]
        public IActionResult Authorize(oneri_sikayet.Models.Personel personel)
        {
            if(personel.Mail == null || personel.Password == null)
            {
                personel.ErrorMessage = "Lütfen tüm alanları doldurunuz";
                return View("Index",personel);
            }
            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnectionString"));
            connection.Open();
            System.Data.DataTable table = new System.Data.DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Personel WHERE mail='" + personel.Mail + "' AND password='" + personel.Password + "'",connection);
            adapter.Fill(table);
            connection.Close();
            if(table.Rows.Count != 0)
            {
                HttpContext.Session.SetString("id", table.Rows[0]["id"].ToString());
                HttpContext.Session.SetString("name", table.Rows[0]["name"].ToString());
                HttpContext.Session.SetString("surname", table.Rows[0]["surname"].ToString());
                HttpContext.Session.SetString("mail", table.Rows[0]["mail"].ToString());
                HttpContext.Session.SetString("phone", table.Rows[0]["phone"].ToString());
                HttpContext.Session.SetString("permission", table.Rows[0]["permission"].ToString());
                HttpContext.Session.SetString("passive", table.Rows[0]["isPassive"].ToString());
                return RedirectToAction("Index", "Home");
            }
            else
            {
                personel.ErrorMessage = "Kullanıcı Bulunamadı";
                return View("Index",personel);
            }
        }
    }
}
