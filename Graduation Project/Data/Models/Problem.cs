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
            ProblemUsers = new HashSet<ProblemUser>();
        }
        public int ProblemId { get; set; }

        public string ProblemSource { get; set; }

        public string problemSourceId { get; set; }
        public string UrlSource { get; set; }

        public string problemTitle { get; set; }

        public int problemType { get; set; } //interview ==1 or contest==2

        public string ProblemHtml { get; set; }

        public int? rating { get; set; }

        public virtual ICollection<ProblemTag> ProblemTag { get; set; }
        public virtual ICollection<Submission> Submissions { get; set; }
        public virtual ICollection<ProblemUser> ProblemUsers { get; set; }
    }
}