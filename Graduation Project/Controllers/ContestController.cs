using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using GraduationProject.Data.Repositories.IProblemRepository;
using GraduationProject.ViewModels.ContestViewsModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Controllers.Contest
{
    public class ContestController : Controller
    {
        readonly private IContestRepository<GraduationProject.Data.Models.Contest> contests;
        readonly private IProblemRepository<Problem> problems; 
        readonly private User user; 
        public ContestController(IContestRepository<GraduationProject.Data.Models.Contest> contests
            , IUserRepository<User> Userrepository
            , IHttpContextAccessor httpContextAccessor
            , IProblemRepository<Problem> problems)
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            user = Userrepository.Find(userId);
            this.contests = contests;
            this.problems = problems; 
        }
        // GET: HomeController
        public ActionResult Index()
        {
            var list = contests.List(); 
            return View(list);
        }

        // GET: HomeController/Details/5
        public ActionResult Details(int Id)
        {
            var contest = contests.Find(Id);
            var model = getContestViewModelFromContest(contest); 
            return View(model);
        }

        private ViewContestModel getContestViewModelFromContest(Data.Models.Contest contest)
        {
            string contestStatus = "";
            switch(contest.contestStatus)
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
            foreach (var item in contest.ContestProblems.Where(c => c.contestId == contest.contestId).ToList())
                Problems.Add(item.problem); 
            return new ViewContestModel { contestId = contest.contestId, contestDuration = contest.contestDuration, contestStartTime = contest.contestStartTime, contestStatus = contestStatus, contestTitle = contest.contestTitle, contestVisabilty = contest.contestVisabilty, UserContest = contest.UserContest, creationTime = contest.creationTime, groupId = contest.groupId, Problems = Problems, Submissions = contest.Submissions.Where(c=>c.contestId == contest.contestId).ToList(), group = contest.group};
        }

        // GET: HomeController/Create
        public ActionResult Create(int Id)
        {
            ViewBag.ID = Id; 
            return View();
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GraduationProject.Data.Models.Contest contest)
        {
            try
            {
                contest.creationTime = DateTime.Now; 
                contests.Add(contest);
                var userContest = new UserContest { UserId = user.UserId, ContestId = contest.contestId, isFavourite = false, isOwner = true, isRegistered = true };
                contest.UserContest.Add(userContest);
                contests.Update(contest); 
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
            var contest = contests.Find(id); 

            return View(contest);
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GraduationProject.Data.Models.Contest contest)
        {
            try
            {
                contests.Update(contest);
                return RedirectToAction("Details", "Group", new { id = contest.groupId });
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            var contest = contests.Find(id);
            return View(contest);
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(GraduationProject.Data.Models.Contest contest)
        {
            try
            {
                int groupId = contest.groupId;
                contests.Remove(contest.contestId); 
                return RedirectToAction("Details", "Group", new { id = groupId });
            }
            catch
            {
                return View();
            }
        }
        public ActionResult RegisterInContest(int id)
        {
            var contest = contests.Find(id);
            return RegisterInContest(contest); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterInContest(GraduationProject.Data.Models.Contest contest)
        {
            try
            {
                int contestId = contest.contestId; 
                var currentContest = contests.Find(contestId);
                var userContest = currentContest.UserContest.FirstOrDefault(u => u.ContestId == contestId && u.UserId == user.UserId); 
                if (userContest == null)
                {
                    userContest = new UserContest { ContestId = contestId, UserId = user.UserId, isFavourite = false, isOwner = false, isRegistered = true };
                    currentContest.UserContest.Add(userContest); 
                }else
                {
                    userContest.isRegistered = true;
                }
                contests.Update(currentContest); 
                return RedirectToAction("Details", new { id = contestId });
            }
            catch
            {
                return RedirectToAction("Details", new { id = contest.contestId });
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProblemToContest(int contestId, string problemName)
        {
            var problem = problems.FindByName(problemName); 
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
    }
}
