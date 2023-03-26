using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Repositories.DataBaseRepositories
{
    public class BlogDbRepository : IBlogRepository<Blog>
    {
        private readonly EntitiesContext _dbContext;
    public BlogDbRepository(EntitiesContext dbContext)
    {
        this._dbContext = dbContext;
    }

        public Blog Add(Blog newBlog)
        {
            _dbContext.Add(newBlog);
            Commit();
            return newBlog;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public Blog Find(int id)
        {
            return _dbContext.Blogs.Include(userBlog => userBlog.userBlog)
                .ThenInclude(user=>user.User)
                .Include(comment=> comment.Comments)
                .ThenInclude(commentVote=> commentVote.CommentVotes)
                .Include(group=>group.group)
                .FirstOrDefault(blog => blog.blogId == id);
        }

        public IList<Blog> List()
        {
            return _dbContext.Blogs.Include(userBlog=>userBlog.userBlog)
                 .ThenInclude(user => user.User)
                .Include(comment => comment.Comments)
                .ThenInclude(commentvote => commentvote.CommentVotes)
                .Include(group=>group.group)
                .ToList();
        }

        public void Remove(int id)
        {
            var blog = Find(id);
            if (blog != null)
            {
                _dbContext.Blogs.Remove(blog);
                Commit();
            }
        }

        

        public void Update(Blog newBlod)
        {
            var blog = Find(newBlod.blogId);
            blog.blogtitle = newBlod.blogtitle;
            blog.blogcontent = newBlod.blogcontent;
            Commit();
        }
        public void UpdateVote(int blogId,int userId,int typeVote)
        {
            var blog = Find(blogId);
            var userBlog = blog.userBlog.FirstOrDefault(User => User.userId == userId);

            if (userBlog == null)
            {
                UserBlog newuserBlog = new UserBlog
                {
                    blogId = blogId,
                    userId = userId,
                    blogOwenr = false,
                    VoteValue = typeVote,
                    isFavourite = false
                };
                blog.userBlog.Add(newuserBlog);
            }
            else if (userBlog.VoteValue == 0)
            {
                userBlog.VoteValue = typeVote;
            }
            else {
                return;
            }
            blog.blogvote = blog.blogvote + typeVote;
            Commit();

        }
        public void UpdateFavourite(int blogId, int userId) {
              var blog = Find(blogId);
            var userBlog = blog.userBlog.FirstOrDefault(User => User.userId == userId);

            if (userBlog == null)
            {
                UserBlog newuserBlog = new UserBlog
                {
                    blogId = blogId,
                    userId = userId,
                    blogOwenr = false,
                    VoteValue = 0,
                    isFavourite = true
                };
                blog.userBlog.Add(newuserBlog);
            }
            else if (userBlog.isFavourite == false)
            {
                userBlog.isFavourite = true;
            }
            else if (userBlog.isFavourite == true)
            {
                userBlog.isFavourite = false;
            }
            Commit();
        }
        public IList<Blog> Search(string Title, UserBlog PrepeardBy) {
           
            if (Title!=null && PrepeardBy != null)
            {
                var result = _dbContext.Blogs.Include(userBlog => userBlog.userBlog)
                    .ThenInclude(user => user.User)
                    .Include(comment => comment.Comments)
                    .Include(group => group.group).Where(blog => blog.blogtitle.Contains(Title) &&  blog.blogId == PrepeardBy.blogId
                   ).ToList();
                return result;

            }
            else if (PrepeardBy == null&& Title != null)
            {
                var result = _dbContext.Blogs.Include(userBlog => userBlog.userBlog)
                     .ThenInclude(user => user.User)
                     .Include(comment => comment.Comments)
                     .Include(group => group.group).Where(blog => blog.blogtitle.Contains(Title) 
                    ).ToList();
                return result;
            }else if(Title == null && PrepeardBy != null)
            {
                var result = _dbContext.Blogs.Include(userBlog => userBlog.userBlog)
                                     .ThenInclude(user => user.User)
                                     .Include(comment => comment.Comments)
                                     .Include(group => group.group).Where(blog => blog.blogId== PrepeardBy.blogId
                                    ).ToList();
                return result;
            }else 
            return null;
        }
    }
}
