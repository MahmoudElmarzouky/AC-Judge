using System;
using System.Collections.Generic;

namespace ACJudge.Data.Models
{
    public sealed class Blog
    {
        public Blog()
        {
            
        }

        public Blog(string blogTitle, string blogContent, int? groupId, int ownerId)
        {
            BlogTitle = blogTitle;
            BlogContent = blogContent;
            GroupId = groupId;
            BlogVisibility = groupId != null;
            BlogVote = 0;
            UserBlog.Add(new UserBlog(ownerId));
        }

        public int BlogId { get; set; }
        public string BlogTitle { get; set; }
        public string BlogContent { get; set; }
        public int BlogVote { get; set; }
        public DateTime CreationTime { get; } = DateTime.Now;
        public bool BlogVisibility { get; set; }
        public ICollection<UserBlog> UserBlog { get; set; } = new HashSet<UserBlog>();
        public ICollection<BlogTag> BlogTag { get; set; } = new HashSet<BlogTag>();
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public int? GroupId { get; set; } 
        public Group Group { get; set; }
    }
}