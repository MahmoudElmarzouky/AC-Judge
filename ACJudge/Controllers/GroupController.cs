﻿using System;
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
using ACJudge.ViewModels.GroupViewsModel;

namespace ACJudge.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        private readonly IGroupRepository<Group> _groups;
        private readonly IUserRepository<User> _users;
        private readonly User _user;
        private const int NumberOfItemsForPage = 12;

        public GroupController(IGroupRepository<Group> groups,
            IUserRepository<User> userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _user = userRepository.Find(userId);
            _groups = groups;
            _users = userRepository;
        }
        // GET: HomeController


        public ActionResult Index()
        {
            var list = _getAllowedGroups();
            return Page(list, 1);
        }
        public ActionResult PageCalledFromAView(int pageNumber)
        {
            try
            {
                var groups = _getAllowedGroups();
                return Page(groups, pageNumber);
            }
            catch
            {
                return View("ErrorLink");
            }
        }
        public ActionResult Page(IEnumerable<ViewGroupModel> groups, int pageNumber)
        {
            try
            {
                var viewGroupModels = groups.ToList();
                var numberOfPages = (int)Math.Ceiling((decimal)viewGroupModels.ToList().Count / NumberOfItemsForPage);
                var list = viewGroupModels.Paginate(pageNumber, NumberOfItemsForPage);
                return View("Index", new GroupPage(list, pageNumber, numberOfPages));
            }
            catch
            {
                return View("ErrorLink");
            }
        }

        public ActionResult MyGroups()
        {
            try
            {
                var list = _getAllowedGroups(true);
                return Page(list, 1);
            }
            catch
            {
                return View("ErrorLink");
            }
        }

        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                // User Can See Group even if he is not a member of the group and group is private 
                var group = _groups.Find(id);
                var model = _getViewModelFromGroup(group);
                if (model != null)
                    return View(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("ErrorLink", "Group Doesn't Exist");
            }
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
                var newGroup = _getGroupFromCreateModel(model);
                _groups.CreateNewGroup(_user.UserId, newGroup);
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
            try
            {
                if (!_groups.IsOwner(id, _user.UserId))
                    return View("ErrorLink", "You Can't Edit This Group");
                var group = _groups.Find(id);
                var model = _getEditModelFromGroup(group);
                return View(model);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditGroupModel model)
        {
            try
            {
                if (!_groups.IsOwner(model.groupId, _user.UserId))
                    return View("ErrorLink", "You Can't Edit This Group");
                var oldPassword = _groups.Find(model.groupId).Password;
                if (oldPassword != model.oldPassword)
                    return View("ErrorLink", "Password is inCorrect");
                var newGroup = _getGroupFromEditModel(model);
                _groups.Update(newGroup);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private static Group _getGroupFromEditModel(EditGroupModel model)
        {
            return new Group
            {
                GroupId = model.groupId,
                GroupTitle = model.groupTitle,
                GroupDescription = model.groupDescription,
                Visible = model.visable == "1",
                Password = model.newPassword
            };
        }

        private static EditGroupModel _getEditModelFromGroup(Group group)
        {
            return new EditGroupModel
            {
                groupId = group.GroupId,
                groupTitle = group.GroupTitle,
                groupDescription = group.GroupDescription,
                Visable = group.Visible,
                visable = group.Visible ? "1" : "0"
            };
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                if (!_groups.IsOwner(id, _user.UserId))
                    return View("ErrorLink", "You Can't Delete This Group");
                var group = _groups.Find(id);
                var model = _getCreateModelFromGroup(group);
                return Delete(model);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CreateGroupModel model)
        {
            try
            {
                if (!_groups.IsOwner(model.GroupId, _user.UserId))
                    return View("ErrorLink", "You Can't Delete This Group");
                _groups.Remove(model.GroupId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InviteUsers(int groupId, string userHandles)
        {
            try
            {
                if (!_groups.IsOwner(groupId, _user.UserId))
                    return RedirectToAction("Details", new { groupId });
                var allNames = userHandles.Split(" ");
                foreach (var name in allNames)
                {
                    var currentUser = _users.FindByUserName(name);
                    if (currentUser != null)
                    {
                        _groups.InviteUser(groupId, currentUser.UserId);
                    }
                }

                return RedirectToAction("Details", new { id = groupId });
            }
            catch
            {
                return RedirectToAction("Details", new { id = groupId });
            }
        }


        public ActionResult Leave(int id)
        {
            var userId = _user.UserId;
            return Leave(userId, id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Leave(int userId, int groupId)
        {
            try
            {
                if (_user.UserId != userId)
                    return View("ErrorLink", "You Don't Have Permission To Delete a user");
                _groups.RemoveUser(userId, groupId);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Details", new { id = groupId });
            }
        }

        public ActionResult FlipFavourite(int id)
        {
            return FlipFavourite(_user.UserId, id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FlipFavourite(int userId, int groupId)
        {
            try
            {
                _groups.FlipFavourite(groupId, userId);
                return RedirectToAction("Details", new { id = groupId });
            }
            catch
            {
                return RedirectToAction("Details", new { id = groupId });
            }
        }

        public ActionResult JoinToGroupWithPassword(int id)
        {
            return JoinToGroupWithPassword(id, _user.UserId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JoinToGroupWithPassword(int groupId, int userId)
        {
            // if the group is public or there is an invitation 
            try
            {
                var group = _groups.Find(groupId);
                if (group.Visible || group.UserGroup.FirstOrDefault(u => u.UserId == userId) != null)
                    _groups.AddUser(groupId, userId);
                else
                    return View("ErrorLink", "You Can't Join This Group");
                return RedirectToAction("Details", new { id = groupId });
            }
            catch
            {
                return RedirectToAction("Details", new { id = groupId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JoinToGroupWithPassword(int groupId, int userId, string password)
        {
            try
            {
                var group = _groups.Find(groupId);
                if (group != null && group.Password == password)
                    _groups.AddUser(groupId, userId);
                return RedirectToAction("Details", new { id = groupId });
            }
            catch
            {
                return RedirectToAction("Details", new { id = groupId });
            }
        }

        public ActionResult AcceptInvitation(int id)
        {
            var userId = _user.UserId;
            return JoinToGroupWithPassword(id, userId);
        }

        public ActionResult RejectInvitation(int id)
        {
            var userId = _user.UserId;
            _groups.RemoveUser(userId, id);
            return RedirectToAction("Invitations");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMember(int groupId, int userId, string buttonName)
        {
            if (!_groups.IsOwner(groupId, _user.UserId))
                return View("ErrorLink", "You Can't Edit User");
            try
            {
                switch (buttonName)
                {
                    case "0":
                        _groups.ChangeUserRole(groupId, userId, "Manager");
                        break;
                    case "-1":
                        _groups.ChangeUserRole(groupId, userId, "Participant");
                        break;
                    case "delete":
                        _groups.RemoveUser(userId, groupId);
                        break;
                }

                return RedirectToAction("Details", new { id = groupId });
            }
            catch
            {
                return RedirectToAction("Details", new { id = groupId });
            }
        }

        public ActionResult Invitations()
        {
            try
            {
                var userId = _user.UserId;
                var list = (from item in _groups.List() 
                    let rel = item.UserGroup.FirstOrDefault(u => u.UserId == userId) 
                    where rel != null && rel.UserRole == "Invite" 
                    select _getViewModelFromGroup(item)).ToList();
                return Page(list, 1);
            }
            catch
            {
                return View("Index");
            }
        }

        private static Group _getGroupFromCreateModel(CreateGroupModel model)
        {
            var newGroup = new Group
            {
                GroupId = model.GroupId,
                GroupTitle = model.GroupTitle,
                GroupDescription = model.GroupDescription,
                Password = model.Password,
                Visible = model.visable == "1",
            };
            return newGroup;
        }

        private static CreateGroupModel _getCreateModelFromGroup(Group group)
        {
            var tem = group.UserGroup.FirstOrDefault(u => u.UserRole == "Creator");
            var id = tem?.UserId ?? 0;
            var model = new CreateGroupModel
            {
                GroupId = group.GroupId,
                GroupTitle = group.GroupTitle,
                GroupDescription = group.GroupDescription,
                Password = group.Password,
                Visable = group.Visible,
                visable = group.Visible ? "1" : "0",
                OwnerId = id
            };
            return model;
        }

        private ViewGroupModel _getViewModelFromGroup(Group group)
        {
            try
            {
                var numberOfMembers = group.UserGroup.Count(u => u.UserRole != "Invite");
                var query = group.UserGroup.FirstOrDefault(u => u.UserId == _user.UserId);
                var isFavourite = query?.IsFavourite ?? false;
                var role = query != null ? query.UserRole : "Not In Group";
                return new ViewGroupModel
                {
                    UserRole = role,
                    GroupId = group.GroupId,
                    GroupTitle = group.GroupTitle,
                    GroupDescription = group.GroupDescription,
                    NumberOfMembers = numberOfMembers,
                    GroupStatus = group.Visible ? "Public" : "Private",
                    creationTime = group.CreationTime,
                    UserGroup = group.UserGroup,
                    Contests = group.Contests.ToList(),
                    Blogs = group.Blogs.ToList(),
                    IsFavourite = isFavourite,
                    CurrentUserId = _user.UserId,
                };
            }
            catch
            {
                return null;
            }
        }

        private IEnumerable<ViewGroupModel> _getAllowedGroups(bool mine = false)
        {
            var list = new List<ViewGroupModel>();
            var userId = _user.UserId;
            var numberOfGroupInvitations = 0;
            // [TODO] fix links when select my groups [1, 2 3, . . . ] should take him to myGroup next
            var groups = mine ? _groups.MyGroups(userId) : _groups.List();
            foreach (var item in groups)
            {
                var rel = item.UserGroup.FirstOrDefault(u => u.UserId == userId);
                if (item.Visible || rel != null)
                {
                    var newItem = _getViewModelFromGroup(item);
                    if (newItem != null)
                        list.Add(newItem);
                }

                if (rel != null && rel.UserRole == "Invite")
                    numberOfGroupInvitations++;
            }

            ViewBag.NumberOfGroupInvitations = numberOfGroupInvitations;
            return list;
        }
    }
}