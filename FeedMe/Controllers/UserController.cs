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
                con.InsertOrUpdate($"Insert Into Users (firstname, lastname, email, password, role_id) " +
                                   $"Values ('{user.firstname}', '{user.lastname}', '{user.email}', '{user.password}', 1); " +
                                   $"SELECT SCOPE_IDENTITY(); " +
                                   $" Insert Into CustomerInfo(street_name, street_number, postal_code, city, user_id)" +
                                   $"Values('{customerInfo.street_name}', '{customerInfo.street_number}', '{customerInfo.postal_code}', '{customerInfo.city}', @@IDENTITY)");
                 
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
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
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
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}