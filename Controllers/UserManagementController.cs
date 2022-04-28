using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
namespace oneri_sikayet.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly string connString;
        public UserManagementController(IConfiguration config)
        {
            this.connString = config.GetConnectionString("DefaultConnectionString");
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("id") == null)
                return RedirectToAction("Index", "Login");
            List<oneri_sikayet.Models.Personel> list = new List<Models.Personel>();
            SqlConnection connection = new SqlConnection(connString);
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Personel", connection);
            connection.Open();
            adapter.Fill(dt);
            connection.Close();
            foreach (System.Data.DataRow row in dt.Rows)
                list.Add(new Models.Personel {ID= Convert.ToInt32(row["id"]), Name = row["name"].ToString(), Surname = row["surname"].ToString(),Sicil = row["sicil"].ToString() ,Mail = row["mail"].ToString(), Phone = row["phone"].ToString(), Tcno = row["tcno"].ToString(), Permission = Convert.ToInt32(row["permission"]),isPassive = Convert.ToBoolean(row["isPassive"])});
            return View(list);
        }

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("id") == null || HttpContext.Session.GetString("permission") != "1")
                return RedirectToAction("Index", "Home");
            SqlConnection connection = new SqlConnection(connString);
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Personel WHERE id=" + id + "", connection);
            connection.Open();
            adapter.Fill(dt);
            connection.Close();
            if (dt.Rows.Count == 0)
                return RedirectToAction("Index");
            Models.Personel personel = new Models.Personel();
            personel.ID = Convert.ToInt32(dt.Rows[0]["id"]);
            personel.Name = dt.Rows[0]["name"].ToString();
            personel.Surname = dt.Rows[0]["surname"].ToString();
            personel.Mail = dt.Rows[0]["mail"].ToString();
            personel.Tcno = dt.Rows[0]["tcno"].ToString();
            personel.Phone = dt.Rows[0]["phone"].ToString();
            personel.Sicil = dt.Rows[0]["sicil"].ToString();
            personel.Password = dt.Rows[0]["password"].ToString();
            personel.isPassive = Convert.ToBoolean(dt.Rows[0]["isPassive"].ToString());
            personel.Permission = Convert.ToInt32(dt.Rows[0]["permission"].ToString());
            return View("Edit",personel);
        }

        public IActionResult EditUser(Models.Personel personel)
        {
            if(personel.Name == "" || personel.Surname == "" || personel.Password == "" || personel.Mail == "" || personel.Tcno == "" || personel.Sicil == "")
            {
                personel.ErrorMessage = "Tüm alanları doldurun";
                return View("Edit", personel);
            }
            if (personel.Name == null || personel.Surname == null || personel.Mail == null || personel.Password == null || personel.Tcno == null || personel.Sicil == null)
            {
                personel.ErrorMessage = "Tüm alanları doldurun";
                return View("Edit", personel);
            }

            SqlConnection connection = new SqlConnection(connString);
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlCommand command = new SqlCommand("UPDATE Personel SET name='" + personel.Name + "',surname='" + personel.Surname + "',mail='" + personel.Mail + "',phone='" + personel.Phone + "',tcno='"
                 + personel.Tcno + "',sicil='" + personel.Sicil +   "',permission=" + personel.Permission + ",isPassive=" + (personel.isPassive ? 1 : 0) +  " WHERE id=" + personel.ID, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id == null || HttpContext.Session.GetString("id") == null || HttpContext.Session.GetString("permission") != "1")
                return RedirectToAction("Index", "Home");
            SqlConnection connection = new SqlConnection(connString);
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Personel WHERE id=" + id + "", connection);
            connection.Open();
            adapter.Fill(dt);
            connection.Close();
            if (dt.Rows.Count == 0)
                return RedirectToAction("Index");
            Models.Personel personel = new Models.Personel();
            personel.ID = Convert.ToInt32(dt.Rows[0]["id"]);
            personel.Name = dt.Rows[0]["name"].ToString();
            personel.Surname = dt.Rows[0]["surname"].ToString();
            personel.Mail = dt.Rows[0]["mail"].ToString();
            personel.Tcno = dt.Rows[0]["tcno"].ToString();
            personel.Phone = dt.Rows[0]["phone"].ToString();
            personel.Sicil = dt.Rows[0]["sicil"].ToString();
            personel.Password = dt.Rows[0]["password"].ToString();
            personel.isPassive = Convert.ToBoolean(dt.Rows[0]["isPassive"].ToString());
            return View("Delete", personel);
        }

        public IActionResult DeleteUser(Models.Personel personel)
        {
            if (personel.ID == 0)
                return RedirectToAction("Index");
            SqlConnection connection = new SqlConnection(connString);
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlCommand command = new SqlCommand("DELETE Personel WHERE id=" + personel.ID, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("Index");
        }

        public IActionResult New()
        {
            return View();
        }

        public IActionResult NewUser(Models.Personel personel)
        {
            if (personel.Name == "" || personel.Surname == "" || personel.Password == "" || personel.Mail == "" || personel.Tcno == "" || personel.Sicil == "")
            {
                personel.ErrorMessage = "Tüm alanları doldurun";
                return View("Edit", personel);
            }
            if (personel.Name == null || personel.Surname == null || personel.Mail == null || personel.Password == null || personel.Tcno == null || personel.Sicil == null)
            {
                personel.ErrorMessage = "Tüm alanları doldurun";
                return View("Edit", personel);
            }
            SqlConnection connection = new SqlConnection(connString);
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlCommand command = new SqlCommand("INSERT INTO Personel(name,surname,mail,password,phone,tcno,sicil,permission,isPassive) VALUES('" + personel.Name + "','" + personel.Surname + "','" + personel.Mail + "','"+ personel.Password + "','" + personel.Phone + "','"
                 + personel.Tcno + "','" + personel.Sicil + "'," + personel.Permission + "," + (personel.isPassive ? 1 : 0) + ")", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("Index");

        }
    }
}
