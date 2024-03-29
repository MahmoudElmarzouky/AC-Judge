﻿using System.Collections.Generic;
using System.Linq;
using ACJudge.Data.API;
using ACJudge.Data.Models;
using ACJudge.Data.Repositories.Interfaces;
using ACJudge.ViewModels.ProblemViewsModel;
using Microsoft.EntityFrameworkCore;

namespace ACJudge.Data.Repositories.DataBaseRepositories
{
    public class ProblemDbRepository : IProblemRepository<Problem>
    {
        private readonly EntitiesContext _dbContext;
        public ProblemDbRepository(EntitiesContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Problem Add(Problem newProblem)
        {
            _dbContext.Add(newProblem);
            Commit();
            return newProblem;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public Problem Find(int id)
        {
            return List().FirstOrDefault(problem => problem.ProblemId == id);
        }

        public IList<Problem> List()
        {
            return _dbContext.Problems
                .Include(p => p.Submissions)
                .Include(pu => pu.ProblemUsers)
                .Include(pu => pu.ProblemTag)
                .ThenInclude(t => t.Tag)
                .ToList();
        }

        public void Remove(int id)
        {
            var problem = Find(id);
            if (problem == null) return;
            _dbContext.Problems.Remove(problem);
            Commit();
        }

        public IList<Problem> Search(ProblemFilter filter)
        {
            var items = new List<Problem>();
            var typeIndex = (int)filter.Type;
            var problemId= filter.ProblemId;
            var problemName = string.IsNullOrEmpty(filter.ProblemName)? string.Empty: filter.ProblemName;
            var problemSource = string.IsNullOrEmpty(filter.ProblemSource) || 
                                filter.ProblemSource.Contains("All")? string.Empty: filter.ProblemSource;
            
            for (var i = 0; i < 2; i++)
            {
                items = _dbContext.Problems.Where(item =>
                    item.ProblemType == typeIndex
                    && item.ProblemSourceId.Contains(problemId)
                    && item.ProblemTitle.Contains(problemName)
                    && item.ProblemSource.Contains(problemSource)
                ).ToList();
                if (items.Count != 0) break;
                _addProblemFromOnlineJudge(problemSource,problemId);
            }
        
            return items;
        }

        public void Update(Problem newProblem)
        {
            _dbContext.Problems.Update(newProblem);
            Commit();
        }

        public Problem FindByName(string onlineJudge, string problemSourceId)
        {
            problemSourceId = problemSourceId.ToUpper();
            var problem = List().FirstOrDefault(u => u.ProblemSourceId.Equals(problemSourceId) && 
                                                     u.ProblemSource.Equals(onlineJudge));
            if (problem != null) return problem;
            _addProblemFromOnlineJudge(onlineJudge, problemSourceId);
            problem = List().FirstOrDefault(u => u.ProblemSourceId.Equals(problemSourceId) && 
                                                 u.ProblemSource.Equals(onlineJudge));
            return problem;
        }

        private void _addProblemFromOnlineJudge(string onlineJudge, string problemSourceId)
        {
            var problemIdentifiers = _getProblemIdentifiers(problemSourceId);
            var id = problemIdentifiers[0];
            var c = problemIdentifiers[1];
            // TODO check if id, c are correct before call API 
            try
            {
                var p = APi.GetProblem(onlineJudge, id, c).Result;
                if (p == null) return;

                var newProblem = new Problem()
                {
                    ProblemSource = p.Source,
                    ProblemSourceId = p.ProblemId,
                    ProblemTitle = p.Title[2..],
                    ProblemType = 0,
                    ProblemInHtmlForm = p.Problem,
                    Rating = p.Rate,
                    UrlSource = "https://codeforces.com/problemset/problem/" + id + "/" + c
                };
                Add(newProblem);
                _addProblemTags(newProblem.ProblemId, p.Tags);
            }
            catch
            {
                // ignored 
            }
        }

        private void _addProblemTags(int problemId, IList<string> tags)
        {
            for (var i = 0; i < tags.Count - 1; i++)
            {
                var tag = _dbContext.Tags.FirstOrDefault(tag => tag.TagName == tags[i]);
                if (tag == null)
                {
                    tag = new Tag()
                    {
                        TagName = tags[i]
                    };
                    _dbContext.Tags.Add(tag);
                    Commit();
                }
                var tagId = tag.TagId;
                _dbContext.ProblemTag.Add(new ProblemTag() { ProblemId = problemId, TagId = tagId });
                Commit();
            }
        }

        private static List<string> _getProblemIdentifiers(string problemSourceId)
        {
            // --------------- Convert Source Id to Contest Id and problem char
            var flag = true;
            var id = "";
            var c = "";
            foreach (var item in problemSourceId)
            {
                if (char.IsLetter(item))
                {
                    flag = false;
                    c += item;
                    continue;
                }

                _ = flag ? id += item : c += item;
            }
            return new List<string> { id, c };
        }
    }
}
