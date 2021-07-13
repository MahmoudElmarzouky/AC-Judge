﻿using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using GraduationProject.Data.Repositories.IProblemRepository;
using GraduationProject.ViewModels.ProblemViewsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using X.PagedList;

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
        public ActionResult Index(int? page)
        {
            ViewBag.function = "Index";
            int pagenumber = page ?? 1;
            IList<Problem> ListProblems = problemRepository.Search(1, new List<string> { "1" });
            var model = getAllmodel(ListProblems);
            ViewBag.TotalPageProblem = (model.Count() / 25) + (model.Count() % 25 == 0 ? 0 : 1);
            if (pagenumber < 0 || pagenumber > ViewBag.TotalPageProblem) pagenumber = 1;
            ViewBag.Pagenum = pagenumber;
            var list = model.ToPagedList(pagenumber, 25);
            return View(list);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Submit(int ProblemId, String problemSourceId,string Language )
        {

            var SubmitText=Request.Form["SubmitText"];

            var result = APi.GetVerdict(problemSourceId, SubmitText, Language);
            Submission submission = new Submission
            {
                MemoryConsumeBytes=1,
                TimeConsumeMillis=1,
                Visable=false,
                CreationTime=DateTime.Now,
                Verdict="ac",
                ProgrammingLanguage=Language,
                userId=user.UserId,
                ProblemId=ProblemId,
                SubmissionText=SubmitText
            };
            SubmissionRepository.Add(submission);
            return RedirectToAction("Details", new { id = ProblemId });
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
        public ActionResult Status(int? page)
        {
            int pagenum = page ?? 1;
            ViewBag.function = "Status";
            var submissions = SubmissionRepository.GetSubmissionSpecific(1, "", "", "", "", "", null).OrderByDescending(s => s.SubmissionId) ;
            var list = GetAllStatus(submissions);
            ViewBag.TotalPageProblem = (list.Count() / 25) + (list.Count() % 25 == 0 ? 0 : 1);
            if (pagenum < 0 || pagenum > ViewBag.TotalPageProblem) pagenum = 1;
            ViewBag.Pagenum = pagenum;
            var newlist = list.ToPagedList(pagenum, 25);
            return View(newlist);
        }
        [HttpPost]
        public ActionResult GetTextSubmission(int SubmisionId)
        {
            var Result = SubmissionRepository.Find(SubmisionId).SubmissionText;
            return Content(Result, "text/plain");
        }
        public ActionResult Filter(int? page, string problemID, string problemName, string ProblemSource)
        {
            int pagenumber = page ?? 1;
            ViewBag.problemid = problemID;
            ViewBag.problemname = problemName;
            ViewBag.Problemsource = ProblemSource;
            ViewBag.function = "Filter";
            var ListProblems = problemRepository.Search(2, new List<string> { "1" , problemID, problemName, ProblemSource });
            var model = getAllmodel(ListProblems);
            ViewBag.TotalPageProblem = (model.Count() / 25) + (model.Count() % 25 == 0 ? 0 : 1);
            if (pagenumber < 0 || pagenumber > ViewBag.TotalPageProblem) pagenumber = 1;
            ViewBag.Pagenum = pagenumber;
            var list = model.ToPagedList(pagenumber, 25);
            return View("Index", list);
        }
        public ActionResult FilterStatus(int? page, string UserName, string ProblemName, string ProblemSource, string ProblemResult, string ProblemLang,int? ContestId=null)
        {
            int pagenum = page ?? 1;
            ViewBag.function = "FilterStatus";
            UserName = (UserName == null ? "" : UserName);
            ProblemName = (ProblemName == null ? "" : ProblemName);
            ProblemSource = ((ProblemSource == null || ProblemSource == "All") ? "" : ProblemSource);
            ProblemResult = ((ProblemResult == null || ProblemResult == "All") ? "" : ProblemResult);
            ProblemLang = ((ProblemLang == null || ProblemLang == "All") ? "" : ProblemLang);
            ViewBag.username = UserName;
            ViewBag.problemName = ProblemName;
            ViewBag.problemSource = ProblemSource;
            ViewBag.problemResult = ProblemResult;
            ViewBag.problemLang = ProblemLang;

            var submissions = SubmissionRepository.GetSubmissionSpecific(1, UserName, ProblemName, ProblemSource, ProblemResult, ProblemLang, ContestId ).OrderByDescending(s => s.SubmissionId);
            IEnumerable<ViewStatusModel> list = GetAllStatus(submissions);
            ViewBag.TotalPageProblem = (list.Count() / 25) + (list.Count() % 25 == 0 ? 0 : 1);
            if (pagenum < 0 || pagenum > ViewBag.TotalPageProblem) pagenum = 1;
            ViewBag.Pagenum = pagenum;
            var model = list.ToPagedList(pagenum, 25);
            return View("Status", model);
        }

        public ActionResult FlipFavourite(int id)
        {
            var p = problemRepository.Find(id);
            if (p == null)
            {
                return View("~/Views/Shared/ErrorLink.cshtml");
            }
            oppFavorite(p);
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
                    rating = p.rating,
                    UrlSource = p.UrlSource
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
        public IList<ViewStatusModel> GetAllStatus(IEnumerable<Submission> submissions)
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
                    SubmitTime = item.CreationTime,
                    contestId=item.contestId
                };
                if (item.Visable == true || (login && item.user.UserId == user.UserId)) tmp.Visiable = true;
                else item.Visable = false;
                list.Add(tmp);
            }
            return list;
        }
        public ActionResult FlipFavouriteDetails(int id)
        {
            if (login)
            {
                var problem = problemRepository.Find(id);
                if (problem == null)
                {
                    return View("~/Views/Shared/ErrorLink.cshtml");
                }
                oppFavorite(problem);
                return RedirectToAction("Details", new { id = id });
            }
            return View("~/Views/Shared/ErrorLink.cshtml");
        }
        public ActionResult Details(int id)
        {
            var problem = problemRepository.Find(id);
            if (problem == null)
            {
                return View("~/Views/Shared/ErrorLink.cshtml");
            }
            if (login)
                ViewBag.User = user;
            var model = GetDetailProblem(problem);
            return View(model);
        }
        private ViewProblemDetails GetDetailProblem(Problem problem)
        {
            
            ViewProblemDetails model = new ViewProblemDetails()
            {
                problemId = problem.ProblemId,
                problemSource = problem.ProblemSource,
                problemsourceId = problem.problemSourceId,
                urlSource = problem.UrlSource,
                problemtitle = problem.problemTitle,
                Problemhtml = problem.ProblemHtml,
                Rating = problem.rating,
                NumberAc = problem.Submissions.Where(p => p.Verdict == "Accepted").Count(),
                Numbersubmission = problem.Submissions.Count()
            };
            if (login)
            {
                var Is_Favorite = ListMyfavorite.FirstOrDefault(f => f.IsFavourite == true && f.ProblemId == problem.ProblemId);
                if (Is_Favorite != null)
                    model.IsFavorite = true;
                else
                    model.IsFavorite = false;
            }
            List<string> tags = new List<string>();

            foreach (var item in problem.ProblemTag)
            {
                tags.Add(item.Tag.tagName);
            }
            model.problemTag = tags;
            return model;
        }
        private void oppFavorite(Problem newproblem)
        {
            ProblemUser pu = new ProblemUser();
            var problemuser = newproblem.ProblemUsers.FirstOrDefault(u => u.UserId == user.UserId);
            if (problemuser == null)
            {
                pu.ProblemId = newproblem.ProblemId;
                pu.UserId = user.UserId;
                pu.IsFavourite = true;
                newproblem.ProblemUsers.Add(pu);
            }
            else
            {
                problemuser.IsFavourite ^= true;
            }

            problemRepository.Update(newproblem);
        }
    }
}
