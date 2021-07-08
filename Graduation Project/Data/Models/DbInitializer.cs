using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace GraduationProject.Data.Models
{
    public class DbInitializer
    {
        static EntitiesContext dbcontext;
        public static void Seed(IServiceProvider serviceProvider)
        {
            var rand = new Random();
            dbcontext =
                serviceProvider.GetRequiredService<EntitiesContext>();
            var d = DateTime.Now;
            dbcontext.Submissions.RemoveRange(dbcontext.Submissions); 
            dbcontext.SaveChanges();

            string code = "#include<iostream>;" +
                "using namespace std;" +
                "int main(){" +
                "int n;" +
                "cin >> n;" +
                "for(int i = 0; i < n; i++)" +
                "{" +
                "int x;" +
                "cin >> x;" +
                "if (x > 3)" +
                "   cout << \"Hello From the other hand\";" +
                "else " +
                "cout << \"Hello From The hand\";" +
                "}" +
                "}";

            string[] verdicts = { "Accepted", "Wrong Answer", "Time limit exceeded" };
            var users = dbcontext.Users;
            var problems = dbcontext.Problems;
                foreach(var p in problems)
                {
                    foreach(var u in users)
                    {
                        int lim = 1 + rand.Next(3); 
                        for (int i = 0; i < lim; i++)
                        {
                            AddSubmission(0, p.ProblemId, u.UserId, verdicts[i], code);

                        }
                        dbcontext.SaveChanges(); 
                    }

                }
            
        }
        private static void AddSubmission(int contestId, int problemId, int userId, string verdict, string code)
        {
            var sub = new Submission
            {
                contestId = contestId,
                CreationTime = DateTime.Now,
                InContest = true,
                ProblemId = problemId,
                MemoryConsumeBytes = 200,
                ProgrammingLanguage = "C++",
                Visable = true,
                userId = userId,
                Verdict = verdict,
                SubmissionText = code,
                TimeConsumeMillis = 3055
            };
            dbcontext.Submissions.Add(sub);
            dbcontext.SaveChanges(); 
        }
        private static void LoadCurrentContest(Contest contest)
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
