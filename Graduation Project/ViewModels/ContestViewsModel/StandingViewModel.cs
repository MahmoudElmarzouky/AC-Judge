using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProject.Data.Models;

namespace GraduationProject.ViewModels.ContestViewsModel
{
    public class NavInfo
    {
        public int? groupId { get; set; }
        public string contestTitle { get; set; }
        public DateTime contestStartTime { get; set; }
        public int contestDuration { get; set; }
        public Boolean IsCurrentUserOwner { get; set; }
    }
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
                        return "submit-standing Pfsolve";
                }
                else
                {
                        return "submit-standing Psolve";
                }
            }else
            {
                if (NumberOfSubmissions > 0)
                {
                        return "submit-standing Pattemped";
                }
                    else
                {
                        return "submit-standing Punattemped"; 
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
        public int NumberOfSolvedProblems { get; set; } = 0; 
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
            NavInfo = new NavInfo(); 
        }
        public int contestId { get; set; }
        public int NumberOfProblems { get; set; }
        public int NumberOfUsers { get {
                return users.Count; 
            } set { } }
        public IList<UserInStanding> users { get; set;}
        public NavInfo NavInfo { get; set; }
    }
}
