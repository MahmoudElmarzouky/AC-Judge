using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.ViewModels.ProblemViewsModel
{
    public class ViewProblemModel
    {
        public string Status { get; set; }
        public Boolean Favorite { get; set; }
        public string OnlineJudge { get; set; }
        public int ProblemSourceId { get; set; }
        public string Title { get; set; }
        public int rating { get; set; }
    }
}
