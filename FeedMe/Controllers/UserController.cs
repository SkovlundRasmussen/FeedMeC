using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FeedMe.Models;
using FeedMe.ViewModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FeedMe.Controllers
{
    public class UserController : Controller
    {

        const string SessionRoleId = "_RoleId";
        const string SessionUserId = "_UserId";
        Sql_connection con = new Sql_connection();

        static UInt64 CalculateHash(string read)
        {
            UInt64 hashedValue = 3074457345618258791ul;
            for (int i = 0; i < read.Length; i++)
            {
                hashedValue += read[i];
                hashedValue *= 3074457345618258799ul;
            }
            return hashedValue;
        }

        // GET: User
        public ActionResult Index()
        {

            DataTable dt = con.ReturnDataInDatatable("SELECT Users.user_id, firstname, lastname, email, role_name, street_name, street_number, postal_code, city " +
                "                                    FROM Users " +
                "                                    INNER JOIN CustomerInfo ON CustomerInfo.user_id = Users.user_id " +
                "                                    INNER JOIN UsersRole ON UsersRole.role_id = Users.role_id");

            var userList = new List<UserCreateViewModel>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                UserCreateViewModel userCustomer = new UserCreateViewModel();
                CustomerInfo customer = new CustomerInfo();
                User user = new User();
                Role role = new Role();

                user.user_id = Convert.ToInt32(dt.Rows[i]["user_id"]);
                user.firstname = dt.Rows[i]["firstname"].ToString();
                user.lastname = dt.Rows[i]["lastname"].ToString();
                user.email = dt.Rows[i]["email"].ToString();

                role.role_name = dt.Rows[i]["role_name"].ToString();

                customer.street_name = dt.Rows[i]["street_name"].ToString();
                customer.street_number = dt.Rows[i]["street_number"].ToString();
                customer.city = dt.Rows[i]["city"].ToString();
                customer.postal_code = Convert.ToInt32(dt.Rows[i]["postal_code"]);

                userCustomer.user = user;
                userCustomer.customerInfo = customer;
                userCustomer.role = role;

                userList.Add(userCustomer);
            }
            return View(userList);
        }

        public ActionResult Login()
        {
            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            var hashed_pw = CalculateHash(user.password);
            DataTable dt = con.ReturnDataInDatatable($"SELECT email, Users.role_id, Users.user_id, role_name " +
                                                     $"FROM Users " +
                                                     $"INNER JOIN UsersRole ON UsersRole.role_id = Users.role_id " +
                                                     $"WHERE email = '{ user.email }' AND password = '{hashed_pw}'");

            if (dt.Rows.Count == 1)
            {
                Console.WriteLine(dt.Rows[0]["role_id"]);

                HttpContext.Session.SetInt32(SessionRoleId, Convert.ToInt32(dt.Rows[0]["role_id"]));
                HttpContext.Session.SetInt32(SessionUserId, Convert.ToInt32(dt.Rows[0]["user_id"]));
                
                return RedirectToAction("Details");
            }
            else {
                Console.WriteLine("DET VIRKER IKKE");
                return View();
            }
            
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user, CustomerInfo customerInfo)
        {
            var hashed_pw = CalculateHash(user.password);
            if (ModelState.IsValid)
            {
                // USING A STORED PROCEDURE. THIS WILL CHECK IF THE EMAIL OF THE USER EXIST IN THE PROGRAM 
                con.InsertOrUpdate($"EXEC SP_newUser '{user.firstname}', '{user.lastname}', '{user.email}', '{hashed_pw}','{customerInfo.city}', '{customerInfo.postal_code}', '{customerInfo.street_name}', '{customerInfo.street_number}'");

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        public ActionResult CreateStaff()
        {
            return View();
        }


        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            DataTable dt = con.ReturnDataInDatatable($"SELECT firstname, lastname, email, street_name, street_number, postal_code, city " +
                                                     $"FROM Users " +
                                                     $"INNER JOIN CustomerInfo ON CustomerInfo.user_id = Users.user_id " +
                                                     $"WHERE Users.user_id = '{id}'"); 

            if (dt.Rows.Count == 1)
            {
                UserCreateViewModel userCustomer = new UserCreateViewModel();
                CustomerInfo customer = new CustomerInfo();
                User user = new User();

                user.firstname = dt.Rows[0]["firstname"].ToString();
                user.lastname = dt.Rows[0]["lastname"].ToString();
                user.email = dt.Rows[0]["email"].ToString();

                customer.street_name = dt.Rows[0]["street_name"].ToString();
                customer.street_number = dt.Rows[0]["street_number"].ToString();
                customer.city = dt.Rows[0]["city"].ToString();
                customer.postal_code = Convert.ToInt32(dt.Rows[0]["postal_code"]);

                userCustomer.user = user;
                userCustomer.customerInfo = customer;

                return View(userCustomer);
            }
            else {

                return View();
            }

        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, User user, CustomerInfo customerInfo)
        {
            if(ModelState.IsValid)
            {
                con.InsertOrUpdate($"UPDATE Users SET firstname = '{user.firstname}', lastname = '{user.lastname}', email = '{user.email}' WHERE user_id = '{id}'");
                con.InsertOrUpdate($"UPDATE CustomerInfo SET street_name = '{customerInfo.street_name}', street_number = '{customerInfo.street_number}', postal_code = '{customerInfo.postal_code}', city = '{customerInfo.city}' WHERE user_id = '{id}'");

                return RedirectToAction(nameof(Index));
               
               // return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        public IActionResult Details()
        {
            var id = Convert.ToInt32(HttpContext.Session.GetInt32(SessionUserId));
            DataTable dt = con.ReturnDataInDatatable($" SELECT Users.user_id, firstname, lastname, email, role_name, street_name, street_number, postal_code, city " +
                                                     $" FROM Users " +
                                                     $" INNER JOIN CustomerInfo ON CustomerInfo.user_id = Users.user_id " +
                                                     $" INNER JOIN UsersRole ON UsersRole.role_id = Users.role_id WHERE Users.user_id = '{id}'");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                UserCreateViewModel userCustomer = new UserCreateViewModel();
                CustomerInfo customer = new CustomerInfo();
                User user = new User();
                Role role = new Role();

                user.user_id = Convert.ToInt32(dt.Rows[i]["user_id"]);
                user.firstname = dt.Rows[i]["firstname"].ToString();
                user.lastname = dt.Rows[i]["lastname"].ToString();
                user.email = dt.Rows[i]["email"].ToString();

                role.role_name = dt.Rows[i]["role_name"].ToString();

                customer.street_name = dt.Rows[i]["street_name"].ToString();
                customer.street_number = dt.Rows[i]["street_number"].ToString();
                customer.city = dt.Rows[i]["city"].ToString();
                customer.postal_code = Convert.ToInt32(dt.Rows[i]["postal_code"]);

                userCustomer.user = user;
                userCustomer.customerInfo = customer;
                userCustomer.role = role;

                return View(userCustomer);
            }
            return null;

        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                con.InsertOrUpdate($"DELETE FROM Users WHERE user_id = '{id}'");
                con.InsertOrUpdate($"DELETE FROM CustomerInfo WHERE user_id = '{id}'");

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}