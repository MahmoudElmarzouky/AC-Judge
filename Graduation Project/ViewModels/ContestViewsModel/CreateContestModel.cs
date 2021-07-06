using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.ViewModels.ContestViewsModel
{
    public class GroupData
    {
        public int groupId { get; set; }
        public string groupTitle { get; set; }

    }
    public class ProblemData
    {
        public int problemId { get; set; }
        public string Alias { get; set; } = "";
    }
    public class CreateContestModel
    {
        public CreateContestModel()
        {
            groups = new List<GroupData>();
            problems = new List<ProblemData>(); 
        }
        public string CreateFromGroup { get; set; } = "0";
        public int groupId { get; set; }
        public string contestTitle { get; set; } = "";
        public string Password { get; set; } = "";
        public DateTime StartTime { get; set; } = DateTime.Now;
        public int Duration { get; set; }
        public string Visable { get; set; } = "";
        public IList<GroupData> groups { get; set; }
        public IList<ProblemData> problems { get; set; }
        
    }
}
