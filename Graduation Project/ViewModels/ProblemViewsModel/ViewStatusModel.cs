using GraduationProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.ViewModels.ProblemViewsModel
{
    public class ViewStatusModel
    {
        public int RunID { get; set; }
        public Boolean Visiable { get; set; }
        public int UserId { get; set; }
        public String UserName { get; set; }
        public int ProblemId { get; set; }
        public int? contestId { get; set; }
        public string OnlineJudge { get; set; }
        public string ProblemSourcesId { get; set; }
        public string Verdict { get; set; }
        public string TimeConsumed { get; set; }
        public string MemoryConsumed { get; set; }
        public string Language { get; set; }
        public DateTime SubmitTime { get; set; }
        public string Title { get; set; }
    }
}
