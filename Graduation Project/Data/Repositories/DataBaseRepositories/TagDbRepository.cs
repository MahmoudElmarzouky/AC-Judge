﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;

namespace GraduationProject.Data.Repositories.DataBaseRepositories
{
    public class TagDbRepository : IRepository<Tag>
    {
        readonly private EntitiesContext dbcontext;
        public TagDbRepository(EntitiesContext dbcontext)
        {
            this.dbcontext = dbcontext; 
        }
        public Tag Add(Tag newTag)
        {
            dbcontext.Tags.Add(newTag);
            Commit();
            return newTag; 
        }

        public void Commit()
        {
            dbcontext.SaveChanges(); 
        }

        public Tag Find(int Id)
        {
            var tag = dbcontext.Tags.FirstOrDefault(tag => tag.tagId == Id);
            return tag; 
        }

        public IList<Tag> List()
        {
            return dbcontext.Tags.ToList(); 
        }

        public void Remove(int Id)
        {
            var tag = Find(Id);
            if (tag != null)
            {
                dbcontext.Tags.Remove(tag);
                Commit(); 
            }
        }

        public void Update(Tag newTag)
        {
            var tag = Find(newTag.tagId);
            tag = newTag;
            Commit(); 
        }
    }
}
