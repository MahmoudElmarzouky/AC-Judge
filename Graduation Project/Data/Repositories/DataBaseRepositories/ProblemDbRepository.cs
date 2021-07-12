﻿using System;
using System.Collections.Generic;
using System.Linq;
using GraduationProject.Data.Models;
using GraduationProject.Data.API;
using GraduationProject.Data.Repositories.IProblemRepository;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Data.Repositories.DataBaseRepositories
{
    public class ProblemDbRepository : IProblemRepository<Problem>
    {
        readonly private EntitiesContext dbcontext;
        public ProblemDbRepository(EntitiesContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public Problem Add(Problem newProblem)
        {
            dbcontext.Add(newProblem);
            Commit();
            return newProblem;
        }

        public void Commit()
        {
            dbcontext.SaveChanges();
        }

        public Problem Find(int Id)
        {
            var problem = dbcontext.Problems
                .Include(p => p.Submissions)
                .Include(pu => pu.ProblemUsers)
                .Include(pu => pu.ProblemTag)
                .ThenInclude(t => t.Tag)
                .FirstOrDefault(problem => problem.ProblemId == Id);
            return problem;
        }

        public IList<Problem> List()
        {
            return dbcontext.Problems
                .Include(p => p.Submissions)
                .Include(pu => pu.ProblemUsers)
                .Include(pu => pu.ProblemTag)
                .ThenInclude(t => t.Tag)
                .ToList();
        }

        public void Remove(int Id)
        {
            var problem = Find(Id);
            if (problem != null)
            {
                dbcontext.Problems.Remove(problem);
                Commit();
            }
        }

        public IList<Problem> Search(int x, IList<string> list)
        {
            var items = new List<Problem>();
            if (x == 1)
            {
                int type = int.Parse(list[0]);
                items = dbcontext.Problems.Where(item => item.problemType == type).ToList();
            }
            else if (x == 2)
            {
                int type = int.Parse(list[0]);
                string problemID = list[1];
                string problemName = list[2];
                string ProblemSource = (list[3] == "All" ? "" : list[3]);
                problemID = (problemID == null ? "" : problemID);
                problemName = (problemName == null ? "" : problemName);
                ProblemSource = (ProblemSource == null ? "" : ProblemSource);
                items = dbcontext.Problems.Where(item =>
                    item.problemType == type
                    && item.problemSourceId.Contains(problemID)
                    && item.problemTitle.Contains(problemName)
                    && item.ProblemSource.Contains(ProblemSource)
                ).ToList();
            }
            return items;
        }

        public void Update(Problem newProblem)
        {
            dbcontext.Problems.Update(newProblem);
            Commit();
        }

        public Problem FindByName(string OnlineJudge, string ProblemSourceId)
        {
            ProblemSourceId = ProblemSourceId.ToUpper();

            var problem = dbcontext.Problems
                .Include(p => p.Submissions)
                .Include(pu => pu.ProblemUsers)
                .Include(pu => pu.ProblemTag)
                .ThenInclude(t => t.Tag)
                .FirstOrDefault(u => u.problemSourceId == ProblemSourceId && u.ProblemSource == OnlineJudge);
            if (problem == null)
            {
                if (OnlineJudge == "CodeForces")
                {
                    // --------------- Convert Source Id to Contest Id and problem char
                    Boolean flag = true;
                    string id = "";
                    string c = "";
                    foreach (var item in ProblemSourceId)
                    {
                        if (Char.IsLetter(item))
                        {
                            flag = false;
                            c += item;
                            continue;
                        }
                        _ = (flag == true) ? id += item : c += item;
                    }
                    // ------------------------------------------------------------------
                    var p = APi.GetProblem(OnlineJudge, id, c);

                    if (p == null) return null;
                    Problem newproblem = new Problem()
                    {
                        ProblemSource = p.source,
                        problemSourceId = p.problemID,
                        problemTitle = p.title.Substring(2),
                        problemType = 1,
                        ProblemHtml = p.problem,
                        rating = p.rate,
                        UrlSource = "https://codeforces.com/problemset/problem/" + id + "/" + c
                    };
                    Add(newproblem);
                    for (int i = 0; i < p.tags.Count() - 1; i++)
                    {
                        var x = dbcontext.Tags.FirstOrDefault(tag => tag.tagName == p.tags[i]);
                        if (x == null)
                        {
                            Tag newTag = new Tag()
                            {
                                tagName = p.tags[i]
                            };
                            dbcontext.Tags.Add(newTag);
                            Commit();
                            int tagid= dbcontext.Tags.FirstOrDefault(tag => tag.tagName == p.tags[i]).tagId;
                            int problemid= dbcontext.Problems.FirstOrDefault(u => u.problemSourceId == ProblemSourceId && u.ProblemSource == OnlineJudge).ProblemId;
                            dbcontext.ProblemTag.Add(new ProblemTag() { ProblemId = problemid, TagId = tagid });
                            Commit();
                        }
                        else
                        {
                            int tagid = x.tagId;
                            int problemid = dbcontext.Problems.FirstOrDefault(u => u.problemSourceId == ProblemSourceId && u.ProblemSource == OnlineJudge).ProblemId;
                            dbcontext.ProblemTag.Add(new ProblemTag() { ProblemId = problemid, TagId = tagid });
                            Commit();
                        }
                        
                         
                    }

                    
                    return newproblem;
                }


            }
            return problem;
        }
    }
}
