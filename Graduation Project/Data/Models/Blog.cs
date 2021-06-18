using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation_Project.Data.Models
{
    public class Blog
    {
        public int blogId { get; set; }
        public string blogtitle { get; set; }
        public string blogcontent { get; set; }
        public int blogvote { get; set; }
        public DateTime creationTime { get; set; }
        public string blogVisabilty { get; set; }
    }
}
