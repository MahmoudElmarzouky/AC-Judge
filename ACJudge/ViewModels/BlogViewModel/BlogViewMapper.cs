using System;
using System.Linq;
using ACJudge.Data.Models;

namespace ACJudge.ViewModels.BlogViewModel;

public static class BlogViewMapper
{
    public static ViewBlogModel GetViewModel(Blog blog, Data.Models.User user)
    {
        try{
            var blogOwner = blog.UserBlog.First(b => b.BlogOwner).User;
            var currentUserBlog = blog.UserBlog.First(ub => ub.UserId == user.UserId);
            var isOwner = blogOwner.UserId == user.UserId;
            var isFavorite = currentUserBlog.IsFavourite;

            var blogOwnerObject = new BlogOwner
            {
                Id = blogOwner.UserId,
                UserName = blogOwner.UserName,
                PhotoUrl = blogOwner.PhotoUrl
            };
            var model = new ViewBlogModel
            {
                BlogId = blog.BlogId,
                BlogTitle = blog.BlogTitle,
                BlogOwner = blogOwnerObject,
                BlogContent = blog.BlogContent,
                BlogVote = blog.BlogVote,
                CreationTime = blog.CreationTime,
                Comments = blog.Comments,
                CurrentUserId = user!.UserId,
                GroupId = blog.GroupId,
                IsOwner = isOwner,
                IsFavorite = isFavorite
            };
            return model;
        }catch
        {
            return new ViewBlogModel();
        }
    }
}