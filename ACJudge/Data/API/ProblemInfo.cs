using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACJudge.Data.API
{
    public class ProblemInfo
    {
        public string Title { get; set; }
        public string Source { get; set; }
        public string ProblemId { get; set; }
        public int Rate { get; set; }
        public IList<string> Tags { get; set; }
        public string Problem { get; set; }
    }
}
