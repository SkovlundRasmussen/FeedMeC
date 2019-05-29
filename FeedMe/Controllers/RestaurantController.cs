using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedMe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FeedMe.Controllers
{
    public class RestaurantController : Controller
    {
        private IMongoCollection<Order> order;
        private IMongoCollection<Restaurant> collection;
        private string restaurant;
        
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
            restaurant = model.ToString();
            return View(model);
        }

        public IActionResult Add(string id)
        {
            var model = collection.Find(document => document.Id == id).ToList();
            return RedirectToPage("Detail/" + model, "Restaurant");
        }


    }
}
