﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProject.Data.Models;

namespace GraduationProject.ViewModels.ContestViewsModel
{
    public class ViewContestModel
    {
        public ViewContestModel()
        {
            UserContest = new HashSet<UserContest>();
            Submissions = new HashSet<Submission>();
            Problems = new HashSet<Problem>();
        }
        public Boolean IsFavourite { get; set; }
        public string FavouuriteClass { get {
                return IsFavourite ? "fas fa-heart active" : "fas fa-heart"; 
            } set { } }
        public int contestId { get; set; }
        public string contestTitle { get; set; }
        public DateTime contestStartTime { get; set; }
        public int contestDuration { get; set; }
        public DateTime creationTime { get; set; }
        public string contestVisabilty { get; set; }
        public string VisableClass { get { return contestVisabilty == "Public" ? "fas fa-users" : "fas fa-users-slash"; } set { } }
        public string contestStatus { get; set; } // upcoming, running, ended 
        public string contestStatusClass { get {
                switch (contestStatus)
                {
                    case "Upcoming":
                        return "scheduled"; 
                    case "Running":
                        return "running";
                    case "Ended":
                        return "ended"; 
                }
                return ""; 
            } set { } }
        public int PreparedById { get; set; }
        public string PreparedBy { get; set; }
        public  ICollection<UserContest> UserContest { get; set; }
        public  ICollection<Submission> Submissions { get; set; }
        public  ICollection<Problem> Problems { get; set; }
        public int? groupId { get; set; }
        public string GlobalTime { get
            {
                return string.Format("https://www.timeanddate.com/worldclock/fixedtime.html?day={0}&month={1}&year={2}&hour={3}&min={4}&sec={5}&p1=166", contestStartTime.Day, contestStartTime.Month, contestStartTime.Year, contestStartTime.Hour, contestStartTime.Minute, contestStartTime.Second);
            }
            set { } }
        public ContestFilter Filter { get; set; }
    }
}
