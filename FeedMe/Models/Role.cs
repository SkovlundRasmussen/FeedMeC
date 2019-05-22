using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.Models
{
    public class Role
    {
        public int role_id { get; set; }
        public string role_name { get; set; }
        public object Name { get; internal set; }
    }
}
