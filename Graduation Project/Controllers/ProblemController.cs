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
        private readonly IRepository<Submission> submissonRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly User user;
        public ProblemController(IUserRepository<User> Userrepository,IProblemRepository<Problem> problemRepository, IRepository<Submission> submissonRepository, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            this.problemRepository = problemRepository;
            this.submissonRepository = submissonRepository;
            user = Userrepository.Find(userId);

        }
        public ActionResult Index()
        {
            var ListMysubmission = user.submissions;
            var ListMyfavorite = user.ProblemUsers;
            var ListProblems = problemRepository.List();
            List<ViewProblemModel> model=null;
            foreach (var p in ListProblems)
            {
                ViewProblemModel item = new ViewProblemModel()
                {
                    OnlineJudge = p.ProblemSource,
                    ProblemSourceId = p.problemSourceId,
                    Title = p.problemTitle,
                    rating = p.rating
                };
                var acsubmission = ListMysubmission.FirstOrDefault(s => s.ProblemId == p.ProblemId && s.Verdict == "Accept");
                if (acsubmission != null)
                {
                    item.Status = "Accept";
                }
                else
                {
                    var wrsubmission = ListMysubmission.FirstOrDefault(s => s.ProblemId == p.ProblemId && s.Verdict == "Accept");
                    if (wrsubmission != null)
                        item.Status = "Attemp";
                    else
                        item.Status = "";
                }
                var Is_Favorite = ListMyfavorite.FirstOrDefault(f=>f.IsFavourite==true && f.ProblemId==p.ProblemId);
                if (Is_Favorite != null)
                    item.Favorite = true;
                else
                    item.Favorite = false;

                model.Add(item);

            }
            
            return View(model);
        }
        public ActionResult Filter()
        {
            var ProblemSource = Request.Form["ProblemSource"];
            var NameProblem = Request.Form["NameProblem"];
            var list=new List<Problem>();
            if (ProblemSource != "" && NameProblem != "")
            {
                list = (List<Problem>)problemRepository.Search(4, new List<string>() { "1", ProblemSource, NameProblem });
            }
            else if (NameProblem != "")
            {
                list = (List<Problem>)problemRepository.Search(3, new List<string>() { "1", NameProblem });
            }
            else if (ProblemSource != "")
            {
                list = (List<Problem>)problemRepository.Search(2, new List<string>() { "1", ProblemSource });
            }
            else
            {
                list = (List<Problem>)problemRepository.Search(1, new List<string>() { "1" });
            }
            return View("Index", list);
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
