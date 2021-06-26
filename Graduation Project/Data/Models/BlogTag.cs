using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation_Project.Data.Models
{
    public class BlogTag
    {
        public int BlogId { get; set; }
        public int TagId { get; set; }
        public Blog blog { get; set; }
        public Tag tag { get; set; }

       
    }
}
