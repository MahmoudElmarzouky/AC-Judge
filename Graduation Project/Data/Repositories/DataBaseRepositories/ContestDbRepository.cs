using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using GraduationProject.ViewModels.ContestViewsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Repositories.DataBaseRepositories
{
    public class ContestDbRepository : IContestRepository<Contest>
    {
        readonly private EntitiesContext dbcontext;
        public ContestDbRepository(EntitiesContext dbcontext)
        {
            this.dbcontext = dbcontext;
            foreach(var item in dbcontext.Contests.ToList())
                LoadCurrentContest(item);
        }
    
        public Contest Add(Contest newContest)
        {
            dbcontext.Add(newContest);
            Commit();
            return newContest;
        }
        private UserContest CreateUserContest(int contestId, int userId, Boolean isRegistered, Boolean isFavourite, Boolean isOwner)
        {
            return new UserContest
            {
                ContestId = contestId,
                UserId = userId,
                isRegistered = isRegistered,
                isFavourite = isFavourite,
                isOwner = isOwner
            };
        }
        public Contest CreateNewContest(int userId, Contest newContest)
        {
            var contest = new Contest
            {
                contestDuration = newContest.contestDuration,
                contestStartTime = newContest.contestStartTime,
                contestTitle = newContest.contestTitle,
                contestVisabilty = newContest.contestVisabilty,
                InGroup = newContest.InGroup,
                groupId = newContest.groupId,
                creationTime = DateTime.Now,
            };
            Add(contest); 
            contest.UserContest.Add( CreateUserContest(contest.contestId, userId, true, false, true) );
            Commit();
            return contest; 
        }

        public void Commit()
        {
            dbcontext.SaveChanges();
        }

        public Contest Find(int Id)
        {
            var contest = dbcontext.Contests.FirstOrDefault(contest => contest.contestId == Id);
            return contest;
        }

        public IList<Contest> List()
        {
            return dbcontext.Contests.ToList();
        }
        public IList<Contest> PublicContests()
        {
            return dbcontext.Contests.Where(u => u.InGroup == false).ToList(); 
        }

        public void Remove(int Id)
        {
            var contest = Find(Id);
            if (contest != null)
            {
                dbcontext.Contests.Remove(contest);
                Commit();
            }
        }


        public void Update(Contest newContest)
        {
            var contest = Find(newContest.contestId);
            contest.contestDuration = newContest.contestDuration;
            contest.contestTitle = newContest.contestTitle;
            contest.contestVisabilty = newContest.contestVisabilty;
            contest.contestStartTime = newContest.contestStartTime; 
            Commit();
        }

        public void AddProblemToContest(int problemId, int contestId)
        {
            var contest = Find(contestId);
            int currentNumberofProblems = contest.ContestProblems.Count;
            int problemOrder = currentNumberofProblems + 1;
            if (contest.ContestProblems.FirstOrDefault(u => u.problemId == problemId) != null)
                return;
            contest.ContestProblems.Add(createNewProblemRelation(contestId, problemId, problemOrder));
            Commit(); 
        }
        private ContestProblem createNewProblemRelation(int contestId, int problemId, int order)
        {
            return new ContestProblem {
                contestId = contestId,
                problemId = problemId,
                order = order
            };
        }
        public void RegisterInContest(int userId, int contestId)
        {
            var contest = Find(contestId);
            if (contest == null)
                return;
            var userContest = contest.UserContest.FirstOrDefault(u => u.UserId == userId);
            if (userContest == null)
            {
                userContest = CreateUserContest(contestId, userId, true, false, false);
            }
            else
            {
                userContest.isRegistered = true;
            }
            Commit();
        }
        public void FlipFavourite(int contestId, int userId)
        {
            var currentUsercontest = getUserContestRole(contestId, userId);
            if (currentUsercontest == null)
                return;
            currentUsercontest.isFavourite ^= true;
            Commit();
        }

        private UserContest getUserContestRole(int contestId, int userId)
        {
            return Find(contestId).UserContest.FirstOrDefault(u => u.UserId == userId); 
        }

        private void LoadCurrentContest(Contest contest)
        {
            dbcontext.Entry(contest).Collection(c => c.ContestProblems).Load();
            dbcontext.Entry(contest).Collection(c => c.UserContest).Load();
            dbcontext.Entry(contest).Collection(c => c.Submissions).Load();
            foreach (var cp in contest.ContestProblems)
                dbcontext.Entry(cp).Reference(c => c.problem).Load();
            foreach (var uc in contest.UserContest)
                dbcontext.Entry(uc).Reference(u => u.User).Load(); 
        }
        private Boolean IsOwner(UserContest userContest, string name)
        {
            if (userContest == null) return false;
            if (name == null) return true; 
            return userContest.User.UserName.Contains(name); 
        }
        private string getContestType(Boolean inGroup)
        {
            return inGroup ? "Group" : "Classical"; 
        }
        private string getContestStatus(int num)
        {
            switch(num)
            {
                case -1:
                    return "upComming";
                case 0:
                    return "Running";
                case 1:
                    return "Ended"; 
            }
            return ""; 
        }
        
        private string RemoveNull(string x)
        {
            if (x == null) x = "";
            return x; 
        }
        private string ChangeToAll(string x)
        {
            x = RemoveNull(x); 
            if (x.Contains("All")) x = "";
            return x; 
        }
        private ContestFilter Fix(ContestFilter model)
        {
            model.contestTitle = ChangeToAll(model.contestTitle);
            model.ContestStatus = ChangeToAll(model.ContestStatus);
            model.ContestType =ChangeToAll(model.ContestType);
            model.PrepeardBy= ChangeToAll(model.PrepeardBy);
            model.ContestX = ChangeToAll(model.ContestX);
            model.ContestPrivacy = ChangeToAll(model.ContestPrivacy); 
            return model;
        }
        private Boolean IsContestStatus(int contestStatusNum, string status)
        {
            // upcoming, running, ended 
            return getContestStatus(contestStatusNum).Contains(status) == true;
        }
        private Boolean IsContestType(Boolean InGroup, string type)
        {
            if (type == "") return true;
            var currentType = getContestType(InGroup);
            return currentType.Contains(type) == true;
        }
        private Boolean IsContestMe(Contest contest, string contestMe, Boolean IsFav, Boolean IsOwner, Boolean hasSubmission, Boolean Isparticipant)
        {
            if (contestMe == "") return true;
            switch(contestMe)
            {
                case "My Contests":
                    return hasSubmission;
                case "My Participation":
                    return Isparticipant; 
                case "My Arrangement":
                    return IsOwner;
                case "My Favorites":
                    return IsFav; 
            }
            return false; 
        }
        private Boolean IsCurrentPrivacy(string contestPrivacy, string privacy)
        {
            if (privacy == "") return true;
            return contestPrivacy.Contains(privacy) == true; 
        }
        private Boolean Isparticipant(Contest contest, int userId)
        {
            var dateDuringContest = contest.contestStartTime.AddMinutes(contest.contestDuration);
            var mySubmissions = contest.Submissions.FirstOrDefault(u => u.userId == userId && u.CreationTime <= dateDuringContest);
            return mySubmissions != null; 
        }
        public IList<Contest> Filter(ContestFilter model)
        {
            model = Fix(model);
            var list = new List<Contest>();
            int userId = model.userId; 
            foreach(var contest in dbcontext.Contests)
            {
                LoadCurrentContest(contest); 
                // see if user can see the contest 
                var role = getUserContestRole(contest.contestId, userId);
                if (role == null)
                    continue;
                var isFav = role.isFavourite;
                var isOwner = role.isOwner;
                var hasSubmission = contest.Submissions.FirstOrDefault(u => u.userId == userId) != null ? true : false;
                var isparticipant = Isparticipant(contest, userId); 
                if (!contest.contestTitle.Contains(model.contestTitle))
                    continue;
                if (!IsOwner(contest.UserContest.FirstOrDefault(u => u.isOwner == true), model.PrepeardBy))
                    continue;
                if (!IsContestStatus(contest.contestStatus, model.ContestStatus))
                    continue;
                if (!IsContestType(contest.InGroup, model.ContestType))
                    continue;
                if (!IsContestMe(contest, model.ContestX, isFav, isOwner, hasSubmission, isparticipant))
                    continue;
                if (!IsCurrentPrivacy(contest.contestVisabilty, model.ContestPrivacy))
                    continue;
                list.Add(contest); 
            }
            return list; 
        }
    }
}
