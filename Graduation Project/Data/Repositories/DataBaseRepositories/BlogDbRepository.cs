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
        readonly private EntitiesContext dbcontext;
    public BlogDbRepository(EntitiesContext dbcontext)
    {
        this.dbcontext = dbcontext;
    }

        public Blog Add(Blog newBlog)
        {
            dbcontext.Add(newBlog);
            Commit();
            return newBlog;
        }

        public void Commit()
        {
            dbcontext.SaveChanges();
        }

        public Blog Find(int Id)
        {
            var blog = dbcontext.Blogs.Include(userBlog => userBlog.userBlog)
                .ThenInclude(user=>user.User)
                .Include(comment=>comment.Comments)
                .Include(group=>group.group)
                .FirstOrDefault(blog => blog.blogId == Id);
            return blog;
        }

        public IList<Blog> List()
        {
            return dbcontext.Blogs.Include(userBlog=>userBlog.userBlog)
                 .ThenInclude(user => user.User)
                .Include(comment => comment.Comments)
                .Include(group=>group.group)
                .ToList();
        }

        public void Remove(int Id)
        {
            var blog = Find(Id);
            if (blog != null)
            {
                dbcontext.Blogs.Remove(blog);
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
            userBlog.VoteValue = typeVote;
            blog.userBlog.Remove(blog.userBlog.FirstOrDefault(User => User.userId == userId));
            blog.userBlog.Add(userBlog);
            Commit();

        }
    }
}
