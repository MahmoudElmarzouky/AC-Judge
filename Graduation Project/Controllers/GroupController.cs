using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProject.Data.Repositories.Interfaces;
using GraduationProject.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GraduationProject.ViewModels;

namespace GraduationProject.Controllers.Group
{
    public class GroupController : Controller
    {
        private readonly IRepository<GraduationProject.Data.Models.Group> groups;
        public GroupController(IRepository<GraduationProject.Data.Models.Group> groups)
        {
            this.groups = groups;

        }
        // GET: HomeController

        private void AddData()
        {
            var newGroup = new GraduationProject.Data.Models.Group { GroupTitle = "The Best Group Ever", GroupDescription = "This Group is For Solving Problems And Also Solving Puzzles", NumberOfUsers = 103 };
            groups.Add(newGroup);
            newGroup = new GraduationProject.Data.Models.Group { GroupTitle = "The Ghots", GroupDescription = "This Group is For Solving Problems And Also Solving Puzzles", NumberOfUsers = 35 };
            groups.Add(newGroup);
            newGroup = new GraduationProject.Data.Models.Group { GroupTitle = "LOL is LOL", GroupDescription = "This Group is For Solving Problems And Also Solving Puzzles", NumberOfUsers = 50 };
            groups.Add(newGroup);
            newGroup = new GraduationProject.Data.Models.Group { GroupTitle = "Bad semicolon", GroupDescription = "This Group is For Solving Problems And Also Solving Puzzles", NumberOfUsers = 3 };
            groups.Add(newGroup);
            newGroup = new GraduationProject.Data.Models.Group { GroupTitle = "Train Hard And Smart", GroupDescription = "This Group is For Solving Problems And Also Solving Puzzles", NumberOfUsers = 120 };
            groups.Add(newGroup);

        }
        public ActionResult Index()
        {   
            var list = groups.List(); 

            return View(list);
        }

        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            var group = groups.Find(id); 
            return View(group);
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
                var newGroup = new GraduationProject.Data.Models.Group{ GroupTitle = model.GroupTitle, GroupDescription = model.GroupDescription, Password = model.Password, creationTime = DateTime.Now };
                groups.Add(newGroup); 
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
            var model = getModelFromGroup(group);
            return View(model);
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateGroupModel model)
        {
            try
            {
                var newGroup = getGroupFromModel(model);
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
            var model = getModelFromGroup(group);
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
        private GraduationProject.Data.Models.Group getGroupFromModel(CreateGroupModel model)
        {
            var newGroup = new GraduationProject.Data.Models.Group { GroupId = model.GroupId, GroupTitle = model.GroupTitle, GroupDescription = model.GroupDescription, Password = model.Password , Visable = model.Visable};
            return newGroup; 
        }
        private CreateGroupModel getModelFromGroup(GraduationProject.Data.Models.Group group)
        {
            var model = new CreateGroupModel { GroupId = group.GroupId, GroupTitle = group.GroupTitle, GroupDescription = group.GroupDescription, Password = group.Password , Visable = group.Visable};
            return model;
        }
    }
}
