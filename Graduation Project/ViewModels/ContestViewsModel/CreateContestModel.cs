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
        public string problemId { get; set; } = "";
        public string Alias { get; set; } = "";
        public string PlatForm { get; set; } = ""; 
    }
    public class CreateContestModel
    {
        public CreateContestModel()
        {
            groups = new List<GroupData>();
            problems = new List<ProblemData>();
            for (int i = 0; i < 26; i++)
                problems.Add(new ProblemData()); 
        }
        public string CreateFromGroup { get; set; } = "0";
        public string FormOneClass { get { return "item contest-classical" + (CreateFromGroup == "1" ? "" : " active"); } set { } }
        public string FormTwoClass { get { return "item contest-group" + (CreateFromGroup == "1" ? " active" : ""); } set { } }
        public string ButtonOneClass { get { return CreateFromGroup == "1" ? "btn active btn-default" : "btn active btn-success"; } set { } }
        public string ButtonTwoClass { get { return CreateFromGroup == "1" ? "btn btn-primary" : "btn btn-default"; } set { } }
        public int groupId { get; set; }
        public string contestTitle { get; set; } = "";
        public string Password { get; set; } = "";
        public DateTime StartTime { get; set; } = DateTime.Now;
        public int Duration { get; set; }
        public string Visable { get; set; } = "";
        public IList<GroupData> groups { get; set; }
        public IList<ProblemData> problems { get; set; }
        public int contestId { get; set; }

    }
}
