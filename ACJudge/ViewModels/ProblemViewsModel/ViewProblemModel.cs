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
        public string ProblemId { get; set; } = "";
        public string ProblemName { get; set; } = "";
        public string ProblemSource { set; get; } = "";
    }

    public class StatusFilter
    {
        
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
