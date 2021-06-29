using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Repositories.DataBaseRepositories
{
    public class BlogDbRepository : IRepository<Blog>
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
            var blog = dbcontext.Blogs.FirstOrDefault(blog => blog.blogId == Id);
            return blog;
        }

        public IList<Blog> List()
        {
            return dbcontext.Blogs.ToList();
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
            dbcontext.Blogs.Update(newBlod);
            Commit();
        }
    }
}
