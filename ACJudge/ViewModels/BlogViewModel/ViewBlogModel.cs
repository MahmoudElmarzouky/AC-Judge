using System;
using System.Collections.Generic;
using ACJudge.Data.Models;

namespace ACJudge.ViewModels.BlogViewModel
{
    public class BlogOwner
    {
        private readonly string _defaultPhotoUrl = "avatar-male.jpg";
        private string _photoUrl = "";
        public int Id { get; set; }
        public string UserName { get; set; }

        public string PhotoUrl
        {
            get => String.IsNullOrEmpty(_photoUrl) ? _defaultPhotoUrl : _photoUrl;
            set => _photoUrl = value;
        }
    }
    public class ViewBlogModel
    {
        public int BlogId { get; set; }
        public string BlogTitle { get; set; }
        public string BlogContent { get; set; }
        public int BlogVote { get; set; }
        public DateTime CreationTime { get; set; }
        public BlogOwner BlogOwner { get; set; }
        public bool IsOwner { get; set; }
        public bool IsFavorite { get; set; }
        public int CurrentUserId { get; set; }
        public int? GroupId { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }

    public class BlogPage
    {
        
        public IEnumerable<ViewBlogModel> Blogs;
        public int PageNumber { get; set; }
        public IEnumerable<int> Pages { get; set; }
        public BlogPage(IEnumerable<ViewBlogModel> blogs, int pageNumber, IEnumerable<int> pages)
        {
            Blogs = blogs;
            PageNumber = pageNumber;
            Pages = pages;
        }
    }
}
