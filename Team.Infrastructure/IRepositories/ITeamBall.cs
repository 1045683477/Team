using System.Collections.Generic;
using System.Threading.Tasks;
using Team.Model.AutoMappers;
using Team.Model.Model;

namespace Team.Infrastructure.IRepositories
{
    /// <summary>
    /// 队伍
    /// </summary>
    public interface ITeamBall
    {
        /// <summary>
        /// 创建队伍,true为创建成功，false为创建失败
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="team">创建模板</param>
        /// <returns></returns>
        bool TeamCreate(Participants participants, Model.Model.Team team);

        /// <summary>
        /// 查询该用户所有创建记录
        /// </summary>
        /// <param name="userId">用户 Id</param>
        /// <returns></returns>
        IEnumerable<Model.Model.Team> TeamSearchUserCreateAll(int userId);

        /// <summary>
        /// 查询该用户所有参加记录
        /// </summary>
        /// <param name="userId">用户 Id</param>
        /// <returns></returns>
        IEnumerable<Participants> TeamSearchUserJoinAll(int userId);

        /// <summary>
        /// 查询某个运动所有正在组队队伍
        /// </summary>
        /// <param name="sport">运动类型</param>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        IEnumerable<Model.Model.Team> TeamSearchTeaming(Sport sport,int userId);

        /// <summary>
        /// 查询某个正在组队的团队信息
        /// </summary>
        /// <param name="teamId">团队 Id</param>
        /// <returns></returns>
        Model.Model.Team TeamSearchByIdTeaming(int teamId);

        /// <summary>
        /// 参加队伍
        /// </summary>
        /// <param name="participants">参加者模板</param>
        /// <param name="teamId">队伍 Id</param>
        /// <returns></returns>
        bool ParticipateInTeam(User user, int teamId);

        /// <summary>
        /// 通过队伍名查询正在组队团队
        /// </summary>
        /// <param name="name">队伍名</param>
        /// <param name="userId">用户 ID</param>
        /// <returns></returns>
        Model.Model.Team TeamSearchByName(string name,int userId);
    }
}
