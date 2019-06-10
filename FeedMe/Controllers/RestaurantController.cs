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

        const string SessionUserId = "_UserId";
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
            int session_id = Convert.ToInt32(HttpContext.Session.GetInt32(SessionUserId));
            int pk_rest_id;
            string rest_test = rest_id;

            DataTable dt = con.ReturnDataInDatatable($"SELECT * FROM Restaurant WHERE rest_object_id = '{rest_id}'");
            if (dt.Rows.Count == 1)
            {
                pk_rest_id = Convert.ToInt32(dt.Rows[0]["rest_id"]); 
            }
            else
            {
                Console.WriteLine("There are no restaurant id's present");
                return View();
            }

           
            con.InsertOrUpdate($"INSERT INTO Cart(user_id, rest_id, item_name, item_price) VALUES ('{session_id}', '{pk_rest_id}', '{name}', '{price}')");
 
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
            DataTable dt = con.ReturnDataInDatatable($"SELECT email, RestaurantLogin.role_id, RestaurantLogin.rest_id " +
                                                     $"FROM RestaurantLogin " +
                                                     $"INNER JOIN UsersRole ON RestaurantLogin.role_id = UsersRole.role_id " +
                                                     $"WHERE email = '{ rest.email }' AND pw = '{hashed_pw}'");

            if (dt.Rows.Count == 1)
            {
                Console.WriteLine(dt.Rows[0]["role_id"]);

                HttpContext.Session.SetInt32(SessionRoleId, Convert.ToInt32(dt.Rows[0]["role_id"]));
                HttpContext.Session.SetInt32(SessionRestId, Convert.ToInt32(dt.Rows[0]["rest_id"]));

                return RedirectToAction("Orders");
            }
            else
            {
                Console.WriteLine("DET VIRKER IKKE");
                return View();
            }

        }


        public IActionResult Orders(string id)
        {
            int session_rest_id = Convert.ToInt32(HttpContext.Session.GetInt32(SessionRestId));
            string _query = $"SELECT Orders.order_id, item_name, item_price, Orders.date_added, firstname, lastname, street_name, street_number, city, postal_code, status_name " +
                            $"FROM Orders " +
                            $"INNER JOIN Users ON Orders.user_id = Users.user_id INNER JOIN CustomerInfo On Orders.user_id = CustomerInfo.user_id " +
                            $"INNER JOIN OrderStatus ON Orders.status_id = OrderStatus.status_id WHERE rest_id = '{session_rest_id}'";
            switch (id)
            {
                case "pending":
                    _query += " AND OrderStatus.status_id = 1";
                    break;

                case "accepted":
                    _query += " AND OrderStatus.status_id = 2";
                    break;

                case "denied":
                    _query += " AND OrderStatus.status_id = 3";
                    break;

                default:

                    break;
            }

         
            //Get all orders from the restaurant logged in 
            DataTable dt = con.ReturnDataInDatatable(_query);

            var itemList = new List<OrdersUsersStatus>();


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                OrdersUsersStatus ordersUsersStatus = new OrdersUsersStatus();
                User user = new User();
                Item item = new Item();
                CustomerInfo customer = new CustomerInfo();
                OrderStatus status = new OrderStatus();

                ordersUsersStatus.order_id = Convert.ToInt32(dt.Rows[i]["order_id"]);
                item.name = dt.Rows[i]["item_name"].ToString();
                item.price = dt.Rows[i]["item_price"].ToString();
                item.datetime = dt.Rows[i]["date_added"].ToString();

                user.firstname = dt.Rows[i]["firstname"].ToString();
                user.lastname = dt.Rows[i]["lastname"].ToString();

                customer.city = dt.Rows[i]["city"].ToString();
                customer.street_name = dt.Rows[i]["street_name"].ToString();
                customer.street_number = dt.Rows[i]["street_number"].ToString();
                customer.postal_code = Convert.ToInt32(dt.Rows[i]["postal_code"]);

                status.status_name = dt.Rows[i]["status_name"].ToString();

                ordersUsersStatus.user = user;
                ordersUsersStatus.status = status;
                ordersUsersStatus.CustomerInfo = customer;
                ordersUsersStatus.item = item;

                itemList.Add(ordersUsersStatus);
            }
            return View(itemList);

        }
    }
}
