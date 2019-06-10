using FeedMe.Models;
using FeedMe.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;


namespace FeedMe.Controllers
{
    public class Orders : Controller    
    {
        Sql_connection con = new Sql_connection();

        const string SessionUserId = "_UserId";
        const string SessionRoleId = "_RoleId";
        const string SessionRestId = "_RestId";

        public IActionResult Index()
        {
            
            int session_rest_id = Convert.ToInt32(HttpContext.Session.GetInt32(SessionRestId));
            //Get all orders from the restaurant logged in 
            DataTable dt = con.ReturnDataInDatatable($"SELECT Orders.order_id, item_name, item_price, Orders.date_added, firstname, lastname, street_name, street_number, city, postal_code, status_name " +
                                                     $"FROM Orders " +
                                                     $"INNER JOIN Users ON Orders.user_id = Users.user_id INNER JOIN CustomerInfo On Orders.user_id = CustomerInfo.user_id " +
                                                     $"INNER JOIN OrderStatus ON Orders.status_id = OrderStatus.status_id WHERE rest_id = '{session_rest_id}'");

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
