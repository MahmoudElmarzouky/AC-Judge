using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.IProblemRepository;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using GraduationProject.Data.Repositories.Interfaces;
using GraduationProject.ViewModels.ProblemViewsModel;
using System.Linq;

namespace GraduationProject.Controllers.problems
{
    public class ProblemController : Controller
    {
        private readonly IProblemRepository<Problem> problemRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly User user;
        private readonly IEnumerable<Submission> ListMysubmission;
        private readonly IEnumerable<ProblemUser> ListMyfavorite;
        public ProblemController(IUserRepository<User> Userrepository,IProblemRepository<Problem> problemRepository, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            this.problemRepository = problemRepository;
            user = Userrepository.Find(userId);
            ListMysubmission = user.submissions;
            ListMyfavorite = user.ProblemUsers;

        }
        public ActionResult Index()
        {
            var ListProblems = problemRepository.Search(1,new List<string> { "1" });
            List<ViewProblemModel> model = new List<ViewProblemModel>();
            foreach (var p in ListProblems)
            {
                ViewProblemModel item = new ViewProblemModel()
                {
                    ProblemId=p.ProblemId,
                    OnlineJudge = p.ProblemSource,
                    ProblemSourceId = p.problemSourceId,
                    Title = p.problemTitle,
                    rating = p.rating
                };
                var acsubmission = ListMysubmission.FirstOrDefault(s => s.ProblemId == p.ProblemId && s.Verdict == "Accept");
                if (acsubmission != null)
                {
                    item.Status = "Solved";
                }
                else
                {
                    var wrsubmission = ListMysubmission.FirstOrDefault(s => s.ProblemId == p.ProblemId && s.Verdict == "Wrong");
                    if (wrsubmission != null)
                        item.Status = "Attempted";
                    else
                        item.Status = "";
                }
                var Is_Favorite = ListMyfavorite.FirstOrDefault(f => f.IsFavourite == true && f.ProblemId == p.ProblemId);
                if (Is_Favorite != null)
                    item.Favorite = true;
                else
                    item.Favorite = false;

                model.Add(item);

            }

            return View(model);
        }
        public ActionResult Status()
        {
            return View();
        }
        public ActionResult Filter()
        {
            var problemID = Request.Form["problemID"];
            var problemName = Request.Form["problemName"];
            var ProblemSource = Request.Form["ProblemSource"];
            var list = new List<Problem>();
            if (problemID != "" && problemName != "" && ProblemSource != "All")
            {
                list = (List<Problem>)problemRepository.Search(2, new List<string> { "1", problemID, problemName, ProblemSource });
            }
            else if (problemID != "" && problemName != "")
            {
                list = (List<Problem>)problemRepository.Search(3, new List<string> { "1", problemID, problemName });
            }
            else if (problemID != "" && ProblemSource != "All")
            {
                list = (List<Problem>)problemRepository.Search(4, new List<string> { "1", problemID, ProblemSource });
            }
            else if (problemName != "" && ProblemSource != "All")
            {
                list = (List<Problem>)problemRepository.Search(5, new List<string> { "1", problemName, ProblemSource });
            }
            else if (problemID != "")
            {
                list = (List<Problem>)problemRepository.Search(6, new List<string> { "1", problemID });
            }
            else if (problemName != "")
            {
                list = (List<Problem>)problemRepository.Search(7, new List<string> { "1", problemName });
            }
            else if (ProblemSource != "All")
            {
                list = (List<Problem>)problemRepository.Search(8, new List<string> { "1", ProblemSource });
            }
            else
            {
                list = (List<Problem>)problemRepository.Search(1, new List<string> { "1" });
            }

            
            List<ViewProblemModel> model = new List<ViewProblemModel>();
            foreach (var p in list)
            {
                ViewProblemModel item = new ViewProblemModel()
                {
                    ProblemId = p.ProblemId,
                    OnlineJudge = p.ProblemSource,
                    ProblemSourceId = p.problemSourceId,
                    Title = p.problemTitle,
                    rating = p.rating
                };
                var acsubmission = ListMysubmission.FirstOrDefault(s => s.ProblemId == p.ProblemId && s.Verdict == "Accept");
                if (acsubmission != null)
                {
                    item.Status = "Solved";
                }
                else
                {
                    var wrsubmission = ListMysubmission.FirstOrDefault(s => s.ProblemId == p.ProblemId && s.Verdict == "Wrong");
                    if (wrsubmission != null)
                        item.Status = "Attempted";
                    else
                        item.Status = "";
                }
                var Is_Favorite = ListMyfavorite.FirstOrDefault(f => f.IsFavourite == true && f.ProblemId == p.ProblemId);
                if (Is_Favorite != null)
                    item.Favorite = true;
                else
                    item.Favorite = false;

                model.Add(item);

            }
            return View("Index", model);
        }

        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            var problem = problemRepository.Find(id);
            return View(problem);
        }

        // GET: HomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Problem problem)
        {
            try
            {
                problem.problemType = 1;
                problemRepository.Add(problem);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //// GET: HomeController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: HomeController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: HomeController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: HomeController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
