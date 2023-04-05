using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ACJudge.Data.API;
using ACJudge.Data.Models;
using ACJudge.Data.Repositories.Interfaces;
using ACJudge.ExtensionMethods;
using ACJudge.ViewModels.ProblemViewsModel;
using X.PagedList;

namespace ACJudge.Controllers
{
    public class ProblemController : Controller
    {
        private readonly IProblemRepository<Problem> _problemRepository;
        private readonly ISubmissionRepository<Submission> _submissionRepository;
        private readonly User _user;
        private readonly bool _login;
        private readonly IEnumerable<Submission> _listMySubmission;
        private readonly IEnumerable<ProblemUser> _listMyFavorite;
        private const int PageSize = 25;
        public ProblemController(ISubmissionRepository<Submission> submissionRepository,
            IUserRepository<User> userRepository,
            IProblemRepository<Problem> problemRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _submissionRepository = submissionRepository;
            _problemRepository = problemRepository;
            var isAuthenticated = httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated;
            if (isAuthenticated is true)
            {
                _login = true;
                var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)
                    ?.Value;
                _user = userRepository.Find(userId);
                _listMySubmission = _user.Submissions;
                _listMyFavorite = _user.UserProblems;
            }
            else
            {
                _login = false;
            }
        }

        public ActionResult Index(int? page)
        {
            var problemFilter = new ProblemFilter
            {
                Type = ProblemFilter.ProblemType.Practice
            };
            page ??= 1;
            var allProblems = _problemRepository.Search(problemFilter);
            var totalPages = (int)Math.Ceiling((decimal)allProblems.Count / PageSize);
            if (page < 1 || page > totalPages) page = 1;

            var list = allProblems.Paginate((int)page, PageSize).
                Select(GetProblemModelView);
            var problemPage = new ProblemPageView<ViewProblemModel, ProblemFilter>(list, (int)page, totalPages, new ProblemFilter(), _login);
            return View(problemPage);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Submit(int problemId, string problemSourceId, string language)
        {
            var submitText = Request.Form["SubmitText"];

            var submission = new Submission
            {
                MemoryConsumeBytes = "",
                TimeConsumeMillis = "",
                Visible = false,
                CreationTime = DateTime.Now,
                Verdict = "Inqueue",
                ProgrammingLanguage = language,
                UserId = _user.UserId,
                ProblemId = problemId,
                SubmissionText = submitText
            };
            submission = _submissionRepository.Add(submission);
            APi.GetVerdict(problemSourceId, submitText, language, submission.SubmissionId);
            return RedirectToAction("Details", new { id = problemId });
        }

        public bool CanSeeSubmission(int submissionId)
        {
            var submission = _submissionRepository.Find(submissionId);
            return submission.Visible || (_login && submission.UserId == _user.UserId);
        }

        public ActionResult Status(int? page, ProblemPageView<ViewStatusModel, StatusFilter> model = null)
        {
            var filter = model?.Filter ?? new StatusFilter();
            var submissions = _submissionRepository.GetSpecificSubmission(filter).OrderByDescending(s => s.SubmissionId).
                Select(GetViewStatusModel).ToList();

            var totalPages = (int)Math.Ceiling((decimal)submissions.Count / PageSize);
            if (page < 1 || page > totalPages) page = 1;
            
            var problemPage = new ProblemPageView<ViewStatusModel, StatusFilter>(submissions, (int)page, totalPages, new StatusFilter(), _login);
            
            return View(problemPage);
            
        }

        [HttpPost]
        public ActionResult GetTextSubmission(int submissionId)
        {
            var result = _submissionRepository.Find(submissionId).SubmissionText;
            return Content(CanSeeSubmission(submissionId) ? result : "", "text/plain");
        }

        public ActionResult Filter(int page, ProblemPageView<ViewProblemModel, ProblemFilter> model = null)
        {
            
            var problemFilter = model?.Filter ?? new ProblemFilter();
            var allProblems = _problemRepository.Search(problemFilter);
            
            var totalPages = (int)Math.Ceiling((decimal)allProblems.Count / PageSize);
            if (page < 1 || page > totalPages) page = 1;

            var list = allProblems.Paginate(page, PageSize).
                Select(GetProblemModelView);
            var problemPage = new ProblemPageView<ViewProblemModel, ProblemFilter>(list, page, totalPages, problemFilter, _login);
            return View("Index", problemPage);
        }

        public ActionResult FilterStatus(int? page, ProblemPageView<ViewStatusModel, StatusFilter> model = null)
        {
            var filter = model?.Filter ?? new StatusFilter();

            // TODO Pass StatusFilter to GetSpecificSubmission method 
            var submissions = _submissionRepository.GetSpecificSubmission(filter).
                OrderByDescending(s => s.SubmissionId)
                .Select(GetViewStatusModel).ToList();
            var totalPages = (int)Math.Ceiling((decimal)submissions.Count / PageSize);
            
            if (page < 1 || page > totalPages) page = 1;
            
            var problemPage = new ProblemPageView<ViewStatusModel, StatusFilter>(submissions, (int)page, totalPages, filter, _login);
            
            return View("Status", problemPage);
        }

        public ActionResult FlipFavourite(int id)
        {
            if (!_login) return View("ErrorLink");
            var p = _problemRepository.Find(id);
            if (p == null)
            {
                return View("ErrorLink");
            }

            _flipFavorite(p);
            return RedirectToAction(nameof(Index));
        }
        
        public ViewProblemModel GetProblemModelView(Problem problem)
        {
            var problemViewModel = new ViewProblemModel()
            {
                ProblemId = problem.ProblemId,
                OnlineJudge = problem.ProblemSource,
                ProblemSourceId = problem.ProblemSourceId,
                Title = problem.ProblemTitle,
                Rating = problem.Rating,
                UrlSource = problem.UrlSource
            };
            
            
            if (_login)
            {
                var isSolved = _listMySubmission.FirstOrDefault(s => s.ProblemId == problem.ProblemId
                                                                     && s.Verdict == "Accepted") != null;
                var isWrong = (!isSolved && _listMySubmission.FirstOrDefault(s => s.ProblemId == problem.ProblemId
                    && s.Verdict == "Wrong") != null);
                
                var isFavorite = _listMyFavorite.FirstOrDefault(f => f.IsFavourite && f.ProblemId == problem.ProblemId) != null;
                
                problemViewModel.Status = (isSolved ? "Solved" : isWrong ? "Attempted" : "");
                problemViewModel.Favorite = isFavorite;
            }
            else
            {
                problemViewModel.Favorite = false;
                problemViewModel.Status = "";
            }

            return problemViewModel;
        }
        
        private ViewStatusModel GetViewStatusModel(Submission item)
        {
            var temp = new ViewStatusModel
            {
                RunID = item.SubmissionId,
                UserId = item.User.UserId,
                UserName = item.User.FirstName,
                ProblemId = item.Problem.ProblemId,
                OnlineJudge = item.Problem.ProblemSource,
                ProblemSourcesId = item.Problem.ProblemSourceId,
                Verdict = item.Verdict,
                TimeConsumed = item.TimeConsumeMillis,
                MemoryConsumed = item.MemoryConsumeBytes,
                Language = item.ProgrammingLanguage,
                SubmitTime = item.CreationTime,
                contestId = item.ContestId
            };
            if (item.Visible || (_login && item.User.UserId == _user.UserId))
                temp.Visiable = true;
            else
                item.Visible = false;
            return temp;
        }

        public ActionResult FlipFavouriteDetails(int id)
        {
            if (!_login) return View("ErrorLink");
            var problem = _problemRepository.Find(id);
            if (problem == null)
            {
                return View("ErrorLink");
            }

            _flipFavorite(problem);
            return RedirectToAction("Details", new { id });
        }

        public ActionResult Details(int id)
        {
            var problem = _problemRepository.Find(id);
            if (problem == null)
            {
                return View("ErrorLink");
            }

            var model = GetDetailProblem(problem);
            return View(model);
        }

        private ViewProblemDetails GetDetailProblem(Problem problem)
        {
            // TODO Fix Details 
            var model = new ViewProblemDetails()
            {
                problemId = problem.ProblemId,
                problemSource = problem.ProblemSource,
                problemsourceId = problem.ProblemSourceId,
                urlSource = problem.UrlSource,
                problemtitle = problem.ProblemTitle,
                Problemhtml = problem.ProblemInHtmlForm,
                Rating = problem.Rating,
                NumberAc = problem.Submissions.Count(p => p.Verdict == "Accepted"),
                Numbersubmission = problem.Submissions.Count,
                userName = _user.UserName
            };
            model.IsFavorite = _login &&
                                _listMyFavorite.FirstOrDefault(f =>
                                    f.IsFavourite && f.ProblemId == problem.ProblemId) != null;
            
            model.problemTag = problem.ProblemTag.Select(problemTag => problemTag.Tag.TagName).ToList();
            return model;
        }

        private void _flipFavorite(Problem newProblem)
        {
            var pu = new ProblemUser();
            var problemUser = newProblem.ProblemUsers.FirstOrDefault(u => u.UserId == _user.UserId);
            if (problemUser == null)
            {
                pu.ProblemId = newProblem.ProblemId;
                pu.UserId = _user.UserId;
                pu.IsFavourite = true;
                newProblem.ProblemUsers.Add(pu);
            }
            else
            {
                problemUser.IsFavourite ^= true;
            }

            _problemRepository.Update(newProblem);
        }
    }
}