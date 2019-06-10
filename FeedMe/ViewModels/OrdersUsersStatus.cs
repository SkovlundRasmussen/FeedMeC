using FeedMe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.ViewModels
{
    public class OrdersUsersStatus
    {
        public int order_id { get; set; }
        public User user { get; set; }
        public CustomerInfo CustomerInfo { get; set; }
        public Item item { get; set; }
        public OrderStatus status { get; set; }
    }
}
