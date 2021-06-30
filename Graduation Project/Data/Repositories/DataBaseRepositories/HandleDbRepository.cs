﻿using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Repositories.DataBaseRepositories
{
    public class HandleDbRepository : IRepository<Handle>
    {
        readonly private EntitiesContext dbcontext;
        public HandleDbRepository(EntitiesContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public Handle Add(Handle newHandle)
        {
            dbcontext.Add(newHandle);
            Commit();
            return newHandle;
        }

        public void Commit()
        {
            dbcontext.SaveChanges();
        }

        public Handle Find(int Id)
        {
            var handle = dbcontext.Handles.FirstOrDefault(handle => handle.handleId == Id);
            return handle;
        }

        public IList<Handle> List()
        {
            return dbcontext.Handles.ToList();
        }

        public void Remove(int Id)
        {
            var handle = Find(Id);
            if (handle != null)
            {
                dbcontext.Handles.Remove(handle);
                Commit();
            }
        }

        public IList<Handle> search(int x, IList<string> list)
        {
            throw new NotImplementedException();
        }

        public void Update(Handle newHandle)
        {
            dbcontext.Handles.Update(newHandle);
            Commit();
        }
    }
}
