using System.Linq;
using ACJudge.Data.Models;

namespace ACJudge.ViewModels.BlogViewModel;

public static class BlogViewMapper
{
    public static ViewBlogModel GetViewModel(Blog blog, Data.Models.User user)
    {
        var blogOwner = blog.UserBlog.First(b => b.BlogOwner).User;
            
        var isOwner = blogOwner?.UserIdentityId == user?.UserIdentityId;
        var isFavorite = user?.UserBlogs.
            FirstOrDefault(innerUserBlog => innerUserBlog.IsFavourite 
                                            && innerUserBlog.BlogId==blog.BlogId) != null;
            
        var blogOwnerObject = new BlogOwner
        {
            Id = blogOwner!.UserId,
            UserName = blogOwner!.UserName,
            PhotoUrl = blogOwner!.PhotoUrl
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
            GroupId= blog.GroupId, 
            IsOwner = isOwner,
            IsFavorite= isFavorite
        };
        return model;
    }
}