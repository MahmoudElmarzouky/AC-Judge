using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class UserBlog
    {
        [Key, Column(Order = 0)]
        public int userId { get; set; }
        [Key, Column(Order = 1)]
        public int blogId { get; set; }
        public Boolean isFavourite { get; set; }
        public int blogOwenr { get; set; } // ID for user owner 

        public virtual User User { get; set; }
        public virtual Blog blog { get; set; }

    }
}
