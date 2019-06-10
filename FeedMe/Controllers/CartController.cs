using FeedMe.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;

namespace FeedMe.Controllers
{
    public class CartController : Controller
    {

        const string SessionRoleId = "_RoleId";
        const string SessionUserId = "_UserId";
        Sql_connection con = new Sql_connection();

        public ActionResult Index()
        {
            int session_id = Convert.ToInt32(HttpContext.Session.GetInt32(SessionUserId));
          
            DataTable dt = con.ReturnDataInDatatable($"SELECT * FROM Cart WHERE user_id = '{session_id}' ORDER BY date_added ASC");                                   

            var itemList = new List<Item>();

            
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Item item = new Item();

                    item.id = session_id.ToString();
                    item.restId = dt.Rows[i]["rest_id"].ToString();
                    item.name = dt.Rows[i]["item_name"].ToString();
                    item.price = dt.Rows[i]["item_price"].ToString();
                    item.datetime = dt.Rows[i]["date_added"].ToString();
                    itemList.Add(item);
                }
                return View(itemList);
        }

        public IActionResult Checkout(string id)
        {
            if (id == "checkout")
            {
                int session_id = Convert.ToInt32(HttpContext.Session.GetInt32(SessionUserId));
                con.InsertOrUpdate($"EXEC CreateOrder '{session_id}'");
                return RedirectToAction("Index");
            }

            return View();
        }




    }


}