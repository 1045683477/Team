namespace Team.Model.Model.RunTeamModel
{
    /// <summary>
    /// 每周刷新排行榜的缓存表
    /// </summary>
    public class RunTeamWeekChartBuffer
    {
        public int Id { get; set; }

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
