using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Team.Infrastructure.DbContext;
using Team.Infrastructure.IRepositories;
using Team.Model.AutoMappers.RunTeamMapper;
using Team.Model.Model.RunTeamModel;
using Team.Model.Model.UserModel;

namespace Team.Infrastructure.Repositories
{
    public class RunTeamResource: IRunTeamResource
    {
        #region Initail

        private readonly MyContext _myContext;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RunTeamResource(
            MyContext myContext,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _myContext = myContext;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        #endregion


        /// <summary>
        /// 创建队伍,true为创建成功，false为创建失败
        /// </summary>
        /// <param name="participants">参与者模板</param>
        /// <param name="runTeam">团队模板</param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool TeamCreate(RunParticipants participants, RunTeam runTeam, User user)
        {
            if (user.RunTeamId != 0)
            {
                return false;
            }

            runTeam.UserId = participants.UserId;
            runTeam.CreationTime = DateTime.Now;
            runTeam.Count++;
            runTeam.ApplicationStatus = ApplicationStatus.Yes;
            participants.ApplicationDate=DateTime.Now;

            _myContext.RunTeams.Add(runTeam);

            participants.RunTeamId = runTeam.Id;

            _myContext.RunParticipantses.Add(participants);

            return true;
        }

        /// <summary>
        /// 删除自身创建的队伍,true为删除成功，false为删除失败
        /// </summary>
        /// <param name="userId">创建者 id</param>
        /// <returns></returns>
        public bool DeleteTeamAsync(int userId)
        {
            var runTeam = _myContext.RunTeams.SingleOrDefault(x => x.UserId == userId);
            var user = _myContext.Users.SingleOrDefault(x => x.Id == userId);
            if (runTeam!=null)
            {
                user.RunTeamId = 0;
                user.Role = Role.Client;
                _myContext.Users.Update(user);
                _myContext.Remove(runTeam);

                return true;
            }
            return false;
        }

        /// <summary>
        /// 修改自己组队的队伍状态，是否允许扩充
        /// </summary>
        /// <param name="runTeam"></param>
        public async Task<bool> UpdateTeam(RunTeamUpdateMap runTeam)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("Name", runTeam.Name),
                new SqlParameter("Introduction", runTeam.Introduction),
                new SqlParameter("ApplicationStatus", runTeam.ApplicationStatus),
                new SqlParameter("Id",runTeam.TeamId), 
            };
            var update =await _myContext.Database.ExecuteSqlCommandAsync(
                "update RunTeams set Name=@Name,Introduction=@Introduction,ApplicationStatus=@ApplicationStatus where Id=@Id;", parameters);
            if (update > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 查询所有正在组队的允许参加的队伍
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TeamList> RunTeamsSearchAll()
        {
            var runTeams = _myContext.TeamLists.ToList();
            return runTeams;
        }

        /// <summary>
        /// 查询某个正在组队的团队信息
        /// </summary>
        /// <param name="teamId">团队 Id</param>
        /// <returns></returns>
        public RunTeam TeamSearchByIdTeaming(int teamId)
        {
            return _myContext.RunTeams.Include(x => x.RunParticipantses).SingleOrDefault(x => x.Id == teamId);
        }


        /// <summary>
        /// 申请参加队伍
        /// </summary>
        /// <param name="applicant">申请信息</param>
        /// <returns></returns>
        public void ParticipateInTeam(RunApplicant applicant)
        {
            _myContext.RunApplicants.Add(applicant);
        }

        /// <summary>
        /// 所有查询申请者信息
        /// </summary>
        /// <param name="teamId">申请者 Id</param>
        public IEnumerable<RunApplicant> SearchAllParticipate(int teamId)
        {
            return _myContext.RunApplicants.Where(x => x.RunTeamId == teamId).ToList();
        }

        /// <summary>
        /// 查询单个 申请者信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public RunApplicant SearchByIdApplicant(int UserId)
        {
            var user=_userRepository.UserSearch(UserId);
            if (user.RunTeamId != 0)
            {
                return null;
            }
            return _myContext.RunApplicants.SingleOrDefault(x => x.UserId == UserId);
        }

        /// <summary>
        /// 同意入队
        /// </summary>
        /// <param name="participants"></param>
        public void AgreeWithTheTeam(RunParticipants participants)
        {
            _myContext.RunParticipantses.Add(participants);
            var user=_userRepository.UserSearch(participants.UserId);
            user.RunTeamId = participants.RunTeamId;
        }

        /// <summary>
        /// 离开队伍
        /// </summary>
        /// <param name="userId">用户 Id</param>
        /// <param name="teamId">队伍 Id</param>
        /// <returns></returns>
        public void LeaveTheTeam(int userId, int teamId)
        {
            var user=_myContext.RunParticipantses.SingleOrDefault(x => x.UserId == userId && x.RunTeamId == teamId);
            if (user!=null)
            {
                _myContext.RunParticipantses.Remove(user);
                _unitOfWork.SaveChanged();
            }
        }

        /// <summary>
        /// 查询当前队伍排名 Id
        /// </summary>
        /// <param name="teamId">队伍 Id</param>
        /// <returns></returns>
        public int LevelTeamById(int teamId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查看当前学校队伍前一百名
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<RunTeam> LevelTeam(int userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 修改 user 队伍 Id
        /// </summary>
        public void UpdateUserRunTeamState(User user, int teamId)
        {
            user.RunTeamId = teamId;
            user.Role = Role.Captain;
            _myContext.Update(user);
        }
    }
}
