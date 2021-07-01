using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using GraduationProject.Data.Repositories.IProblemRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Controllers.Interview
{
    public class InterviewController : Controller
    {
        private readonly IProblemRepository<Problem> problemRepository;
        public InterviewController(IProblemRepository<Problem> problemRepository)
        {
            this.problemRepository = problemRepository;
        }
        public ActionResult Index()
        {
            var list = problemRepository.Search(1, new List<string>() { "2" });
            return View(list);
        }
        public ActionResult Filter()
        {
            var ProblemSource = Request.Form["ProblemSource"];
            var NameProblem = Request.Form["NameProblem"];
            var list = new List<Problem>();
            if (ProblemSource != "" && NameProblem != "")
            {
                list = (List<Problem>)problemRepository.Search(4, new List<string>() { "2", ProblemSource, NameProblem });
            }
            else if (NameProblem != "")
            {
                list = (List<Problem>)problemRepository.Search(3, new List<string>() { "2", NameProblem });
            }
            else if (ProblemSource != "")
            {
                list = (List<Problem>)problemRepository.Search(2, new List<string>() { "2", ProblemSource });
            }
            else
            {
                list = (List<Problem>)problemRepository.Search(1, new List<string>() { "2" });

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
                problem.problemType = 2;
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
