using GraduationProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.ViewModels
{
    public class ViewBlogModel
    {
        public int blogId { get; set; }
       
        public string blogtitle { get; set; }
        public string blogcontent { get; set; }
        public int blogvote { get; set; }
        public DateTime creationTime { get; set; }
        public string blogOwner { get; set; }
        public bool isOwner { get; set; }
        public int CurrentUserId { get; set; }
        public int? GroupId { get; set; }
        public  ICollection<Comment> Comments { get; set; }
        public  ICollection<UserBlog> UserBlogs { get; set; }

    }
}
