using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACJudge.ViewModels.ProblemViewsModel
{
    public class ViewProblemDetails
    {
        public int problemId { get; set; }

        public string problemSource { get; set; }

        public string problemsourceId { get; set; }
        public string urlSource { get; set; }

        public string problemtitle { get; set; }
        public string Problemhtml { get; set; }

        public int? Rating { get; set; }

        public List<string> problemTag { get; set; }
        public int NumberAc { get; set; }
        public int Numbersubmission { get; set; }
        public Boolean IsFavorite { get; set; }
        public string userName { get; set; }
    }
}
