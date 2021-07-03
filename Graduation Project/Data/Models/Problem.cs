using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class Problem
    {
        public Problem()
        {
            ProblemTag = new HashSet<ProblemTag>(); 
            Submissions = new HashSet<Submission>();
            ContestProblems = new HashSet<ContestProblem>();
            ProblemUsers = new HashSet<ProblemUser>(); 
        }
        public int ProblemId { get; set; }
        
        public string ProblemSource { get; set; }
        
        public int problemSourceId { get; set; }
        
        public string problemTitle { get; set; }
        
        public int problemType { get; set; } //interview ==1 or contest==2
        
        public int timelimit { get; set; }
        
        public int memorylimit { get; set; }
        
        public string inputType { get; set; }
        
        public string outputType { get; set; }
       
        public string problemText { get; set; }
        
        public int rating { get; set; }
        public virtual ICollection<ProblemTag> ProblemTag { get; set; }
        public virtual ICollection<Submission> Submissions { get; set; }
        public virtual ICollection<ContestProblem> ContestProblems { get; set; }
        public virtual ICollection<ProblemUser> ProblemUsers { get; set; }
    }
}
