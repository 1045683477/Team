namespace Team.Model.Model.RunTeamModel
{
    /// <summary>
    /// 周排行榜
    /// </summary>
    public class RunTimeWeekCharts
    {
        public int Id { get; set; }

        /// <summary>
        /// 队伍 Id
        /// </summary>
        public int TeamId { get; set; }

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
        /// 打卡人数总和
        /// </summary>
        public int ClockIn { get; set; }
    }
}
