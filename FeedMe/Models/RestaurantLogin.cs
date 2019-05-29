using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.Models
{
    public class RestaurantLogin
    {
        public int rest_id { get; set; }
        public string email { get; set; }
        public string pw { get; set; }
        public int role_id { get; set; }
    }
}
