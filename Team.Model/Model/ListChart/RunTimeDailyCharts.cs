using System;

namespace Team.Model.Model.RunTeamModel
{
    /// <summary>
    /// 日排行榜
    /// </summary>
    public class RunTimeDailyCharts
    {
        public int Id { get; set; }

        /// <summary>
        /// 排名名词
        /// </summary>
        public int Ranking { get; set; }

        /// <summary>
        /// 队伍名
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// 队伍简介
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 跑步长度
        /// </summary>
        public float Distance { get; set; }

        /// <summary>
        /// 队伍 Id
        /// </summary>
        public int RunTeamId { get; set; }

        public RunTeam RunTeam { get; set; }
    }
}
