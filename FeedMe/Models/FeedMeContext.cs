using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.Models
{
    public class FeedMeContext : DbContext
    {
        public DbSet<Role> Blogs { get; set; }
    }
}
