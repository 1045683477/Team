using Team.Model.Model.RunTeamModel;

namespace Team.Model.AutoMappers.RunTeamMapper
{
    public class RunTeamSuccessCreateUserMapper
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 跑步队伍 Id
        /// </summary>
        public int RunTeamId { get; set; }

        public RunTeamSuccessCreateMapper RunTeam { get; set; }
    }
}
