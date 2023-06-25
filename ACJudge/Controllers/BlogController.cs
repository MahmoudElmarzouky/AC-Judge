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
using Microsoft.Extensions.Logging;

namespace ACJudge.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private readonly IBlogRepository<Blog> _blogs;
        private readonly IRepository<Comment> _comments;
        private readonly User _user;
        private readonly ILogger<BlogController> _logger;
        private const int BlogsPerPage = 10;
        private const int NonGroupId = -1;

        public BlogController(IBlogRepository<Blog> blogs
            , IUserRepository<User> userRepository
            , IHttpContextAccessor httpContextAccessor,
            IRepository<Comment>comments,
            ILogger<BlogController> logger
            )
        {
            _logger = logger;
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
                var blogs = _blogs.List();
                    var pageNumber = page ?? 1;
                var numberOfPages = (int)Math.Ceiling((decimal)blogs.Count / BlogsPerPage);
                var currentPageBlogs = blogs.Paginate(pageNumber, BlogsPerPage).
                    Select(blog => BlogViewMapper.GetViewModel(blog, _user)).
                    ToList();
                var currentPage = new BlogPage(currentPageBlogs, pageNumber, Pagination.GetAccessiblePagesNumbers(1, pageNumber, numberOfPages));
                return View(currentPage);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
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
            }catch(Exception e){
                _logger.LogError(e, e.Message);
                return View("ErrorLink");
            }
        }

        public ActionResult CreateComment (int blogId, string commentContent)
        {
            try
            {
                var newComment = new Comment(commentContent, blogId, _user.UserId);
                _comments.Add(newComment);
                return RedirectToAction("Details",new { id = blogId});
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
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
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
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
                var newBlog = new Blog(model.BlogTitle, model.BlogContent,
                    model.GroupId == NonGroupId ? null : model.GroupId, _user.UserId);
                _blogs.Add(newBlog);
                return RedirectToAction("Details",new { id = newBlog.BlogId});
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
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
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
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
                if (blog == null)
                    throw new NullReferenceException("blog is not exist");
                blog.BlogContent = model.BlogContent;
                blog.BlogTitle = model.BlogTitle;
                blog.BlogVisibility = model.BlogVisibility;
                _blogs.Update(blog);
                return RedirectToAction("Details", new { id = model.BlogId });
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
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
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
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
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
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
                var list = _blogs.Search(title, preparedBy);
                    var numberOfPages = (int)Math.Ceiling((decimal)list.Count / BlogsPerPage);
                if (page > numberOfPages)
                    page = numberOfPages;
                if (page < 1)
                    page = 1;
                var currentPage = list.Paginate(page, BlogsPerPage).
                    Select(blog=>BlogViewMapper.GetViewModel(blog, _user)).
                    ToList();
                var blogPage = new BlogPage(currentPage, page, Pagination.GetAccessiblePagesNumbers(1, page, numberOfPages), 
                    new Filter
                    {
                        Title = title,
                        PreparedBy = preparedBy
                    });
                
                return View("Index", blogPage);
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
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
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
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
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                return -1;
            }
        }
        public int Favourite(int id)
        {
            try
            {
                return _blogs.UpdateFavourite(id, _user.UserId);
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                return 0;
            }
        }
    }
}