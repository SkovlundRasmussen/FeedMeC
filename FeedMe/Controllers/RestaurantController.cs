using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedMe.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeedMe.Controllers
{
    public class RestaurantController : Controller
    {
        private IMongoCollection<MenuItem> collection;

        public RestaurantController()
        {
            var client = new MongoClient("mongodb://localhost:32771");
            IMongoDatabase db = client.GetDatabase("FeedMe");
            this.collection = db.GetCollection<MenuItem>("MenuItems");
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var model1 = collection.Find(FilterDefinition<MenuItem>.Empty).ToList();
            return View(model1);
        }
    }
}
