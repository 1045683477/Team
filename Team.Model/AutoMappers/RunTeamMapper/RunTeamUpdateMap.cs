using Team.Model.Model.RunTeamModel;

namespace Team.Model.AutoMappers.RunTeamMapper
{
    /// <summary>
    /// 跑步队伍修改模板
    /// </summary>
    public class RunTeamUpdateMap
    {
        /// <summary>
        /// 队伍 Id 不需要填，后端帮你搞定
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// 队伍名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 队伍简介
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 队长设置队伍状态，是否允许扩充,yes 1 允许，no 2不允许 ，默认允许
        /// </summary>
        public ApplicationStatus ApplicationStatus { get; set; }
    }
}
