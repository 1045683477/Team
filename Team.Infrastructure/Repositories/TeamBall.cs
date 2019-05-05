using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Team.Infrastructure.DbContext;
using Team.Infrastructure.IRepositories;
using Team.Model.Model;

namespace Team.Infrastructure.Repositories
{
    public class TeamBall:ITeamBall
    {
        #region Initial

        private readonly MyContext _myContext;

        public TeamBall(MyContext myContext)
        {
            _myContext = myContext;
        }

        #endregion

        /// <summary>
        /// 创建队伍,true为创建成功，false为创建失败
        /// </summary>
        /// <param name="userId">创建人 Id</param>
        /// <param name="team"></param>
        /// <returns></returns>
        public bool TeamCreate(Participants participants, Model.Model.Team team)
        {
            #region SQL

            //SqlParameter[] parameters = new[]
            //{
            //    new SqlParameter("Sport", team.Sport),
            //    new SqlParameter("AllCount", team.AllCount),
            //    new SqlParameter("Note", team.Note),
            //    new SqlParameter("AgreedTime", team.AgreedTime),
            //    new SqlParameter("UserId", participants.UserId),
            //    new SqlParameter("CreationTime",DateTime.Now), 
            //    new SqlParameter("Count",1),
            //    new SqlParameter("Name",participants.Name),
            //    new SqlParameter(""), 
            //};

            ////var createTeam =await _myContext.Database.ExecuteSqlCommandAsync(
            //    "insert into Teams values (@CreationTime,@AgreedTime,@Note,@AllCount,@Count,@Sport,@UserId);",
            //    parameters);

            #endregion

            team.UserId = participants.UserId;
            team.CreationTime=DateTime.Now;
            team.Count++;

            var teamChanged=_myContext.Teams.Add(team);

            participants.TeamId = team.Id;

            var partChanged = _myContext.Participantses.Add(participants);
            
            if (teamChanged==null || partChanged==null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 查询该用户所有创建记录
        /// </summary>
        /// <param name="userId">用户 Id</param>
        /// <returns></returns>
        public IEnumerable<Model.Model.Team> TeamSearchUserCreateAll(int userId)
        {
            return _myContext.Teams.Where(x => x.UserId == userId).Include(x => x.Participantses).ToList();
        }

        /// <summary>
        /// 查询该用户所有参加记录
        /// </summary>
        /// <param name="userId">用户 Id</param>
        /// <returns></returns>
        public IEnumerable<Participants> TeamSearchUserJoinAll(int userId)
        {
            var teamId = _myContext.Participantses.Where(x => x.UserId == userId).Include(x=>x.Team).ToList();
            if (!teamId.Any())
            {
                return null;
            }
            return teamId;
        }

        /// <summary>
        /// 查询某个运动所有正在组队队伍
        /// </summary>
        /// <param name="sport">运动类型</param>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public IEnumerable<Model.Model.Team> TeamSearchTeaming(Sport sport,int userId)
        {
            var user = _myContext.Users.SingleOrDefault(x => x.Id == userId);
            var a=  _myContext.Teams.Where(x => x.AgreedTime > DateTime.Now && x.AllCount > x.Count && x.Sport==sport && x.User.UniversityId==user.UniversityId).ToList();
            //var a = _myContext.Teams.Include(x=>x.User).ToList();//.Where(x => x.User.UniversityId == universityId)
            //var b = _myContext.Teams
            return a;
        }

        /// <summary>
        /// 查询某个正在组队的团队信息
        /// </summary>
        /// <param name="teamId">团队 Id</param>
        /// <returns></returns>
        public Model.Model.Team TeamSearchByIdTeaming(int teamId)
        {
            return _myContext.Teams.Include(x => x.Participantses).SingleOrDefault(x => x.Id == teamId);
        }

        /// <summary>
        /// 通过队伍名查询正在组队团队
        /// </summary>
        /// <param name="name">队伍名</param>
        /// <param name="userId">用户 ID</param>
        /// <returns></returns>
        public Model.Model.Team TeamSearchByName(string name,int userId)
        {
            var user = _myContext.Users.SingleOrDefault(x => x.Id == userId);
            return _myContext.Teams.Include(x=>x.Participantses).SingleOrDefault(x => x.Name == name && x.AgreedTime>DateTime.Now && x.User.UniversityId==user.UniversityId);
        }

        /// <summary>
        /// 参加队伍
        /// </summary>
        /// <param name="participants">参加者模板</param>
        /// <param name="user"></param>
        /// <param name="teamId">队伍 Id</param>
        /// <returns></returns>
        public bool ParticipateInTeam(User user, int teamId)
        {
            var exits=_myContext.Teams.Include(x=>x.Participantses).SingleOrDefault(x => x.Id == teamId);
            if (exits == null || (exits.Participantses.SingleOrDefault(s=>s.UserId==user.Id))!=null)
            {
                return false;
            }

            int full = exits.AllCount - exits.Count;
            if (full<=0)
            {
                return false;
            }
            Participants participants=new Participants
            {
                Name = user.Name,
                TeamId = teamId,
                UserId = user.Id,
                Team = TeamSearchByIdTeaming(teamId),
                Sex = user.Sex
            };
            exits.Participantses.Add(participants);
            exits.Count++;
            return true;
        }
    }
}
