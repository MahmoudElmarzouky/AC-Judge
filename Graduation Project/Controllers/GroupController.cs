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
        private readonly IRepository<GraduationProject.Data.Models.Group> groups;
        private readonly IUserRepository<User> users;
        private readonly User user;
       

        public GroupController(IRepository<GraduationProject.Data.Models.Group> groups, IUserRepository<User> Userrepository, IHttpContextAccessor httpContextAccessor)
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value; 
            user = Userrepository.Find(userId);
            this.groups = groups;
            users = Userrepository; 
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
            newGroup.UserGroup.Add(CreateRelation(user.UserId, newGroup.GroupId, "Participant"));
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
                var userGroup = CreateRelation(userId, groupId, "Creator");
                newGroup.UserGroup.Add(userGroup);
                groups.Update(newGroup); 
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        // For Testing 
        private void AddAllUserToGroup(GraduationProject.Data.Models.Group newGroup)
        {
            int groupId = newGroup.GroupId; 
            foreach (var currentUser in users.List())
            {
                if (newGroup.UserGroup.FirstOrDefault(u => u.GroupId == groupId && u.UserId == currentUser.UserId) == null)
                {
                    var userGroupTemp = CreateRelation(currentUser.UserId, groupId, "Participant");
                    newGroup.UserGroup.Add(userGroupTemp);

                }
            }
            groups.Update(newGroup);
        }
        private UserGroup CreateRelation(int userId, int groupId, string role)
        {
            var usergroup = new UserGroup { UserId = userId, GroupId = groupId, UserRole = role, MemberSince = DateTime.Now, isFavourite = false };
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
                var currentGroup = groups.Find(groupId); 
                foreach(var name in allNames)
                {
                    var currentUser = users.FindByUserName(name); 
                    if(currentUser != null)
                    {
                        var oldUserGroup = currentGroup.UserGroup.FirstOrDefault(ug => ug.GroupId == groupId && ug.UserId == currentUser.UserId); 
                        if (oldUserGroup == null)
                        {
                            var newUserGroup = new UserGroup { GroupId = groupId, UserId = currentUser.UserId, isFavourite = false, MemberSince = DateTime.Now, UserRole = "Participant" };
                            currentGroup.UserGroup.Add(newUserGroup);
                             
                        }else
                        {
                            // he is here no need to do any thnig
                        }
                    }
                }
                groups.Update(currentGroup);
                return RedirectToAction("Details", new { id = groupId});
            }
            catch
            {
                return RedirectToAction("Details", new { id = groupId });
            }

        }

        
        public ActionResult Leave(int id)
        {
            var group = groups.Find(id);
            var model = getCreateModelFromGroup(group);
            return Leave(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Leave(CreateGroupModel model)
        {
            try
            {
                int userId = user.UserId;
                int groupId = model.GroupId;
                var group = groups.Find(groupId);
                var GroupUserReal = group.UserGroup.FirstOrDefault(u => u.UserId == userId && u.GroupId == groupId);
                var role = GroupUserReal.UserRole;
                group.UserGroup.Remove(GroupUserReal);
                groups.Update(group);
                if (group.UserGroup.Count == 0)
                {
                    // if the only member in the group leaves 
                    // then delete the group 
                    return Delete(model);
                }
                if(role == "Creator")
                {
                    var anyManger = group.UserGroup.FirstOrDefault(u => u.UserRole == "Manager"); 
                    if (anyManger == null)
                        anyManger = group.UserGroup.FirstOrDefault(u => u.GroupId == groupId);
                    anyManger.UserRole = "Creator";
                    groups.Update(group);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult FlipFavourite(int id)
        {
            var group = groups.Find(id);
            var model = getCreateModelFromGroup(group);
            return FlipFavourite(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FlipFavourite(CreateGroupModel model)
        {
            try
            {
                int userId = user.UserId;
                int groupId = model.GroupId;
                var group = groups.Find(groupId);
                var GroupUserReal = group.UserGroup.FirstOrDefault(u => u.UserId == userId && u.GroupId == groupId);
                if (GroupUserReal == null)
                {
                    return RedirectToAction("Details", new { id = model.GroupId });
                }
                GroupUserReal.isFavourite ^= true;
                groups.Update(group);
                return RedirectToAction("Details", new { id = model.GroupId });
            }
            catch
            {
                // there is no view for this method so, if you come here you will see exception 
                return View();
            }
        }
        public ActionResult JoinToGroup(int Id)
        {
            var group = groups.Find(Id);
            return JoinToGroup(group); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JoinToGroup(GraduationProject.Data.Models.Group currentGroup)
        {
            
            try
            {
                int groupId = currentGroup.GroupId;
                int userId = user.UserId;
                var rel = currentGroup.UserGroup.FirstOrDefault(u => u.GroupId == groupId && u.UserId == userId);
                if (rel == null)
                {
                    rel = CreateRelation(userId, groupId, "Participant");
                    currentGroup.UserGroup.Add(rel); 
                }
                groups.Update(currentGroup); 
                return RedirectToAction("Details", new { id = currentGroup.GroupId });
            }
            catch
            {
                return RedirectToAction("Details", new { id = currentGroup.GroupId });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMember(int groupId, int userId, string buttonName)
        {
            try
            {
                var currentGroup = groups.Find(groupId);
                var rel = currentGroup.UserGroup.FirstOrDefault(r => r.GroupId == groupId && r.UserId == userId);
                if (rel == null)
                    return RedirectToAction("Details", new { id = groupId });
                currentGroup.UserGroup.Remove(rel);
                switch (buttonName)
                {
                    case "0":
                        rel.UserRole = "Manager";
                        break;
                    case "-1":
                        rel.UserRole = "Participant";
                        break;
                    case "delete":
                        rel = null;
                        break;
                }
                if (rel != null)
                    currentGroup.UserGroup.Add(rel);
                groups.Update(currentGroup);
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
            var IsFavourite = query != null? query.isFavourite: false;
            var role = query != null ? query.UserRole : "not Set"; 
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
                BlogGroup=group.blogs.ToList(),
                IsFavourite = IsFavourite,
                CurrentUserId = user.UserId,
                
            };
            return model;
        }
    }
}
