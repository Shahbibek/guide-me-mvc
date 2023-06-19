using Microsoft.AspNetCore.Mvc;
using NoNotAgain.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NoNotAgain.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection conn = new SqlConnection("Data Source=LAPTOP\\SQLEXPRESS;Initial Catalog=Today;Integrated Security=True");

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<User> userlist = new List<User>();

            try {
                string query = "Select * from hoo";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    User data = new User();
                    data.Id = Convert.ToInt32(reader["Id"]);
                    data.fname = reader["fname"].ToString();
                    data.lname = reader["lname"].ToString();
                    data.email = reader["email"].ToString();
                    data.salary = reader["salary"].ToString();
                    userlist.Add(data);
                }

            }catch(Exception ex)
            {
                ex.Message.ToString();
            }
            conn.Close();
            return View(userlist);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            try
            {
                string query = "Insert into hoo(fname, lname, email, salary, password) values (@fname, @lname, @email, @salary, @password)";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@fname", user.fname.ToString());
                cmd.Parameters.AddWithValue("@lname", user.lname.ToString());
                cmd.Parameters.AddWithValue("@email", user.email.ToString());
                cmd.Parameters.AddWithValue("@salary", user.salary.ToString());
                cmd.Parameters.AddWithValue("@password", user.password.ToString());
                int v = await cmd.ExecuteNonQueryAsync();
                if (v > 0)
                {
                    ViewBag.notify = "Successfully Created";
                }
                else
                {
                    ViewBag.notify = "Something Went Wrong";
                }

            }
            catch(Exception ex)
            {
                ex.Message.ToString();
            }
            conn.Close();
            return View();
        }

        public async Task<ActionResult> Delete(int Id)
        {
            try {
                string query = "Delete from hoo where Id=@Id";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", Id.ToString());
                int v = await cmd.ExecuteNonQueryAsync();
                if (v > 0)
                {
                    ViewBag.msg = "Successfully Deleted";
                }
                else
                {
                    ViewBag.msg = "Somethig Went Wrong";
                }

            }catch(Exception ex)
            {
                ex.Message.ToString();
            }
            conn.Close();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int Id)
        {
            DataTable dt = new DataTable();

            string query = "Select * from hoo where Id=@Id";
            conn.Open();
            SqlDataAdapter adapt = new SqlDataAdapter(query, conn);
            adapt.SelectCommand.Parameters.AddWithValue("@Id", Id.ToString());
            adapt.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                User data = new User();
                data.fname = dt.Rows[0][1].ToString();
                data.lname = dt.Rows[0][2].ToString();
                data.email = dt.Rows[0][3].ToString();
                data.salary = dt.Rows[0][4].ToString();
                return View(data);
                conn.Close();
            }
            else
            {
                return RedirectToAction("Index");
                
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(User user, int Id)
        {
            string query = "Update hoo set fname=@fname, lname=@lname, email=@email, salary=@salary where Id=@Id";
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", Id.ToString());
            cmd.Parameters.AddWithValue("@fname", user.fname.ToString());
            cmd.Parameters.AddWithValue("@lname", user.lname.ToString());
            cmd.Parameters.AddWithValue("@email", user.email.ToString());
            cmd.Parameters.AddWithValue("@salary", user.salary.ToString());
            int v = await cmd.ExecuteNonQueryAsync();
            if(v > 0)
            {
                ViewBag.help = "Updated Successfully";
            }
            else
            {
                ViewBag.help = "Something Went Wrong";
            }
            conn.Close();
            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}