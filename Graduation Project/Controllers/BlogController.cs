using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProject.Data.Repositories.Interfaces;
using GraduationProject.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GraduationProject.ViewModels;

namespace GraduationProject.Controllers.Blog
{
    [Authorize]
    public class BlogController : Controller
    {
        private readonly IRepository<Data.Models.Blog> blogs;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private User user;
        public BlogController(IRepository<GraduationProject.Data.Models.Blog> blogs
            , IUserRepository<User> Userrepository
            , IHttpContextAccessor httpContextAccessor
            )
        {
            this.blogs = blogs;

            _httpContextAccessor = httpContextAccessor;
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            user = Userrepository.Find(userId);

        }
        // GET: HomeController
        public ActionResult Index()
        {
            var list = new List<ViewBlogModel>();
            foreach (var item in blogs.List())
                list.Add(getViewModelFromBlog(item));

            return View(list);
        }
        private ViewBlogModel getViewModelFromBlog(GraduationProject.Data.Models.Blog blog)
        {
            var userBlog = blog.userBlog.FirstOrDefault(b => b.blogId == blog.blogId);
            bool IsOwner = false;
            if (userBlog.User.UserIdentityId == user.UserIdentityId) {
                IsOwner = true;
            }
            var model = new ViewBlogModel
            {
                blogId = blog.blogId,
                blogtitle = blog.blogtitle,
                blogOwner = userBlog.User.UserName,
                blogcontent = blog.blogcontent,
                blogvote = blog.blogvote
                , creationTime = blog.creationTime
                , Comments = blog.Comments
                , isOwner = IsOwner
            };
            return model;
        }


        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            var blog = blogs.Find(id);
            return View(getViewModelFromBlog(blog));
        }

        public ActionResult Comments (int id)
        {
            TempData["mydata"] = id;
            return RedirectToAction("Create", "Comment");
        }

        // GET: HomeController/Create
        public ActionResult Create(int id)
        {
            TempData["GroupID"] = id;
            return View();
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GraduationProject.Data.Models.Blog model)
        {
            try
            {
                int GrpupID = (int)TempData["GroupID"];
                var newBlog = new GraduationProject.Data.Models.Blog
                {
                    blogtitle = model.blogtitle,
                    blogcontent = model.blogcontent,
                    groupId = GrpupID,
                    blogVisabilty = false,
                    blogvote = 0,
                    creationTime = DateTime.Now
                };
                blogs.Add(newBlog);
                int userId = user.UserId;
                int blogId = newBlog.blogId;
                var userBlog= CreateRelation(userId, blogId);
                newBlog.userBlog.Add(userBlog);
                blogs.Update(newBlog);
                if(GrpupID==3)
                return RedirectToAction(nameof(Index));
                else
                return RedirectToAction("Details", "Group", new { id = GrpupID });
            }
            catch
            {
                return View();
            }
        }
        private UserBlog CreateRelation(int userId, int blogId)
        {
            var usergroup = new UserBlog { userId = userId,
                blogId = blogId,
                blogOwenr = true,
                isFavourite = false,
                VoteValue=0
            ,User=user};
            return usergroup;
        }
        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            var blog = blogs.Find(id);
            return View(blog);
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, GraduationProject.Data.Models.Blog model)
        {
            try
            {
                
                var newBlog = new GraduationProject.Data.Models.Blog
                {

                    blogId = model.blogId,
                    blogtitle = model.blogtitle,
                    blogcontent = model.blogcontent,
                    groupId = model.groupId,
                    blogVisabilty = model.blogVisabilty,
                    blogvote = model.blogvote,
                    creationTime =model.creationTime
                };
                blogs.Update(newBlog);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            var blog = blogs.Find(id);
            return View(blog);
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, GraduationProject.Data.Models.Blog model)
        {
            try
            {
                blogs.Remove(model.blogId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        //------------------------
        public ActionResult UpVote(int id)
        {
            var blog = blogs.Find(id);
            return UpVote(blog);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpVote(GraduationProject.Data.Models.Blog model )
        {
            try
            {
                var newVote = blogs.Find(model.blogId);
                var userBlog = newVote.userBlog.FirstOrDefault(UserBlog => UserBlog.blogId == model.blogId);
                userBlog.VoteValue = 1;
                
                var newBlog = new GraduationProject.Data.Models.Blog
                {

                    blogId = newVote.blogId,
                    blogtitle = newVote.blogtitle,
                    blogcontent = newVote.blogcontent,
                    groupId = 1,
                    blogVisabilty = newVote.blogVisabilty,
                    blogvote = ++newVote.blogvote,
                    creationTime = newVote.creationTime,
                    
                };
                blogs.Update(newBlog);
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
        //------------
    }
}