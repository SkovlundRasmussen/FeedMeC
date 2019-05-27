using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedMe.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace FeedMe.Controllers
{
    public class RestaurantController : Controller
    {
        private IMongoCollection<Restaurant> collection;

        public RestaurantController()
        {
            var client = new MongoClient("mongodb://localhost:32771");
            IMongoDatabase db = client.GetDatabase("FeedMe");
            this.collection = db.GetCollection<Restaurant>("Restaurant");
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var model = collection.Find(FilterDefinition<Restaurant>.Empty).ToList();
            return View(model);
        }
    }
}
