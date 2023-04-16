using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ACJudge.Data.Models;
using ACJudge.Data.Repositories.Interfaces;
using ACJudge.ExtensionMethods;
using ACJudge.ViewModels.BlogViewModel;

namespace ACJudge.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private readonly IBlogRepository<Blog> _blogs;
        private readonly IRepository<Comment> _comments;
        private readonly User _user;
        private const int BlogsPerPage = 10;
        private const int NonGroupId = -1;

        public BlogController(IBlogRepository<Blog> blogs
            , IUserRepository<User> userRepository
            , IHttpContextAccessor httpContextAccessor,
            IRepository<Comment>comments
            )
        {
            _blogs = blogs;
            _comments = comments;
            var userId = httpContextAccessor.HttpContext?.User.
                FindFirst(ClaimTypes.NameIdentifier)
                ?.Value;
            _user = userRepository.Find(userId);
        }
        // GET: HomeController
        public ActionResult Index(int? page)
        {
            try
            {
                
                var blogs = _blogs.List().
                    Select(blog => BlogViewMapper.GetViewModel(blog, _user)).ToList();
                var pageNumber = page ?? 1;
                var numberOfPages = (int)Math.Ceiling((decimal)blogs.Count / BlogsPerPage);
                var currentPageBlogs = blogs.Paginate(pageNumber, BlogsPerPage);
                var currentPage = new BlogPage(currentPageBlogs, pageNumber, Enumerable.Range(1, numberOfPages));
                return View(currentPage);
            }
            catch (Exception)
            {
                return View("ErrorLink");
            }
        }

        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var blog = _blogs.Find(id);
                return blog == null ? View("ErrorLink") : 
                    View(BlogViewMapper.GetViewModel(blog, _user));
            }catch(Exception){
                return View("ErrorLink");
            }
        }

        public ActionResult CreateComment (int blogId, string commentContent)
        {
            try {
                var newComment = new Comment
                {
                    Content = commentContent,
                    Upvote = 0,
                    DownVote = 0,
                    BlogId = blogId
                };
                var commentVotes = new CommentVote
                {
                    UserId = _user.UserId,
                    IsFavourite = false,
                    Value = 0,
                };
                newComment.CommentVotes.Add(commentVotes);
                _comments.Add(newComment);
                return RedirectToAction("Details",new { id = blogId});
            }
            catch (Exception)
            {
                return View("ErrorLink");
            }
        }
        // GET: HomeController/Create
        public ActionResult Create()
        {
            try { 
                // create a blog from a group ,, trying to read the group Id
                var idString = (string)RouteData.Values["id"] ?? string.Empty;
                var groupId = idString.Equals(string.Empty) ? NonGroupId : int.Parse(idString);
                return View(new Blog { GroupId = groupId });
            }
            catch (Exception)
            {
                return View("ErrorLink");
            }
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Blog model)
        {
            try
            {
                var isGroupBlog = model.GroupId != NonGroupId;
                var newBlog = new Blog
                {
                    BlogTitle = model.BlogTitle,
                    BlogContent = model.BlogContent,
                    GroupId = !isGroupBlog? null: model.GroupId,
                    BlogVisibility = !isGroupBlog,
                    BlogVote = 0
                };
                var userBlog = new UserBlog 
                { 
                    UserId = _user.UserId,
                    BlogOwner = true,
                    IsFavourite = false,
                    VoteValue = 0
                };
                newBlog.UserBlog.Add(userBlog);
                _blogs.Add(newBlog);
                return !isGroupBlog ? 
                    RedirectToAction(nameof(Index)) : 
                    RedirectToAction("Details", "Group", new { id = model.GroupId });
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
                if (!_canEditTheBlog(id, _user.UserId))
                {
                    return View("ErrorLink");
                }
                var blog = _blogs.Find(id);
                return View(blog);
            }
            catch (Exception)
            {
                return View("ErrorLink");
            }
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Blog model)
        {
            try
            {
                if (!_canEditTheBlog(id, _user.UserId))
                    return View("ErrorLink");
                
                var blog = _blogs.Find(model.BlogId);
                blog.BlogContent = model.BlogContent;
                blog.BlogTitle = model.BlogTitle;
                blog.BlogVisibility = model.BlogVisibility;
                _blogs.Update(blog);
                return RedirectToAction("Details", new { id = model.BlogId });
            }
            catch
            {
                return View("ErrorLink");
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            try 
            { 
                if (!_canEditTheBlog(id, _user.UserId)) 
                    return View("ErrorLink");
                
                var blog = _blogs.Find(id);
                return View(blog);
            }
            catch (Exception)
            {
                return View("ErrorLink");
            }
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Blog model)
        {
            try
            {
                if (!_canEditTheBlog(id, _user.UserId))
                    return View("ErrorLink");
                
                _blogs.Remove(model.BlogId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("ErrorLink");
            }
        }
        private bool _canEditTheBlog(int blogId, int userId)
        {
            var blog = _blogs.Find(blogId);
            var rel = blog.UserBlog.FirstOrDefault(u => u.UserId == userId && u.BlogOwner);
            return  rel != null;
        }
        public ActionResult Filter(string title, string preparedBy, int page = 1)
        {
            try
            {
                title ??= "";
                preparedBy ??= "";
                var list = _blogs.Search(title, preparedBy).
                    Select(blog=>BlogViewMapper.GetViewModel(blog, _user)).ToList();
                var numberOfPages = (int)Math.Ceiling((decimal)list.Count / BlogsPerPage);
                var currentPage = list.Paginate(page, BlogsPerPage);
                var blogPage = new BlogPage(currentPage, page, Enumerable.Range(1, numberOfPages), 
                    new Filter
                    {
                        Title = title,
                        PreparedBy = preparedBy
                    });
                
                return View("Index", blogPage);
            }
            catch (Exception)
            {
                return View("ErrorLink");
            }
        }

        public int UpVote(int id)
        {
            try
            {
                _blogs.UpdateVote(id,_user.UserId,1);
                var blog = _blogs.Find(id);
                return blog.BlogVote;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public int DownVote(int id)
        {
            try { 
                _blogs.UpdateVote(id, _user.UserId, -1);
                var blog = _blogs.Find(id);
                return blog.BlogVote;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public ActionResult Favourite(int id)
        {
            try { 
                var blog = _blogs.Find(id);
                return Favourite(blog);
            }
            catch (Exception)
            {
                return View("ErrorLink");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Favourite(Blog model)
        {
            try
            {
                _blogs.UpdateFavourite(model.BlogId, _user.UserId);
                return RedirectToAction("Details", new { id = model.BlogId });
            }
            catch
            {
                return View("ErrorLink");
            }
        }
    }
}