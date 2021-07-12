﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using GraduationProject.Data.Repositories.IProblemRepository;
using GraduationProject.ViewModels.ContestViewsModel;
using GraduationProject.ViewModels.ProblemViewsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Controllers.Contest
{
    [Authorize]
    public class ContestController : Controller
    {
        readonly private IContestRepository<GraduationProject.Data.Models.Contest> contests;
        readonly private IProblemRepository<Problem> problems;
        readonly private IGroupRepository<GraduationProject.Data.Models.Group> groups;
        private readonly ISubmissionRepository<Submission> Submissions;
        readonly private User user; 
        readonly private int NumberOfItemsForPage = 12; 
        public ContestController(IContestRepository<GraduationProject.Data.Models.Contest> contests
            , IUserRepository<User> Userrepository
            , IHttpContextAccessor httpContextAccessor
            , IProblemRepository<Problem> problems
            , IGroupRepository<GraduationProject.Data.Models.Group> groups
            , ISubmissionRepository<Submission> Submissions
            )
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            user = Userrepository.Find(userId);
            this.contests = contests;
            this.problems = problems;
            this.groups = groups;
            this.Submissions = Submissions;


        }
        // GET: HomeController
        public ActionResult Index()
        {
            try
            {
                return View(getPageItems(getAllContests(), 1));
            }catch
            {
                return View("ErrorLink");
            }
        }
        private List<ViewContestModel> getAllContests()
        {
            List<ViewContestModel> list = new List<ViewContestModel>();
            foreach (var c in contests.PublicContests())
            {
                if (CanAccessTheContest(c.contestId, user.UserId))
                    list.Add(getContestViewModelFromContest(c));
            }
            return list; 
        }
        // GET: HomeController/Details/5
        public ActionResult Details(int Id)
        {
            try
            {
                if (!CanAccessTheContest(Id, user.UserId))
                    return RedirectToAction("Index");
                var contest = contests.Find(Id);
                var model = getContestViewModelFromContest(contest);
                return View(model);
            }catch
            {
                return View("ErrorLink");
            }
        }

        // GET: HomeController/Create
        public ActionResult Create(int Id)
        {
            try
            {
                var createContestView = CreateContestView();
                if (groups.Find(Id) != null)
                {
                    createContestView.CreateFromGroup = "1";
                    createContestView.groupId = Id;
                }
                return View(createContestView);
            }catch
            {
                return View("Index");
            }
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateContestModel model)
        {
            try
            {
                var contest = CreateContestFromCreateContestModel(model);
                contests.CreateNewContest(user.UserId, contest); 
                if (!contest.InGroup)
                    return RedirectToAction("Index");
                else 
                    return RedirectToAction("Details", "Group", new { id = contest.groupId });
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                if (!contests.IsOwner(id, user.UserId))
                    return RedirectToAction("Details", new { id });
                var contest = contests.Find(id);
                return View(getCreateContestModel(contest));
            }catch
            {
                return View("Index"); 
            }

        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateContestModel model)
        {
            if (!contests.IsOwner(model.contestId, user.UserId))
                return RedirectToAction("Index");
            var contest = CreateContestFromCreateContestModel(model);
            try
            {
                contests.Update(contest);
                if (contest.groupId != null)
                    return RedirectToAction("Details", "Group", new { id = contest.groupId });
                else
                    return RedirectToAction("Index"); 
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                if (!contests.IsOwner(id, user.UserId))
                    return RedirectToAction("Index");
                var contest = contests.Find(id);
                return View(contest);
            }catch
            {
                return View("ErrorLink");
            }


        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(GraduationProject.Data.Models.Contest contest)
        {
            if (!contests.IsOwner(contest.contestId, user.UserId))
                return RedirectToAction("Index");
            try
            {
                int? groupId = contests.Find(contest.contestId).groupId;
                contests.Remove(contest.contestId);
                if (groupId != null)
                    return RedirectToAction("Details", "Group", new { id = groupId });
                else
                    return RedirectToAction("Index"); 
            }
            catch
            {
                return View();
            }
        }
        public ActionResult RegisterInContest(int id)
        {
            try
            {
                contests.RegisterInContest(user.UserId, id);
                return RedirectToAction("Details", new { id });
            }catch
            {
                return View("ErrorLink");
            }
        }

        public ActionResult Standing(int Id)
        {
            try
            {
                if (!CanAccessTheContest(Id, user.UserId))
                    return RedirectToAction("Index");
                return View(CreateStandingView(Id));
            }catch
            {
                return View("ErrorLink");
            }
        }

        public ActionResult FlipFavourite(int id, ContestFilter Filter)
        {
            return FlipFavourite(user.UserId, id, Filter);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FlipFavourite(int userId, int contestId, ContestFilter Filter)
        {
            try 
            { 
                if (!CanAccessTheContest(contestId, user.UserId))
                    return RedirectToAction("Filter", Filter);
            
                contests.FlipFavourite(contestId, userId);
                return RedirectToAction("Filter", Filter);
            }
            catch
            {
                return RedirectToAction("Filter", Filter);
            }
        }
        public ActionResult Filter()
        {
            try
            {
                ContestFilter filter = new ContestFilter { PrepeardBy = (string)TempData["PrepeardBy"] };
                return Filter(filter);
            }catch
            {
                return View("Index");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter(ContestFilter model)
        {
            try
            {
                if (model.Reset == "Reset")
                    return RedirectToAction("Index");
                var list = new List<ViewContestModel>();
                model.userId = user.UserId;
                foreach (var c in contests.Filter(model))
                    list.Add(getContestViewModelFromContest(c));
                return View("Index", getPageItems(list, 1));
            }catch
            {
                return View("Index"); 
            }
        }

        // This is For Problem 
        
        public ActionResult Status(int Id)
        {
            try
            {
                if (!CanAccessTheContest(Id, user.UserId))
                    return RedirectToAction("Index");
                var contest = contests.Find(Id);
                var model = getContestViewModelFromContest(contest);
                model.Submissions = model.Submissions.OrderByDescending(u => u.CreationTime).ToList(); 
                return View("Details", model);
            }
            catch
            {
                return View("ErrorLink");
            }

        }
        
        public ActionResult MySubmission(int Id)
        {
            try
            {
                if (!CanAccessTheContest(Id, user.UserId))
                    return RedirectToAction("Index");
                var contest = contests.Find(Id);
                var model = getContestViewModelFromContest(contest);
                model.Submissions = model.Submissions.Where(u => u.userId == user.UserId).OrderByDescending(u=>u.CreationTime).ToList(); 
                return View("Details", model);
            }
            catch
            {
                return View("ErrorLink");
            }
        }
        
        public ActionResult DisplayProblem(int id, int problemId)
        {
            var contest = contests.Find(id);
            var owner = contest.UserContest.FirstOrDefault(u => u.isOwner == true);
            int? ownerId = owner != null ? owner.UserId : null;
            string alias = contest.ContestProblems.FirstOrDefault(u => u.problemId == problemId).Alias; 
            
            var problem = contest.ContestProblems.FirstOrDefault(u => u.problemId == problemId).problem;
            if (alias == "") alias = problem.problemTitle; 
            var model = GetDetailProblem(problem);
            model.contestDuration = contest.contestDuration;
            model.contestId = contest.contestId;
            model.contestStartTime = contest.contestStartTime;
            model.groupId = contest.groupId;
            model.IsCurrentUserOwner = (user.UserId == ownerId);
            model.contestTitle = contest.contestTitle;
            model.problemtitle = alias; 
            return View("ProblemInContest", model);
        }
        // This Code Copied From Problem Controller vv
        private ViewProblemDetailsInContest GetDetailProblem(Problem problem)
        {

            ViewProblemDetailsInContest model = new ViewProblemDetailsInContest()
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
            
            List<string> tags = new List<string>();

            foreach (var item in problem.ProblemTag)
            {
                tags.Add(item.Tag.tagName);
            }
            model.problemTag = tags;
            return model;
        }
        // This Code Copied From Problem Controller ^^

        // This is For Problem 
        private Boolean CanAccessTheContest(int contestId, int userId)
        {
            try
            {
                var c = contests.Find(contestId);
                var rel = c.UserContest.FirstOrDefault(u => u.UserId == userId);
                return c.contestVisabilty == "Public" || rel != null;
            }catch
            {
                return false; 
            }
        }
        private ViewContestModel getContestViewModelFromContest(Data.Models.Contest contest)
        {
            string contestStatus = "";
            switch (contest.contestStatus)
            {
                case -1:
                    contestStatus = "Upcoming";
                    break;
                case 0:
                    contestStatus = "Running";
                    break;
                case 1:
                    contestStatus = "Ended";
                    break;
            }
            IList<ProblemInfo> Problems = new List<ProblemInfo>();
            foreach (var item in contest.ContestProblems.OrderBy(u=>u.order).ToList())
            {
                string ProblemUrl = item.problem.UrlSource; 
                var newProblem = new ProblemInfo();
                newProblem.ProblemId = item.problemId;
                newProblem.Origin = item.PlatForm;
                newProblem.OriginName = item.ProblemSourceId; 
                newProblem.PropblemTitle = item.Alias;
                newProblem.Solved = contest.Submissions.FirstOrDefault
                    (u => u.userId == user.UserId && 
                    u.ProblemId == item.problemId && 
                    u.Verdict == "Accepted") != null;
                newProblem.NumberOfAccepted = contest.Submissions.Where
                    (u => u.userId == user.UserId &&
                    u.ProblemId == item.problemId &&
                    u.Verdict == "Accepted").Count();
                newProblem.NumberOfSubmissions = contest.Submissions.Where(u=>u.ProblemId == item.problemId).Count();
                newProblem.originUrl = ProblemUrl;
                newProblem.HasAttempt = contest.Submissions.FirstOrDefault
                    (u => u.userId == user.UserId && u.ProblemId == item.problemId) != null;
                Problems.Add(newProblem); 
            }
            var owner = contest.UserContest.FirstOrDefault(u => u.isOwner == true).User;
            var currentUser = contest.UserContest.FirstOrDefault(u => u.UserId == user.UserId); 
            return new ViewContestModel
            {
                contestId = contest.contestId,
                contestDuration = contest.contestDuration,
                contestStartTime = contest.contestStartTime,
                contestStatus = contestStatus,
                contestTitle = contest.contestTitle,
                contestVisabilty = contest.contestVisabilty,
                UserContest = contest.UserContest,
                creationTime = contest.creationTime,
                groupId = contest.groupId,
                Problems = Problems,
                Submissions = contest.Submissions.ToList(),
                IsFavourite = currentUser != null? currentUser.isFavourite: false,
                PreparedBy = owner.UserName, 
                PreparedById = owner.UserId,
                IsCurrentUserOwner = (owner.UserId == user.UserId)
            };
        }
        private StandingViewModel CreateStandingView(int contestId)
        {
            IList<UserInStanding> users = new List<UserInStanding>();
            int PenalityForWrongAnswer = 10;
            var contest = contests.Find(contestId);
            var usersInContest = contest.UserContest.Where(u=>u.isRegistered == true).Select(u=>u.User);
            int NumberOfProblems = contest.ContestProblems.Count();
            int NumberOfUsers = usersInContest.Count();
            var problemsInContest = contest.ContestProblems.ToList().OrderBy(u => u.order);
            var submissions = contest.Submissions.ToList().OrderBy(u => u.CreationTime);
            Boolean FirstAccepted = false;

            foreach (var u in usersInContest)
            {
                var EmptyUserProblemsRaw = new List<GraduationProject.ViewModels.ContestViewsModel.Data>();
                foreach (var p in problemsInContest)
                {
                    EmptyUserProblemsRaw.Add(new ViewModels.ContestViewsModel.Data { problemId = p.problemId, Solved = false });
                }
                users.Add(new UserInStanding { userId = u.UserId, userName = u.UserName, UserPoblemsRaw = EmptyUserProblemsRaw });
            }
            int? ownerId = null; 
            try
            {
                ownerId = contest.UserContest.FirstOrDefault(u => u.isOwner == true).UserId;
            }catch
            {
                ownerId = null; 
            }
            foreach (var submission in submissions)
            {
                
                int currentUserId = submission.userId;
                int currentProblemId = submission.ProblemId;
                var submissionVerdict = submission.Verdict;
                var currentUserIndex = users.IndexOf(users.FirstOrDefault(u => u.userId == currentUserId));
                var currentProblemIndex = users[currentUserIndex].UserPoblemsRaw.
                    IndexOf(users[currentUserIndex].UserPoblemsRaw.
                    FirstOrDefault(u => u.problemId == currentProblemId));
                int currentNumberOfSubmissions = users[currentUserIndex].
                    UserPoblemsRaw[currentProblemIndex].NumberOfSubmissions;
                if (users[currentUserIndex].UserPoblemsRaw[currentProblemIndex].Solved)
                    continue;
                users[currentUserIndex].UserPoblemsRaw[currentProblemIndex].NumberOfSubmissions++;
                users[currentUserIndex].UserPoblemsRaw[currentProblemIndex].Submissions.Add(submission);
                if (submissionVerdict == "Accepted")
                {
                    users[currentUserIndex].NumberOfSolvedProblems++; 
                    users[currentUserIndex].UserPoblemsRaw[currentProblemIndex].problemPenality = 
                        (int)submission.CreationTime.Subtract(contest.contestStartTime).TotalMinutes
                        + currentNumberOfSubmissions * PenalityForWrongAnswer;
                    users[currentUserIndex].UserPoblemsRaw[currentProblemIndex].Solved = true; 
                    if (FirstAccepted == false)
                    {
                        FirstAccepted = true;
                        users[currentUserIndex].UserPoblemsRaw[currentProblemIndex].FirstAcceptedSubmission = true; 
                    }
                }
            }
            NavInfo navinfo = new NavInfo {
                contestDuration = contest.contestDuration,
                contestStartTime = contest.contestStartTime,
                contestTitle = contest.contestTitle,
                groupId = contest.groupId,
                IsCurrentUserOwner = ownerId == user.UserId
            };

            return new StandingViewModel
            {
                contestId = contestId,
                NumberOfProblems = NumberOfProblems,
                NumberOfUsers = NumberOfUsers,
                users = users.OrderBy(u => -u.NumberOfSolvedProblems).ThenBy(u => u.TotalPenality).ToList(),
                NavInfo = navinfo
                
            };
        }
        private CreateContestModel CreateContestView()
        {
            IList<GroupData> myGroups = new List<GroupData>();
            foreach (var g in user.UserGroup.Select(u => u.Group))
                myGroups.Add(new GroupData { groupId = g.GroupId, groupTitle = g.GroupTitle });

            return new CreateContestModel
            {
                groups = myGroups
            };
        }
        private GraduationProject.Data.Models.Contest CreateContestFromCreateContestModel(CreateContestModel model)
        {
            var contestProblems = getContestProblmes(model.problems); 
            return new GraduationProject.Data.Models.Contest
            {
                groupId = model.CreateFromGroup == "0" ? null : model.groupId,
                contestDuration = model.Duration,
                contestStartTime = model.StartTime,
                contestTitle = model.contestTitle,
                contestVisabilty = model.Visable == "1" ? "Public" : "Private",
                ContestProblems = contestProblems, 
                Password = model.Password,
                contestId = model.contestId
            };
        }

        private List<ContestProblem> getContestProblmes(IList<ProblemData> Modelproblems)
        {
            var list = new List<ContestProblem>();
            int order = 0; 
            foreach (var p in Modelproblems)
            {
                Problem current = null;
                try
                {
                    current = problems.FindByName(p.PlatForm, p.problemId);
                }
                catch
                {
                    continue;
                }
                if (current == null) continue;
                list.Add(
                new ContestProblem
                {
                    problemId = current.ProblemId,
                    order = order++,
                    Alias = p.Alias,
                    PlatForm = p.PlatForm,
                    ProblemSourceId = p.problemId
                });
            }
            return list; 
        }

        private CreateContestModel getCreateContestModel(GraduationProject.Data.Models.Contest contest)
        {
            var problems = new List<ProblemData>();
            foreach(var cp in contest.ContestProblems)
            {
                problems.Add(new ProblemData { PlatForm = cp.PlatForm, Alias = cp.Alias, problemId = cp.ProblemSourceId }); 
            }
            return new CreateContestModel
            {
                Visable = contest.contestVisabilty,
                contestTitle = contest.contestTitle,
                Duration = contest.contestDuration,
                groupId = contest.groupId == null? -1: (int)contest.groupId,
                problems = problems, 
                StartTime = contest.contestStartTime,
                contestId = contest.contestId
            };
        }
        private List<ViewContestModel> getPageItems(List<ViewContestModel> list, int PageNumber)
        {
            if (PageNumber < 1 || PageNumber > (list.Count + NumberOfItemsForPage - 1) / NumberOfItemsForPage)
                PageNumber = 1;
            ViewBag.NumberOfPages = (list.Count + NumberOfItemsForPage - 1) / NumberOfItemsForPage;
            ViewBag.PageNumber = PageNumber;
            int startIndex = (PageNumber - 1) * NumberOfItemsForPage;
            int endIndex = PageNumber * NumberOfItemsForPage;
            if (list.Count > endIndex)
                list.RemoveRange(endIndex, list.Count - endIndex);
            if (list.Count > NumberOfItemsForPage)
                list.RemoveRange(0, list.Count - NumberOfItemsForPage);
            return list;
        }
        public ActionResult Page(int PageNumber)
        {
            try
            {
                var list = getPageItems(getAllContests(), PageNumber);
                return View("Index", list);
            }catch
            {
                return View("Index"); 
            }
        }
        public ActionResult SubmitCode(int contestId, int problemId, string code, string lang)
        {
            
            try
            {
                int userId = user.UserId;
                if (!CanAccessTheContest(contestId, userId))
                    return RedirectToAction("MySubmission", new { contestId });
                contests.Submit(userId, contestId, problemId, code, lang);
                return RedirectToAction("MySubmission", new { id = contestId });
            }
            catch
            {
                return RedirectToAction("MySubmission", new { contestId });
            }
            
        }
        
        [HttpPost]
        public ActionResult GetTextSubmission(int SubmisionId)
        {
            var Result = Submissions.Find(SubmisionId).SubmissionText;
            return Content(Result, "text/plain");
        }


    }
}
