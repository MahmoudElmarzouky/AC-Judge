using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation_Project.Data.Models
{
    public class Contest
    {
        public int contestId { get; set; }
        public string contestTitle { get; set; }
        public DateTime contestStartTime { get; set; }
        public int contestDuration { get; set; }
        public DateTime creationTime { get; set; }
        public string contestVisabilty { get; set; }
    }
}
