using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace GraduationProject.Data.Models
{
    public class DbInitializer
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            EntitiesContext context =
                serviceProvider.GetRequiredService<EntitiesContext>();
            if (context.Submissions.Any()) return;
            foreach (var u in context.Users)
            {
                try
                {
                    if (context.UserContest.FirstOrDefault(u=>u.UserId == u.UserId && u.ContestId == 56) == null)
                    context.UserContest.Add(new UserContest { UserId = u.UserId, ContestId = 56, isRegistered = true , isFavourite = false, isOwner = false});
                }catch
                {
                    continue; 
                }
            }
            context.SaveChanges(); 
            var contest = context.Contests.FirstOrDefault(c => c.contestId == 56);
            var users = contest.UserContest.Where(u => u.isRegistered == true);
            for (int i = 0; i < 10; i++)
                context.Problems.Add(CreateProblem(i.ToString()));
            context.SaveChanges();
            var pr = context.Problems;
            int x = contest.ContestProblems.Count + 1;
            foreach (var p in pr)
                try
                {
                    contest.ContestProblems.Add(new ContestProblem { contestId = contest.contestId, order = x++, problemId = p.ProblemId });
                }
                catch
                {
                    continue;
                }
                LoadCurrentContest(context, contest);

            var problems = contest.ContestProblems;
            context.SaveChanges();
            foreach (var u in users)
            {
                foreach(var p in problems)
                {
                        contest.Submissions.Add(CreateSubmission(u.UserId, p.problemId, contest.contestId, "WrongAnswer"));
                }
            }
            foreach (var u in users)
            {
                foreach (var p in problems)
                {
                    contest.Submissions.Add(CreateSubmission(u.UserId, p.problemId, contest.contestId, "Accepted"));
                }
            }
            context.SaveChanges(); 
            
        }
        private static void LoadCurrentContest(EntitiesContext dbcontext, Contest contest)
        {
            dbcontext.Entry(contest).Collection(c => c.ContestProblems).Load();
            dbcontext.Entry(contest).Collection(c => c.UserContest).Load();
            dbcontext.Entry(contest).Collection(c => c.Submissions).Load();
            foreach (var cp in contest.ContestProblems)
                dbcontext.Entry(cp).Reference(c => c.problem).Load();
            foreach (var uc in contest.UserContest)
                dbcontext.Entry(uc).Reference(u => u.User).Load();
        }
        
        private static Problem CreateProblem(string name)
        {
            return new Problem
            {
                problemTitle = name
            }; 
        }
        private static Submission CreateSubmission(int userId, int problemId, int contestId, string Verdict)
        {
            return new Submission
            {
                CreationTime = DateTime.Now,
                InContest = true,
                contestId = contestId,
                ProblemId = problemId,
                Verdict = Verdict, 
                userId = userId
            }; 
        }
    }
}
