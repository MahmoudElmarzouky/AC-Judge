using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation_Project.Data.Models
{
    public class Tag
    {
        public Tag()
        {
            ProblemTag = new HashSet<ProblemTag>(); 
        }
        public int tagId { get; set; }
        public string tagName { get; set; }
        public virtual ICollection<ProblemTag> ProblemTag { get; set; }
    }   
}
