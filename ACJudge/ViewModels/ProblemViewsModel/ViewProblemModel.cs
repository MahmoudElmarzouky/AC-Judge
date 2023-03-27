using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACJudge.ViewModels.ProblemViewsModel
{
    public class ViewProblemModel
    {
        public int ProblemId { get; set; }
        public string Status { get; set; }
        public Boolean Favorite { get; set; }
        public string OnlineJudge { get; set; }
        public string ProblemSourceId { get; set; }
        public string UrlSource { get; set; }
        public string Title { get; set; }
        public int? rating { get; set; }
        
    }
}
