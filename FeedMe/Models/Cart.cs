using System;
using System.Collections;
using System.Collections.Generic;

namespace FeedMe.Models
{
    public class Cart
    {
        public string userId { get; set; }
        public string restId { get; set; }

        public string itemName { get; set; }
        public string itemPrice { get; set; }
        public string dateAdded { get; set; }

        List<Item> items = new List<Item>();

        public void AddToCart(Item item)
        {
            items.Add(item);
        }

        public void RemoveFromCart(Item item)
        {
            items.Remove(item);
        }
    }
}