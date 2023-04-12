using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACJudge.Data.Models;

namespace ACJudge.ViewModels.ProblemViewsModel
{
    public class ViewProblemModel
    {
        public int ProblemId { get; set; }
        public string Status { get; set; }
        public bool Favorite { get; set; }
        public string OnlineJudge { get; set; }
        public string ProblemSourceId { get; set; }
        public string UrlSource { get; set; }
        public string Title { get; set; }
        public int? Rating { get; set; }
    }
    public interface IFilter<T>
    {
        public bool Valid(T type);
    }
    public class ProblemFilter
    {
        public enum ProblemType
        {
            Practice,
            Interview
        };

        public ProblemType Type { get; set; } = 0;
        public string ProblemId { get; set; } = "";
        public string ProblemName { get; set; } = "";
        public string ProblemSource { set; get; } = "";
    }

    public class StatusFilter
    {
        public string UserName { get; set; } = "";
        public string ProblemName { get; set; } = "";
        public string ProblemSource { get; set; } = "";
        public string ProblemResult { get; set; } = "";
        public string ProblemLanguage { get; set; } = "";
        public int ContestId { get; set; } = 0;

        public bool Valid(Submission submission)
        {
            // TODO complete the method 
            try
            {
                if (submission.User.UserName.Equals(UserName)) return true;
                if (submission.Problem.ProblemTitle.Equals(ProblemName)) return true;
                if (submission.Problem.ProblemSource.Equals(ProblemSource)) return true;
                if (submission.ProgrammingLanguage.Equals(ProblemLanguage)) return true;
                if (submission.ContestId == ContestId) return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
    }



    public class ProblemPageView<T, TFilter>
    {
        public ProblemPageView(IEnumerable<T> items, int pageNumber, int totalNumberOfPages, TFilter filter, bool isLogin)
        {
            TotalNumberOfPages = totalNumberOfPages;
            Items = items;
            PageNumber = pageNumber;
            Filter = filter;
            IsLogin = isLogin;
        }
        
        public IEnumerable<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int TotalNumberOfPages;

        public ProblemPageView()
        {
        }

        public TFilter Filter { get; set; }
        public bool IsLogin { set; get; }
    }
}
