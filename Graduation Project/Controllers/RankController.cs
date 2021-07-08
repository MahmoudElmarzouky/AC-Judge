using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using GraduationProject.ViewModels.Rank;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Controllers.Rank
{
    public class RankController : Controller
    {
        private readonly ISubmissionRepository<Submission> SubmissionRepository;
        private readonly IUserRepository<User> userRepository;
        public RankController(IUserRepository<User> _userRepository, ISubmissionRepository<Submission> _SubmissionRepository)
        {
            SubmissionRepository = _SubmissionRepository;
            userRepository = _userRepository;
        }
        public ActionResult Index()
        {
            ViewData["Countries"] = GetAllCountries();
            IOrderedEnumerable<RankViewModel> list = GetAllUserRank();
            return View(list);
        }
        public ActionResult FilterRanking()
        {
            var Country = Request.Form["Country"];
            var UserName = Request.Form["UserName"];
            var BirthYearLowerBound = Int32.Parse(Request.Form["BirthYearLowerBound"]);
            var BirthYearUpperBound = Int32.Parse(Request.Form["BirthYearUpperBound"]);
            var RatingLowerBound = Int32.Parse(Request.Form["RatingLowerBound"]);
            var RatingUpperBound = Int32.Parse(Request.Form["RatingUpperBound"]);
            ViewData["Countries"] = GetAllCountries();
            IOrderedEnumerable<RankViewModel> list = GetAllUserRank();
            var newlist = list.Where(e =>
                e.Country.Contains(Country)&&
                (UserName != "" ? e.UserName==UserName:e.UserName.Contains("")) &&
                e.Birthyear >= BirthYearLowerBound &&
                e.Birthyear <= BirthYearUpperBound &&
                e.TotalSolved >= RatingLowerBound &&
                e.TotalSolved <= RatingUpperBound 
                );
            return View("Index", newlist);
        }
        IOrderedEnumerable<RankViewModel> GetAllUserRank()
        {
            var users = userRepository.List();
            IList<RankViewModel> list = new List<RankViewModel>();
            foreach (var item in users)
            {
                RankViewModel tmp = new RankViewModel
                {
                    userid = item.UserId,
                    Birthyear = item.BirthDate,
                    Country = item.Country,
                    UserName = item.UserName
                };
                tmp.TotalSolved = item.submissions.Where(s => s.Verdict == "Accepted").Select(p => p.ProblemId).Distinct().Count();

                list.Add(tmp);
            }
            IOrderedEnumerable<RankViewModel> newlist = list.OrderByDescending(T => T.TotalSolved);
            return newlist;
        }
        IEnumerable<string> GetAllCountries()
        {
            List<string> Countries = new List<string>();
            CultureInfo[] cultureInfos = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo cultureInfo in cultureInfos)
            {
                RegionInfo regionInfo = new(cultureInfo.LCID);
                if (!Countries.Contains(regionInfo.EnglishName))
                {
                    Countries.Add(regionInfo.EnglishName);
                }
            }
            Countries.Remove("Israel");
            Countries.Add("Palastine");
            Countries.Sort();
            return Countries;
        }
    }
    
}