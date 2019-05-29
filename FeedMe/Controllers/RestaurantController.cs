using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FeedMe.Controllers;
using FeedMe.Models;
using FeedMe.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FeedMe.Controllers
{
    public class RestaurantController : Controller
    {
        private Item menuItem;
        Sql_connection con = new Sql_connection();
        private IMongoCollection<Restaurant> collection;
        
        const string SessionRoleId = "_RoleId";
        const string SessionRestId = "_RestId";

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


        public RestaurantController()
        {
            var client = new MongoClient("mongodb+srv://admin:admin123@feedme-exv6o.mongodb.net/FeedMe?retryWrites=true");
            IMongoDatabase db = client.GetDatabase("FeedMe");
            this.collection = db.GetCollection<Restaurant>("Restaurant");
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var model = collection.Find(FilterDefinition<Restaurant>.Empty).ToList();
            return View(model);
        }
        
        // GET: /<controller>/
        public IActionResult Detail(string id)
        {
            var model = collection.Find(document => document.Id == id).ToList();
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Add(string id, string name, string price, string rest_id)
        {
            var model = collection.Find(FilterDefinition<Restaurant>.Empty).ToList();

            Item item = new Item();
            item.id = id;
            item.name = name;
            item.price = price;
            item.restId = rest_id;
            
            
            Console.WriteLine("Item ID : " + id);
            Console.WriteLine("Name: " + name);
            Console.WriteLine("Price: " + price);
            Console.WriteLine("Restaurant ID: " + rest_id);

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(RestaurantLogin rest)
        {
            var hashed_pw = CalculateHash(rest.pw);
            DataTable dt = con.ReturnDataInDatatable($"SELECT email, Restaurant_login.role_id, Restaurant_login.rest_id " +
                                                     $"FROM Restaurant_login " +
                                                     $"INNER JOIN UsersRole ON Restaurant_login.role_id = UsersRole.role_id " +
                                                     $"WHERE email = '{ rest.email }' AND pw = '{hashed_pw}'");

            if (dt.Rows.Count == 1)
            {
                Console.WriteLine(dt.Rows[0]["role_id"]);

                HttpContext.Session.SetInt32(SessionRoleId, Convert.ToInt32(dt.Rows[0]["role_id"]));
                HttpContext.Session.SetInt32(SessionRestId, Convert.ToInt32(dt.Rows[0]["rest_id"]));
                return View("Index");
            }
            else
            {
                Console.WriteLine("DET VIRKER IKKE");
                return View();
            }

        }
    }
}
