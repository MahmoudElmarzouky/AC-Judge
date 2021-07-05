using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using GraduationProject.Data.Repositories.IProblemRepository;
using GraduationProject.ViewModels.ProblemViewsModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace GraduationProject.Controllers.problems
{
    public class ProblemController : Controller
    {
        private readonly IProblemRepository<Problem> problemRepository;
        private readonly ISubmissionRepository<Submission> SubmissionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly User user;
        private readonly Boolean login;
        private readonly IEnumerable<Submission> ListMysubmission;
        private readonly IEnumerable<ProblemUser> ListMyfavorite;
        public ProblemController(ISubmissionRepository<Submission> _SubmissionRepository, IUserRepository<User> Userrepository, IProblemRepository<Problem> problemRepository, IHttpContextAccessor httpContextAccessor)
        {
            SubmissionRepository = _SubmissionRepository;
            this.problemRepository = problemRepository;
            _httpContextAccessor = httpContextAccessor;
            var flag = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
            if (flag == true)
            {
                login = true;
                var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
                user = Userrepository.Find(userId);
                ListMysubmission = user.submissions;
                ListMyfavorite = user.ProblemUsers;
            }
            else
            {
                login = false;
            }

        }
        public ActionResult Index()
        {
            IList<Problem> ListProblems = problemRepository.Search(1, new List<string> { "1" });
            var model = getAllmodel(ListProblems);
            return View(model);
        }
        public Boolean CanSeeSubmission(int SubmissionId)
        {
            var submssion = SubmissionRepository.Find(SubmissionId);
            if (submssion.Visable == true)
                return true;
            if (login && submssion.userId == user.UserId)
                return true;
            return false;
        }
        public ActionResult Status()
        {
            var submissions = SubmissionRepository.GetSubmissionSpecific(false);
            var list = GetAllStatus(submissions);
            return View(list);
        }
        public ActionResult Filter()
        {
            var problemID = Request.Form["problemID"];
            var problemName = Request.Form["problemName"];
            var ProblemSource = Request.Form["ProblemSource"];
            var list = problemRepository.Search(2, new List<string> { "1" , problemID,problemName,ProblemSource});
            var model = getAllmodel(list);
            return View("Index", model);
        }
        public ActionResult FilterStatus()
        {
            var User_Name = Request.Form["UserName"];
            var ProblemName = Request.Form["ProblemName"];
            var ProblemSource = Request.Form["ProblemSource"]=="All"?"":(string)Request.Form["ProblemSource"];
            var ProblemResult = Request.Form["ProblemResult"]=="All"?"":(string)Request.Form["ProblemResult"];
            var ProblemLang = Request.Form["ProblemLang"]=="All"?"":(string)Request.Form["ProblemLang"];
            var submissions = SubmissionRepository.GetSubmissionSpecific(false);
            IEnumerable<ViewStatusModel> list = GetAllStatus(submissions);
            IEnumerable<ViewStatusModel> model = list.Where(
                s => 
                s.UserName.Contains(User_Name)  && 
                s.ProblemSourcesId.Contains(ProblemName) &&
                s.OnlineJudge.Contains(ProblemSource) &&
                s.Verdict.Contains(ProblemResult) &&
                s.Language.Contains(ProblemLang)
                );
            return View("Status", model);
        }
        public ActionResult FlipFavourite(int id)
        {
            var newproblem = problemRepository.Find(id);
            ProblemUser pu = new ProblemUser();
            var problemuser = newproblem.ProblemUsers.FirstOrDefault(u => u.UserId == user.UserId);
            if (problemuser == null)
            {
                pu.ProblemId = id;
                pu.UserId = user.UserId;
                pu.IsFavourite = true;
                newproblem.ProblemUsers.Add(pu);
            }
            else
            {
                problemuser.IsFavourite ^= true;
            }

            problemRepository.Update(newproblem);

            return RedirectToAction(nameof(Index));
        }

        public List<ViewProblemModel> getAllmodel(IList<Problem> ListProblems)
        {
            List<ViewProblemModel> model = new List<ViewProblemModel>();
            foreach (var p in ListProblems)
            {
                ViewProblemModel item = new ViewProblemModel()
                {
                    ProblemId = p.ProblemId,
                    OnlineJudge = p.ProblemSource,
                    ProblemSourceId = p.problemSourceId,
                    Title = p.problemTitle,
                    rating = p.rating
                };
                if (login)
                {
                    var acsubmission = ListMysubmission.FirstOrDefault(s => s.ProblemId == p.ProblemId && s.Verdict == "Accepted");
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
                }
                else
                {
                    item.Favorite = false;
                    item.Status = "";
                }

                model.Add(item);

            }
            return model;
        }
        public IList<ViewStatusModel> GetAllStatus(IList<Submission> submissions)
        {
            IList<ViewStatusModel> list = new List<ViewStatusModel>();
            foreach (var item in submissions)
            {
                var tmp = new ViewStatusModel
                {
                    RunID = item.SubmissionId,
                    UserId = item.user.UserId,
                    UserName = item.user.FirstName,
                    ProblemId = item.problem.ProblemId,
                    OnlineJudge = item.problem.ProblemSource,
                    ProblemSourcesId = item.problem.problemSourceId,
                    Verdict = item.Verdict,
                    TimeConsumed = item.TimeConsumeMillis,
                    MemoryConsumed = item.MemoryConsumeBytes,
                    Language = item.ProgrammingLanguage,
                    SubmitTime = item.CreationTime
                };
                if (item.Visable == true || (login && item.user.UserId == user.UserId)) tmp.Visiable = true;
                else item.Visable = false;
                list.Add(tmp);
            }
            return list;
        }
        public ActionResult Details(int id)
        {
            var problem = problemRepository.Find(id);
            return View(problem);
        }


        public ActionResult Create()
        {
            return View();
        }


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
