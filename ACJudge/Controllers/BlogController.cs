using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ACJudge.Data.Models;
using ACJudge.Data.Repositories.Interfaces;
using ACJudge.ExtensionMethods;
using ACJudge.ViewModels;
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
                var pageNumber = page ?? 1;
                var blogs = GetAllBlogs();
                var numberOfPages = (int)Math.Ceiling((decimal)blogs.Count / BlogsPerPage);
                var currentPage = blogs.Paginate(pageNumber, BlogsPerPage);
                var blogPage = new BlogPage(currentPage, pageNumber, Enumerable.Range(1, numberOfPages));
                return View(blogPage);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public List<ViewBlogModel> GetAllBlogs()
        {
            return _blogs.List().Select(GetViewModelFromBlog).ToList();
        }

        public IList<ViewBlogModel> GetBlogsByUser()
        {
            var userBlogs = _blogs.List().
                Where(blog => blog.UserBlog.
                    FirstOrDefault(ub=>ub.UserId == _user.UserId && ub.BlogOwner) != null).
                Select(GetViewModelFromBlog).ToList();
            return userBlogs;
        }
        private ViewBlogModel GetViewModelFromBlog(Blog blog)
        {
            var blogOwner = blog.UserBlog.First(b => b.BlogOwner).User;
            
            var isOwner = blogOwner?.UserIdentityId == _user.UserIdentityId;
            var isFavorite = _user.UserBlogs.
                FirstOrDefault(innerUserBlog => innerUserBlog.IsFavourite 
                                                && innerUserBlog.BlogId==blog.BlogId) != null;
            
            var blogOwnerObject = new BlogOwner
            {
                Id = blogOwner.UserId,
                UserName = blogOwner.UserName,
                PhotoUrl = blogOwner.PhotoUrl
            };
            var model = new ViewBlogModel
            {
                BlogId = blog.BlogId,
                BlogTitle = blog.BlogTitle,
                BlogOwner = blogOwnerObject,
                BlogContent = blog.BlogContent,
                BlogVote = blog.BlogVote, 
                CreationTime = blog.CreationTime, 
                Comments = blog.Comments,
                CurrentUserId=_user.UserId,
                GroupId=blog.GroupId, 
                IsOwner = isOwner,
                IsFavorite=isFavorite
            };
            return model;
        }


        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var blog = _blogs.Find(id);
                if (blog != null)
                    return View(GetViewModelFromBlog(blog));
                return RedirectToAction(nameof(Index));
            }catch(Exception){
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult CreateComment (int blogId,string commentContent)
        {
            try { 
                var newComment = new Comment
                    {
                        Content = commentContent,
                        Upvote = 0,
                        DownVote = 0,
                        BlogId = blogId
                    };
                _comments.Add(newComment);
                var userId = _user.UserId;
                var commentId = newComment.CommentId;
                var commentVotes = CreateCommentRelation(userId, commentId);
                newComment.CommentVotes.Add(commentVotes);
                _comments.Update(newComment);
                return RedirectToAction("Details",new { id = blogId});
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        private CommentVote CreateCommentRelation(int userId, int commentId)
        {
            var commentVotes = new CommentVote
            {
                CommentId = commentId,
                UserId = userId,
                IsFavourite = false,
                Value = 0,
                User = _user
            };
            return commentVotes;
        }
        // GET: HomeController/Create
        public ActionResult Create()
        {
            
            try { 
                // create a blog from a group ,, trying to read the group Id
                var groupId = int.Parse((string)RouteData.Values["id"] ?? string.Empty);
                return View(new Blog
                {
                    GroupId = groupId
                });
            }
            catch (Exception)
            {
                // Create a normal blog
                return View(new Blog
                {
                    GroupId = null
                });
            }
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Blog model)
        {
            try
            {
                var isGroupBlog = model.GroupId != null;
                var newBlog = new Blog
                {
                    BlogTitle = model.BlogTitle,
                    BlogContent = model.BlogContent,
                    GroupId = model.GroupId,
                    BlogVisibility = !isGroupBlog,
                    BlogVote = 0
                };
                _blogs.Add(newBlog);
                var userId = _user.UserId;
                var blogId = newBlog.BlogId;
                var userBlog= _createUserBlogRelation(userId, blogId);
                newBlog.UserBlog.Add(userBlog);
                _blogs.Update(newBlog);
               
                return !isGroupBlog ? RedirectToAction(nameof(Index)) : RedirectToAction("Details", "Group", new { id = model.GroupId });
            }
            catch
            {
                return View();
            }
        }
        private UserBlog _createUserBlogRelation(int userId, int blogId)
        {
            var userBlog = new UserBlog { UserId = userId,
                BlogId = blogId,
                BlogOwner = true,
                IsFavourite = false,
                VoteValue=0,
                User=_user
            };
            return userBlog;
        }
        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            try 
            { 
                if (!_canEditTheBlog(id, _user.UserId))
                {
                    return RedirectToAction(nameof(Index));
                }
                var blog = _blogs.Find(id);
                return View(blog);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
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
                {
                    return RedirectToAction(nameof(Index));
                }
                var blog = _blogs.Find(model.BlogId);
                var newBlog = new Blog
                {
                    BlogId = model.BlogId,
                    BlogTitle = model.BlogTitle,
                    BlogContent = model.BlogContent,
                    GroupId = blog.GroupId,
                    BlogVisibility = model.BlogVisibility,
                    BlogVote = blog.BlogVote
                };
                _blogs.Update(newBlog);
                return RedirectToAction("Details", new { id = model.BlogId });
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
                if (!_canEditTheBlog(id, _user.UserId)) 
                { 
                    return RedirectToAction(nameof(Index));
                }
                var blog = _blogs.Find(id);
                return View(blog);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
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
                {
                    return RedirectToAction(nameof(Index));
                }
                _blogs.Remove(model.BlogId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private bool _canEditTheBlog(int blogId, int userId)
        {
            var blog = _blogs.Find(blogId);
            var rel = blog.UserBlog.FirstOrDefault(u => u.UserId == userId&&u.BlogOwner);
            return  rel != null;
        }
        public ActionResult Filter(string title, string preparedBy, int page = 1)
        {
            try
            {
                title ??= "";
                preparedBy ??= "";
                var list = _blogs.Search(title, preparedBy).Select(GetViewModelFromBlog).ToList();
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
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult UpVote(int id)
        {
            try { 
                var blog = _blogs.Find(id);
                return UpVote(blog);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpVote(Blog model )
        {
            try
            {
                _blogs.UpdateVote(model.BlogId,_user.UserId,1);
                return RedirectToAction("Details", new { id = model.BlogId });
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public ActionResult DownVote(int id)
        {
            try { 
                var blog = _blogs.Find(id);
                return DownVote(blog);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownVote(Blog model)
        {
            try
            {
                _blogs.UpdateVote(model.BlogId, _user.UserId, -1);
                return RedirectToAction("Details", new { id = model.BlogId });
            }
            catch
            {
                return RedirectToAction(nameof(Index));
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
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Favourite(Blog model)
        {
            try
            {
                _blogs.UpdateFavourite(model.BlogId, _user.UserId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}