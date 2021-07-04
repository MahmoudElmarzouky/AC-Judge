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
using GraduationProject.ViewModels.GroupViewsModel;

namespace GraduationProject.Controllers.Group
{
    [Authorize]
    public class GroupController : Controller
    {
        private readonly IGroupRepository<GraduationProject.Data.Models.Group> groups;
        private readonly IUserRepository<User> users;
        private readonly User user;
       

        public GroupController(IGroupRepository<GraduationProject.Data.Models.Group> groups, IUserRepository<User> Userrepository, IHttpContextAccessor httpContextAccessor)
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value; 
            user = Userrepository.Find(userId);
            this.groups = groups;
            users = Userrepository; 
        }
        // GET: HomeController

        
        public ActionResult Index()
        {
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
                groups.CreateNewGroup(user.UserId, newGroup); 
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            var group = groups.Find(id);
            var model = getEditModelFromGroup(group);
            return View(model);
        }

        

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditGroupModel model)
        {
            try
            {
                var newGroup = getGroupFromEditModel(model);
                // need some change 
                groups.Update(newGroup); 
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private GraduationProject.Data.Models.Group getGroupFromEditModel(EditGroupModel model)
        {
            // check old password = password in group 
            return new GraduationProject.Data.Models.Group
            {
                GroupId = model.groupId,
                GroupTitle = model.groupTitle,
                GroupDescription = model.groupDescription,
                Visable = model.Visable,
                Password = model.newPassword
            };
        }
        private EditGroupModel getEditModelFromGroup(Data.Models.Group group)
        {
            return new EditGroupModel
            {
                groupId = group.GroupId,
                groupTitle = group.GroupTitle,
                groupDescription = group.GroupDescription,
                Visable = group.Visable
            };
        }
        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            var group = groups.Find(id);
            var model = getCreateModelFromGroup(group);
            return Delete(model);
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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUsers(int groupId, string textareaUsers)
        {
            try
            {
                var allNames = textareaUsers.Split(" ");
                foreach(var name in allNames)
                {
                    var currentUser = users.FindByUserName(name); 
                    if(currentUser != null)
                    {
                        groups.AddUser(groupId, currentUser.UserId); 
                    }
                }
                return RedirectToAction("Details", new { id = groupId});
            }
            catch
            {
                return RedirectToAction("Details", new { id = groupId });
            }
        }

        
        public ActionResult Leave(int id)
        {
            int userId = user.UserId;
            return Leave(userId, id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Leave(int userId, int groupId)
        {
            try
            {
                groups.RemoveUser(userId, groupId);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Details", new { id = groupId });
            }
        }
        public ActionResult FlipFavourite(int id)
        {
            return FlipFavourite(user.UserId, id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FlipFavourite(int userId, int groupId)
        {
            try
            {
                groups.FlipFavourite(groupId, userId);
                return RedirectToAction("Details", new { id = groupId });
            }
            catch
            {
                return RedirectToAction("Details", new { id = groupId });
            }
        }
        public ActionResult JoinToGroup(int Id)
        {
            return JoinToGroup(Id, user.UserId); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JoinToGroup(int groupId, int userId)
        {
            
            try
            {
                groups.AddUser(groupId, userId);  
                return RedirectToAction("Details", new { id = groupId });
            }
            catch
            {
                return RedirectToAction("Details", new { id = groupId });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMember(int groupId, int userId, string buttonName)
        {
            try
            {
                switch (buttonName)
                {
                    case "0":
                        groups.ChangeUserRole(groupId, userId, "Manager");
                        break;
                    case "-1":
                        groups.ChangeUserRole(groupId, userId, "Participant");
                        break;
                    case "delete":
                        groups.RemoveUser(userId, groupId); 
                        break;
                }
                return RedirectToAction("Details", new { id = groupId });
            }
            catch
            {
                return RedirectToAction("Details", new { id = groupId });
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
            };
            return newGroup; 
        }
        private CreateGroupModel getCreateModelFromGroup(GraduationProject.Data.Models.Group group)
        {

            var tem = group.UserGroup.FirstOrDefault(u => u.UserRole == "Creator");
            int ID = tem == null? 0: tem.UserId; 
            var model = new CreateGroupModel { 
                GroupId = group.GroupId, 
                GroupTitle = group.GroupTitle, 
                GroupDescription = group.GroupDescription, 
                Password = group.Password, 
                Visable = group.Visable, 
                OwnerId = ID
            };
            return model;
        }
        private ViewGroupModel getViewModelFromGroup(GraduationProject.Data.Models.Group group)
        {
            
            int NumberOfMembers = group.UserGroup.Count;
            var query = group.UserGroup.FirstOrDefault(u => u.UserId == user.UserId);
            var IsFavourite = query != null? query.isFavourite: false;
            var role = query != null ? query.UserRole : "--"; 
            var model = new ViewGroupModel {
                UserRole = role,
                GroupId = group.GroupId,
                GroupTitle = group.GroupTitle,
                GroupDescription = group.GroupDescription,
                NumberOfMembers = NumberOfMembers,
                GroupStatus = group.Visable ? "Public" : "Private",
                creationTime = group.creationTime,
                UserGroup = group.UserGroup,
                Contests = group.Contests.ToList(), 
                Blogs= group.blogs.ToList(),
                IsFavourite = IsFavourite,
                CurrentUserId = user.UserId,
            };
            return model;
        }
    }
}
