﻿using System;
using System.Linq;
using ACJudge.Data.API;
using ACJudge.Data.Repositories.DataBaseRepositories;
using ACJudge.ViewModels.ProblemViewsModel;
using Bogus;
using Bogus.DataSets;
using Microsoft.Extensions.DependencyInjection;

namespace ACJudge.Data.Models
{
    public class DbInitializer
    {
        private static EntitiesContext _dbContext;
        private static Random rand;
        private static readonly bool _generate = true;
        public static void Seed(IServiceProvider serviceProvider)
        {
             if (!_generate) return;
             rand = new Random();
            _dbContext = serviceProvider.GetRequiredService<EntitiesContext>();
            removeBigTitle();
        }

        private static void removeBigTitle()
        {
            var gorupRepo = new GroupDbRepository(_dbContext);
            var items = gorupRepo.MyGroups(1);
            for (var i = 0; i < items.Count; i++)
            {
                var title = items[i].GroupTitle.Split(" ");
                string newTitle = null;
                if (title.Length > 2)
                {
                    newTitle = title[0] + " " + title[1];
                }

                if (newTitle != null)
                {
                    items[i].GroupTitle = newTitle;
                }
            }
            gorupRepo.Commit();
        }

        private static void addGroups(int count = 1)
        {
            var gorupRepo = new GroupDbRepository(_dbContext);
            var groupGenerator = GetGroupGenerator();
            for(var i = 0; i < count; i++)
                gorupRepo.CreateNewGroup(1, groupGenerator.Generate());
            gorupRepo.Commit();
        }

        private static void addProblems(int count = 1)
        {
            var problemRepo = new ProblemDbRepository(_dbContext);

            for (var i = 1; i <= count; i++)
            {
                for (char index = 'A'; index <= 'D'; index++)
                {
                    try
                    {
                        problemRepo.Search(new ProblemFilter { ProblemId = $"{i}{index}" });
                        Console.WriteLine($"problem {i}{index} added ");
                    }
                    catch
                    {
                        Console.WriteLine($"problem {i}{index} can't added ");
                        continue;
                    }
                }
            }
        }

        private static void _generateBlogs(int count = 1)
        {
            var generator = GetBlogGenerator();
            var blogs = generator.Generate(count);
            
            for (var i = 0; i < blogs.Count; i++)
            {
                var userBlog = new UserBlog
                {
                    UserId = 1,
                    BlogOwner = true,
                    IsFavourite = false,
                    VoteValue = 0
                };
                blogs[i].UserBlog.Add(userBlog);
            }
            
            _dbContext.Blogs.AddRange(blogs);
            _dbContext.SaveChanges();
        }

        private static Faker<Blog> GetBlogGenerator()
        {
            return new Faker<Blog>().RuleFor(b => b.BlogTitle, b => b.Random.Words(3 + rand.Next(1)))
                .RuleFor(b => b.BlogContent, b => b.Random.Words(50 + rand.Next(50)))
                .RuleFor(b => b.BlogVote, 0)
                .RuleFor(b => b.BlogVisibility, true);
        }

        private static Faker<Group> GetGroupGenerator()
        {
            return new Faker<Group>().RuleFor(g => g.Visible, true).RuleFor(g => g.GroupTitle, g => g.Name.JobTitle())
                .RuleFor(g => g.GroupDescription, g => g.Name.JobDescriptor());
        }

        private static void GetAllProblem()
        {
            for (int i = 33; i <= 60; i++)
            {
                for (char ca = 'a'; ca <= 'd'; ca++)
                {
                    var ProblemSourceId = i.ToString() + ca;
                    var OnlineJudge = "CodeForces";
                    var problem = _dbContext.Problems.FirstOrDefault(u => u.ProblemSourceId == ProblemSourceId && u.ProblemSource == OnlineJudge);
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
                            c = c.ToUpper();
                            // ------------------------------------------------------------------

                            var p = APi.GetProblem(OnlineJudge, id, c).Result;

                            if (p == null) continue;
                            var newProblem = new Problem()
                            {
                                ProblemSource = p.Source,
                                ProblemSourceId = p.ProblemId,
                                ProblemTitle = p.Title.Substring(2),
                                ProblemType = 2,
                                ProblemInHtmlForm = p.Problem,
                                Rating = p.Rate,
                                UrlSource = "https://codeforces.com/problemset/problem/" + id + "/" + c
                            };
                            _dbContext.Add(newProblem);
                            _dbContext.SaveChanges();
                        }
                    }

                }

            }
        }
        private static void AddSubmission(int? contestId, int problemId, int userId, string verdict, string code)
        {
            var sub = new Submission
            {
                ContestId = contestId,
                CreationTime = DateTime.Now,
                ProblemId = problemId,
                MemoryConsumeBytes = "200",
                ProgrammingLanguage = "C++",
                Visible = false,
                UserId = userId,
                Verdict = verdict,
                SubmissionText = code,
                TimeConsumeMillis = "3055"
            };
            _dbContext.Submissions.Add(sub);

        }
        private static void LoadCurrentContest(Contest contest)
        {
            _dbContext.Entry(contest).Collection(c => c.ContestProblems).Load();
            _dbContext.Entry(contest).Collection(c => c.UserContest).Load();
            _dbContext.Entry(contest).Collection(c => c.Submissions).Load();
            foreach (var cp in contest.ContestProblems)
                _dbContext.Entry(cp).Reference(c => c.Problem).Load();
            foreach (var uc in contest.UserContest)
                _dbContext.Entry(uc).Reference(u => u.User).Load();
        }
        
        private static Problem CreateProblem(string name)
        {
            return new Problem
            {
                ProblemTitle = name
            }; 
        }
        private static Submission CreateSubmission(int userId, int problemId, int contestId, string Verdict)
        {
            return new Submission
            {
                CreationTime = DateTime.Now,
                ContestId = contestId,
                ProblemId = problemId,
                Verdict = Verdict, 
                UserId = userId
            }; 
        }
    }
}
