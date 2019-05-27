using FeedMe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.ViewModels
{
    public class UserCreateViewModel
    {
        public User user { get; set; } 
        public CustomerInfo customerInfo { get; set; }
        public Role role { get; set; }
    }
}
