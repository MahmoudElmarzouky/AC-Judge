using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class Group
    {
        public Group()
        {
            UserGroup = new HashSet<UserGroup>();
            blogs = new HashSet<Blog>();
            Contests = new HashSet<Contest>(); 
        }
        public int GroupId { get; set; }
        public String GroupTitle { get; set; }
        public DateTime creationTime { get; set; }
        public string GroupDescription { get; set; }
        public Boolean Visable { get; set; }
        public string Password { get; set; }
        public virtual ICollection<UserGroup> UserGroup { get; set; }
        public virtual ICollection<Blog> blogs { get; set; }
        public virtual ICollection<Contest> Contests { get; set; }
    }
}
