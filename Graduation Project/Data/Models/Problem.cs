using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation_Project.Data.Models
{
    public class Problem
    {
        public Problem()
        {
            ProblemTag = new HashSet<ProblemTag>(); 
        }
        public int ProblemId { get; set; }
        
        public string ProblemSource { get; set; }
        
        public int problemSourceId { get; set; }
        
        public string problemTitle { get; set; }
        
        public int problemType { get; set; } //interview or contest
        
        public int timelimit { get; set; }
        
        public int memorylimit { get; set; }
        
        public string inputType { get; set; }
        
        public string outputType { get; set; }
       
        public string problemText { get; set; }
        
        public int rating { get; set; }
        public virtual ICollection<ProblemTag> ProblemTag { get; set; }
    }
}
