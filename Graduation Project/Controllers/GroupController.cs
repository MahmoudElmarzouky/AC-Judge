using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProject.Data.Repositories.Interfaces;
using GraduationProject.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GraduationProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GraduationProject.Controllers.Group
{
    [Authorize]
    public class GroupController : Controller
    {
        private readonly IRepository<GraduationProject.Data.Models.Group> groups;
        private readonly User user;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GroupController(IRepository<GraduationProject.Data.Models.Group> groups, IUserRepository<User> Userrepository, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value; 
            this.groups = groups;
            user = Userrepository.Find(userId);
        }
        // GET: HomeController

        private void AddGroupData()
        {
            var newGroup = new GraduationProject.Data.Models.Group { GroupTitle = "The Best Group Ever", GroupDescription = "This Group is For Solving Problems And Also Solving Puzzles"};
            groups.Add(newGroup);
            newGroup = new GraduationProject.Data.Models.Group { GroupTitle = "The Ghots", GroupDescription = "This Group is For Solving Problems And Also Solving Puzzles"};
            groups.Add(newGroup);
            newGroup = new GraduationProject.Data.Models.Group { GroupTitle = "LOL is LOL", GroupDescription = "This Group is For Solving Problems And Also Solving Puzzles"};
            groups.Add(newGroup);
            newGroup = new GraduationProject.Data.Models.Group { GroupTitle = "Bad semicolon", GroupDescription = "This Group is For Solving Problems And Also Solving Puzzles"};
            groups.Add(newGroup);
            newGroup = new GraduationProject.Data.Models.Group { GroupTitle = "Train Hard And Smart", GroupDescription = "This Group is For Solving Problems And Also Solving Puzzles"};
            groups.Add(newGroup);

        }
        private void AddContestData()
        {
            var newGroup = new GraduationProject.Data.Models.Group { GroupTitle = "The Best Group Ever", GroupDescription = "This Group is For Solving Problems And Also Solving Puzzles" };
            var newContest = new GraduationProject.Data.Models.Contest { contestDuration = 300, contestTitle = "The Red Hat Contest", contestStartTime = DateTime.Now };
            newGroup.Contests.Add(newContest);
            newContest = new GraduationProject.Data.Models.Contest { contestDuration = 150, contestTitle = "The Red Hat Contest2", contestStartTime = DateTime.Now };
            newGroup.Contests.Add(newContest);
            newContest = new GraduationProject.Data.Models.Contest { contestDuration = 300, contestTitle = "The White Hat Contest", contestStartTime = DateTime.Now };
            newGroup.Contests.Add(newContest);
            newContest = new GraduationProject.Data.Models.Contest { contestDuration = 300, contestTitle = "The black Hat Contest", contestStartTime = DateTime.Now };
            newGroup.Contests.Add(newContest);
            groups.Add(newGroup);
            newGroup.UserGroup.Add(CreateRelation(user.UserId, newGroup.GroupId));
            groups.Update(newGroup); 
        }
        public ActionResult Index()
        {
           // AddContestData(); 
            var list = new List<ViewGroupModel>();
            foreach (var item in groups.List())
                list.Add(getViewModelFromGroup(item)); 
           
            return View(list);
        }

        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            var group = groups.Find(id);
            var model = getViewModelFromGroup(group); 
            return View(model);
        }

        // GET: HomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateGroupModel model)
        {
            try
            {
                var newGroup = getGroupFromCreateModel(model); 
                groups.Add(newGroup);
                int userId = user.UserId;
                int groupId = newGroup.GroupId;
                var userGroup = CreateRelation(userId, groupId);
                newGroup.UserGroup.Add(userGroup);
                groups.Update(newGroup); 

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private UserGroup CreateRelation(int userId, int groupId)
        {
            var usergroup = new UserGroup { UserId = userId, GroupId = groupId, UserRole = "Admin", MemberSince = DateTime.Now, isFavourite = false };
            return usergroup; 
        }

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            var group = groups.Find(id);
            var model = getCreateModelFromGroup(group);
            return View(model);
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateGroupModel model)
        {
            try
            {
                var newGroup = getGroupFromCreateModel(model);
                groups.Update(newGroup); 
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
            var group = groups.Find(id);
            var model = getCreateModelFromGroup(group);
            return View(model);
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CreateGroupModel model)
        {
            try
            {
                groups.Remove(model.GroupId); 
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private GraduationProject.Data.Models.Group getGroupFromCreateModel(CreateGroupModel model)
        {
            var newGroup = new GraduationProject.Data.Models.Group { 
                GroupId = model.GroupId, 
                GroupTitle = model.GroupTitle, 
                GroupDescription = model.GroupDescription, 
                Password = model.Password, 
                Visable = model.Visable, 
                creationTime = DateTime.Now,
            };
            return newGroup; 
        }
        private CreateGroupModel getCreateModelFromGroup(GraduationProject.Data.Models.Group group)
        {
            var model = new CreateGroupModel { GroupId = group.GroupId, GroupTitle = group.GroupTitle, GroupDescription = group.GroupDescription, Password = group.Password , Visable = group.Visable};
            return model;
        }
        private ViewGroupModel getViewModelFromGroup(GraduationProject.Data.Models.Group group)
        {
            
            int NumberOfMembers = group.UserGroup.Count;
            var query = group.UserGroup.FirstOrDefault(u => u.UserId == user.UserId);
            var role = query != null? query.UserRole: "Not Set";
            var userGroupRel = group.UserGroup.Where(u => u.GroupId == group.GroupId).ToList();
            var model = new ViewGroupModel {
                GroupId = group.GroupId,
                GroupTitle = group.GroupTitle,
                GroupDescription = group.GroupDescription,
                UserRole = role,
                NumberOfMembers = NumberOfMembers,
                GroupStatus = group.Visable ? "Public" : "Private",
                creationTime = group.creationTime,
                UserGroup = userGroupRel,
                Contests = group.Contests.ToList()
            };
            return model;
        }
    }
}
