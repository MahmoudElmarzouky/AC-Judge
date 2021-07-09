using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using GraduationProject.ViewModels.ContestViewsModel;
using GraduationProject.ViewModels.ProblemViewsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraduationProject.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserRepository<User> users;
        private readonly ISubmissionRepository<Submission> SubmissionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Boolean login;
        private User user;
        public UserController(ISubmissionRepository<Submission> _SubmissionRepository, IUserRepository<User> users, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            this.users = users;
            var flag = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
            if (flag == true)
            {
                SubmissionRepository = _SubmissionRepository;
                login = true;
                _httpContextAccessor = httpContextAccessor;
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                user = users.Find(userId);
            }
            else
            {
                login = false;
            }

        }

        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }

        [Route("User/Details/{id}")]
        public ActionResult Details(int id)
        {
            var currentuser = users.Find(id);
            if (login == true)
                TempData["userIdentity"] = user.UserIdentityId;
            return View(currentuser);
        }
        [Authorize]
        [Route("User/MyProfile")]
        public ActionResult Details()
        {
            var currentuser = users.Find(user.UserId);
            TempData["userIdentity"] = user.UserIdentityId;
            return View(currentuser);
        }
        public ActionResult OpenContest(int id)
        {
            var currentuser = users.Find(id);
            TempData["PrepeardBy"] = currentuser.UserName;
            return RedirectToAction("filter", "Contest");
        }
        public ActionResult OpenBlog()
        {
            TempData["BlogsByUser"] = "UserBlogs";
            return RedirectToAction("Index", "Blog");
        }
        [Authorize]
        public ActionResult MySubmission(int id)
        {
            ViewBag.USER = user;
            IList<ViewStatusModel> list = GetAllSubmission(id);
            return View(list);
        }
        public void FlibShare(int SubmisionId)
        {
            Submission submission = SubmissionRepository.Find(SubmisionId);
            submission.Visable ^= true;
            SubmissionRepository.Update(submission);
        }
        public IList<ViewStatusModel> GetAllSubmission(int id)
        {
            var allSubmission = SubmissionRepository.FindSubmissionUser(id);
            IList<ViewStatusModel> list = new List<ViewStatusModel>();
            foreach (var item in allSubmission)
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
                if (item.Visable == true) tmp.Visiable = true;
                else item.Visable = false;
                list.Add(tmp);
            }
            return list;
        }
        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
