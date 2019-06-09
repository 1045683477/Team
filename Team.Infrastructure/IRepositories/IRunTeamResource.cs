using System.Collections.Generic;
using System.Threading.Tasks;
using Team.Model.AutoMappers.RunTeamMapper;
using Team.Model.Model.RunTeamModel;
using Team.Model.Model.UserModel;

namespace Team.Infrastructure.IRepositories
{
    /// <summary>
    /// 跑步队伍
    /// </summary>
    public interface IRunTeamResource
    {
        /// <summary>
        /// 创建队伍,true为创建成功，false为创建失败
        /// </summary>
        /// <param name="participants">参与者模板</param>
        /// <param name="runTeam">团队模板</param>
        /// <param name="user"></param>
        /// <returns></returns>
        bool TeamCreate(RunParticipants participants, RunTeam runTeam,User user);

        /// <summary>
        /// 删除自身创建的队伍,true为删除成功，false为删除失败
        /// </summary>
        /// <param name="userId">创建者 id</param>
        /// <returns></returns>
        bool DeleteTeamAsync(int userId);

        /// <summary>
        /// 更新队伍信息
        /// </summary>
        /// <param name="runTeam"></param>
        Task<bool> UpdateTeam(RunTeamUpdateMap runTeam);

        /// <summary>
        /// 查询所有正在组队的允许参加的队伍
        /// </summary>
        /// <returns></returns>
        IEnumerable<TeamList> RunTeamsSearchAll();

        /// <summary>
        /// 查询某个正在组队的团队信息
        /// </summary>
        /// <param name="teamId">团队 Id</param>
        /// <returns></returns>
        RunTeam TeamSearchByIdTeaming(int teamId);

        /// <summary>
        /// 申请参加队伍
        /// </summary>
        /// <param name="applicant">申请信息</param>
        /// <returns></returns>
        void ParticipateInTeam(RunApplicant applicant);

        /// <summary>
        /// 查询所有申请者信息
        /// </summary>
        /// <param name="teamId">申请者 Id</param>
        IEnumerable<RunApplicant> SearchAllParticipate(int teamId);

        /// <summary>
        /// 查询单个 申请者信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        RunApplicant SearchByIdApplicant(int UserId);

        /// <summary>
        /// 同意入队
        /// </summary>
        /// <param name="participants"></param>
        void AgreeWithTheTeam(RunParticipants participants);

        /// <summary>
        /// 离开队伍
        /// </summary>
        /// <param name="userId">用户 Id</param>
        /// <param name="teamId">队伍 Id</param>
        /// <returns></returns>
        void LeaveTheTeam(int userId, int teamId);

        /// <summary>
        /// 查询当前队伍排名 Id
        /// </summary>
        /// <param name="teamId">队伍 Id</param>
        /// <returns></returns>
        int LevelTeamById(int teamId);

        /// <summary>
        /// 查看当前学校队伍前一百名
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<RunTeam> LevelTeam(int userId);

        /// <summary>
        /// 修改 user 队伍 Id
        /// </summary>
        /// <param name="user"></param>
        /// <param name="teamId"></param>
        void UpdateUserRunTeamState(User user, int teamId);
    }
}
