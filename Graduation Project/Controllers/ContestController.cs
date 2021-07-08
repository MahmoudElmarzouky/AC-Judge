using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using GraduationProject.Data.Repositories.IProblemRepository;
using GraduationProject.ViewModels.ContestViewsModel;
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
        readonly private User user; 
        public ContestController(IContestRepository<GraduationProject.Data.Models.Contest> contests
            , IUserRepository<User> Userrepository
            , IHttpContextAccessor httpContextAccessor
            , IProblemRepository<Problem> problems
            , IGroupRepository<GraduationProject.Data.Models.Group> groups
            )
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            user = Userrepository.Find(userId);
            this.contests = contests;
            this.problems = problems;
            this.groups = groups; 
            
        }
        // GET: HomeController
        public ActionResult Index()
        {
            return View(getAllContests());
        }
        private IList<ViewContestModel> getAllContests()
        {
            IList<ViewContestModel> list = new List<ViewContestModel>();
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
            if (!CanAccessTheContest(Id, user.UserId))
                return RedirectToAction("Index");  
            var contest = contests.Find(Id);
            var model = getContestViewModelFromContest(contest); 
            return View(model);
        }

        // GET: HomeController/Create
        public ActionResult Create(int Id)
        {
            var createContestView = CreateContestView();
            if (groups.Find(Id) != null)
            {
                createContestView.CreateFromGroup = "1";
                createContestView.groupId = Id;
            }
            return View(createContestView);
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
            if (!CanAccessTheContest(id, user.UserId))
                return RedirectToAction("Index");
            var contest = contests.Find(id); 
            return View(contest);
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GraduationProject.Data.Models.Contest contest)
        {
            if (!CanAccessTheContest(contest.contestId, user.UserId))
                return RedirectToAction("Index");
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
            if (!CanAccessTheContest(id, user.UserId))
                return RedirectToAction("Index");
            var contest = contests.Find(id);
            return View(contest);
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(GraduationProject.Data.Models.Contest contest)
        {
            if (!CanAccessTheContest(contest.contestId, user.UserId))
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
            contests.RegisterInContest(user.UserId, id);
            return RedirectToAction("Details", new { id });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProblemToContest(int contestId, string problemName)
        {
            if (!CanAccessTheContest(contestId, user.UserId))
                return RedirectToAction("Index"); 
            var problem = problems.FindByName("CodeForces", problemName); 
            if (problem == null)
                return RedirectToAction("Details", new { id = contestId });
            try
            {
                contests.AddProblemToContest(problem.ProblemId, contestId); 
                return RedirectToAction("Details", new { id = contestId });
            }
            catch
            {
                return RedirectToAction("Details", new { id = contestId });
            }
        }

        public ActionResult Standing(int Id)
        {
            if (!CanAccessTheContest(Id, user.UserId))
                return RedirectToAction("Index");
            return View(CreateStandingView(Id)); 
        }

        public ActionResult FlipFavourite(int id, ContestFilter Filter)
        {
            return FlipFavourite(user.UserId, id, Filter);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FlipFavourite(int userId, int contestId, ContestFilter Filter)
        {
            if (!CanAccessTheContest(contestId, user.UserId))
                return RedirectToAction("Filter", Filter);
            try
            {
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
            ContestFilter filter = new ContestFilter {PrepeardBy = (string)TempData["PrepeardBy"]};
            return Filter(filter);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter(ContestFilter model)
        {
            if (model.Reset == "Reset")
                return View("Index", getAllContests());
            var list = new List<ViewContestModel>();
            model.userId = user.UserId; 
            foreach (var c in contests.Filter(model))
                list.Add(getContestViewModelFromContest(c));
            return View("Index", list); 
        }
        private Boolean CanAccessTheContest(int contestId, int userId)
        {
            var c = contests.Find(contestId);
            var rel = c.UserContest.FirstOrDefault(u => u.UserId == userId);
            return c.contestVisabilty == "Public" || rel != null; 
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
            ICollection<Problem> Problems = new HashSet<Problem>();
            foreach (var item in contest.ContestProblems.ToList())
                Problems.Add(item.problem);
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
            };
        }
        private StandingViewModel CreateStandingView(int contestId)
        {
            int PenalityForWrongAnswer = 10;
            var contest = contests.Find(contestId);
            var usersInContest = contest.UserContest.Where(u=>u.isRegistered == true).Select(u=>u.User);
            int NumberOfProblems = contest.ContestProblems.Count();
            int NumberOfUsers = usersInContest.Count();
            var problemsInContest = contest.ContestProblems.ToList().OrderBy(u => u.order);
            var EmptyUserProblesRaw = new List<GraduationProject.ViewModels.ContestViewsModel.Data>(); 
            foreach(var p in problemsInContest)
            {
                EmptyUserProblesRaw.Add(new ViewModels.ContestViewsModel.Data { problemId = p.problemId, Solved = false }); 
            }
            IList<UserInStanding> users = new List<UserInStanding>();
            
            foreach (var u in usersInContest)
                users.Add(new UserInStanding { userId = u.UserId, userName = u.UserName, UserPoblemsRaw = EmptyUserProblesRaw });
            var submissions = contest.Submissions.ToList().OrderBy(u=>u.CreationTime);
            Boolean FirstAccepted = false; 
            foreach(var submission in submissions)
            {
                // userId and Problem Id, should be commperessed first to calculate then decompressed 
                int currentUserId = submission.userId;
                int currentProblemId = submission.ProblemId;
                var submissionVerdict = submission.Verdict;
                // check if currentUserIndex or currentProblemIndex == -1 
                var currentUserIndex = users.IndexOf(users.FirstOrDefault(u => u.userId == currentUserId));
                var currentProblemIndex = users[currentUserIndex].UserPoblemsRaw.IndexOf(users[currentUserIndex].UserPoblemsRaw.FirstOrDefault(u => u.problemId == currentProblemId));
                int currentNumberOfSubmissions = users[currentUserIndex].UserPoblemsRaw[currentProblemIndex].NumberOfSubmissions;
                if (users[currentUserIndex].UserPoblemsRaw[currentProblemIndex].Solved)
                    continue;
                users[currentUserIndex].UserPoblemsRaw[currentProblemIndex].NumberOfSubmissions++;
                users[currentUserIndex].UserPoblemsRaw[currentProblemIndex].Submissions.Add(submission);
                users[currentUserIndex].UserPoblemsRaw[currentProblemIndex].Solved = (submissionVerdict == "Accept" ? true : false); 
                if (submissionVerdict == "Accepted")
                {
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
            return new StandingViewModel
            {
                contestId = contestId,
                NumberOfProblems = NumberOfProblems, 
                NumberOfUsers = NumberOfUsers, 
                users = users
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
            var Addedproblems = new List<ContestProblem>(); 
            foreach(var p in model.problems)
            {
                var current = problems.FindByName(p.PlatForm, p.problemId);
                if (current == null) continue;
                Addedproblems.Add(new ContestProblem { problemId = current.ProblemId, order = Addedproblems.Count }); 
            }
            return new GraduationProject.Data.Models.Contest
            {
                groupId = model.CreateFromGroup == "0" ? null : model.groupId,
                contestDuration = model.Duration,
                contestStartTime = model.StartTime,
                contestTitle = model.contestTitle,
                InGroup = model.CreateFromGroup == "0" ? false : true,
                contestVisabilty = model.Visable == "1" ? "Public" : "Private",
                ContestProblems = Addedproblems
                // Password 
                // problems 
            };
        }



    }
}
