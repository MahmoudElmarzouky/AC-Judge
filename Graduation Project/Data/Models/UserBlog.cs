using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation_Project.Data.Models
{
    public class UserBlog
    {
        public int userId { get; set; }
        public int blogId { get; set; }
        public Boolean isFavourite { get; set; }
        public int blogOwenr { get; set; } // ID for user owner 

        public virtual User User { get; set; }
        public virtual Blog blog { get; set; }

    }
}
