using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.IProblemRepository;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using GraduationProject.Data.Repositories.Interfaces;

namespace GraduationProject.Controllers.problems
{
    public class ProblemController : Controller
    {
        private readonly IProblemRepository<Problem> problemRepository;
        private readonly IRepository<Submission> submissonRepository;
        private readonly IRepository<User> UserRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProblemController(IRepository<User> UserRepository,IProblemRepository<Problem> problemRepository, IRepository<Submission> submissonRepository, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            this.problemRepository = problemRepository;
            this.submissonRepository = submissonRepository;
            this.UserRepository = UserRepository;
        }
        public ActionResult Index()
        {
            var list = problemRepository.Search(1, new List<string>() { "1" });
            return View(list);
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
        public ActionResult MySolvedProblem()
        {
            var list = sumbissopnRepository.List();
            return View("Index");
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
