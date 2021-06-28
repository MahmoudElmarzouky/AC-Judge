using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class Blog
    {
        public int blogId { get; set; }
        public string blogtitle { get; set; }
        public string blogcontent { get; set; }
        public int blogvote { get; set; }
        public DateTime creationTime { get; set; }
        public string blogVisabilty { get; set; }
        public virtual ICollection<UserBlog> userBlog { get; set; }
        public virtual ICollection<BlogTag> blogTag { get; set; }
        public virtual ICollection<Blog> blogs { get; set; }
        public int groupId { get; set; }
        public Group group { get; set; }

    }
}
