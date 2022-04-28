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
    public class DiscussionManagement : Controller
    {
        private readonly string connString;
        public DiscussionManagement(IConfiguration configuration)
        {
            this.connString = configuration.GetConnectionString("DefaultConnectionString");
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("permission") != "1")
                return RedirectToAction("Index", "Home");
            SqlConnection connection = new SqlConnection(connString);
            System.Data.DataTable table = new System.Data.DataTable();
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Discussion INNER JOIN Personel ON Personel.id = Discussion.userId", connection);
            adapter.Fill(table);
            connection.Close();
            List<Models.Discussion> list = new List<Models.Discussion>();
            foreach (System.Data.DataRow row in table.Rows)
            {
                Models.Discussion discussion = new Models.Discussion
                {
                    ID = Convert.ToInt32(row["id"]),
                    Title = row["title"].ToString(),
                    Subject = row["subject"].ToString(),
                    Benefit = row["benefit"].ToString(),
                    Description = row["description"].ToString(),
                };
                if (!Convert.IsDBNull(row["decisionMade"]))
                    discussion.DecisionMade = Convert.ToBoolean(row["decisionMade"].ToString());
                if (!Convert.IsDBNull(row["decision"]))
                    discussion.Decision = Convert.ToBoolean(row["decision"].ToString());
                if (!Convert.IsDBNull(row["decisionDescription"]))
                    discussion.DecisionDescription = row["decisionDescription"].ToString();
                if (!Convert.IsDBNull(row["points"]))
                    discussion.Points = Convert.ToInt32(row["points"].ToString());
                discussion.Owner = new Models.Personel { ID = Convert.ToInt32(row["id"]), Name = row["name"].ToString(), Surname = row["surname"].ToString(), Sicil = row["sicil"].ToString(), Mail = row["mail"].ToString(), Phone = row["phone"].ToString(), Tcno = row["tcno"].ToString(), Permission = Convert.ToInt32(row["permission"]), isPassive = Convert.ToBoolean(row["isPassive"])};
                list.Add(discussion);
            }
            return View(list);
        }

        public IActionResult Decision(int id)
        {
            if (HttpContext.Session.GetString("permission") != "1")
                return RedirectToAction("Index", "Home");
            SqlConnection connection = new SqlConnection(connString);
            System.Data.DataTable table = new System.Data.DataTable();
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Discussion INNER JOIN Personel ON Personel.id = Discussion.userId WHERE Discussion.id=" + id, connection);
            adapter.Fill(table);
            connection.Close();
            System.Data.DataRow row = table.Rows[0];
            Models.Discussion discussion = new Models.Discussion
            {
                ID = Convert.ToInt32(row["id"]),
                Title = row["title"].ToString(),
                Subject = row["subject"].ToString(),
                Benefit = row["benefit"].ToString(),
                Description = row["description"].ToString(),
            };
            if (!Convert.IsDBNull(row["decisionMade"]))
                discussion.DecisionMade = Convert.ToBoolean(row["decisionMade"].ToString());
            if (!Convert.IsDBNull(row["decision"]))
                discussion.Decision = Convert.ToBoolean(row["decision"].ToString());
            if (!Convert.IsDBNull(row["decisionDescription"]))
                discussion.DecisionDescription = row["decisionDescription"].ToString();
            if (!Convert.IsDBNull(row["points"]))
                discussion.Points = Convert.ToInt32(row["points"].ToString());
            discussion.Owner = new Models.Personel { ID = Convert.ToInt32(row["id"]), Name = row["name"].ToString(), Surname = row["surname"].ToString(), Sicil = row["sicil"].ToString(), Mail = row["mail"].ToString(), Phone = row["phone"].ToString(), Tcno = row["tcno"].ToString(), Permission = Convert.ToInt32(row["permission"]), isPassive = Convert.ToBoolean(row["isPassive"]) };
            return View(discussion);
        }

        public IActionResult MakeDecision(Models.Discussion discussion)
        {
            discussion.DecisionMade = true;
            SqlConnection connection = new SqlConnection(connString);
            SqlCommand command = new SqlCommand("UPDATE Discussion SET decision=" + (discussion.Decision ? 1 : 0) + ",decisionDescription='" + discussion.DecisionDescription + "',points=" + discussion.Points + ",decisionMade=1 WHERE id=" + discussion.ID,connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("Index","DiscussionManagement");
        }
    }
}
