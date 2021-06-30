using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Repositories.DataBaseRepositories
{
    public class CommentDbRepository : IRepository<Comment>
    {
        readonly private EntitiesContext dbcontext;
        public CommentDbRepository(EntitiesContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public Comment Add(Comment newComment)
        {
            dbcontext.Add(newComment);
            Commit();
            return newComment;
        }

        public void Commit()
        {
            dbcontext.SaveChanges();
        }

        public Comment Find(int Id)
        {
            var comment = dbcontext.Comments.FirstOrDefault(comment => comment.commentId == Id);
            return comment;
        }

        public IList<Comment> List()
        {
            return dbcontext.Comments.ToList();
        }

        public void Remove(int Id)
        {
            var comment = Find(Id);
            if (comment != null)
            {
                dbcontext.Comments.Remove(comment);
                Commit();
            }
        }

        public IList<Comment> search(int x, IList<string> list)
        {
            throw new NotImplementedException();
        }

        public void Update(Comment newComment)
        {
            dbcontext.Comments.Update(newComment);
            Commit();
        }
    }
}
