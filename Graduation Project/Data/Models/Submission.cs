using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation_Project.Data.Models
{
    public class Submission
    {
        public int SubmissionId { get; set; }
        public float MemoryConsumeBytes { get; set; }
        public float TimeConsumeMillis { get; set; }
        public Boolean Visable { get; set; }
        public DateTime CreationTime { get; set; }
        public string Verdict { get; set; }
        public string ProgrammingLanguage { get; set; }
    }
}
