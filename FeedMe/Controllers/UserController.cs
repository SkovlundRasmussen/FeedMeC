using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FeedMe.Models;
using FeedMe.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FeedMe.Controllers
{
    public class UserController : Controller
    {

        Sql_connection con = new Sql_connection();
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

            DataTable dt = con.ReturnDataInDatatable("SELECT email, role_id, password " +
               "                                    FROM Users " +
               "                                    INNER JOIN UsersRole ON UsersRole.role_id = Users.role_id");

            if (dt != null)
            {
                return View();
            }
            else {

                return View(user);
            }
 

          

            
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user, CustomerInfo customerInfo)
        {
            if(ModelState.IsValid)
            {
                con.InsertOrUpdate($"EXEC SP_newUser '{user.firstname}', '{user.lastname}', '{user.email}', '{user.password}','{customerInfo.city}', '{customerInfo.postal_code}', '{customerInfo.street_name}', '{customerInfo.street_number}'");
                /*
                con.InsertOrUpdate($"Insert Into Users (firstname, lastname, email, password, role_id) " +
                                   $"Values ('{user.firstname}', '{user.lastname}', '{user.email}', '{user.password}', 1); " +
                                   $"SELECT SCOPE_IDENTITY(); " +
                                   $" Insert Into CustomerInfo(street_name, street_number, postal_code, city, user_id)" +
                                   $"Values('{customerInfo.street_name}', '{customerInfo.street_number}', '{customerInfo.postal_code}', '{customerInfo.city}', @@IDENTITY)");
                */
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
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