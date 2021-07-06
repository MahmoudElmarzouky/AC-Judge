using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProject.Data.Models;

namespace GraduationProject.ViewModels.ContestViewsModel
{
    public class Data
    {
        public Data()
        {
            Submissions = new List<Submission>(); 
        }
        public Boolean FirstAcceptedSubmission { get; set; } = false;
        public int problemPenality { get; set; }
        public int problemId { get; set; }
        public string td_className { get { 
            if(Solved)
            {
                if (FirstAcceptedSubmission)
                {
                        return "bg-primary";
                }
                else
                {
                        return "bg-success";
                }
            }else
            {
                if (NumberOfSubmissions > 0)
                {
                        return "bg-danger";
                }
                    else
                {
                        return "bg-info"; 
                }
            }
            } set { } }
        public int NumberOfSubmissions { get; set; } = 0;
        public Boolean Solved { get; set; }
        public IList<Submission> Submissions { get; set; }
        
    }
    public class UserInStanding
    {
        public UserInStanding()
        {
            UserPoblemsRaw = new List<Data>(); 
        }
        public int NumberOfSolvedProblems { get; set; }
        public int TotalPenality { get {
                return UserPoblemsRaw.Where(u => u.Solved == true).Sum(u => u.problemPenality); 
            } set { } }
        public int userId { get; set; }
        public string userName { get; set; }
        public IList<Data> UserPoblemsRaw { get; set; }
    }
    public class StandingViewModel
    {
        public StandingViewModel()
        {
            users = new List<UserInStanding>(); 
        }
        public int contestId { get; set; }
        public int NumberOfProblems { get; set; }
        public int NumberOfUsers { get {
                return users.Count; 
            } set { } }
        public IList<UserInStanding> users { get; set;}  
    }
}
