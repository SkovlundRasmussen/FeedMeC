using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.Models
{
    public class CustomerInfo
    {
        public string street_name { get; set; }
        public string street_number { get; set; }
        public int postal_code { get; set; }
        public string city { get; set; }

        public int user_id { get; set; }
    }
}
