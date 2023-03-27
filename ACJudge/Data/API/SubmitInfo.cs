using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACJudge.Data.API
{
    public class SubmitInfo
    {
        public string Submission { get; set; }
        public string Problem { get; set; }
        public string Verdict { get; set; }
        public string Test { get; set; }
        public string Time { get; set; }
        public string Memory { get; set; }
    }
}
