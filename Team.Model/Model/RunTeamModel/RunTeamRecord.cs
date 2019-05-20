using System;

namespace Team.Model.Model.RunTeamModel
{
    /// <summary>
    /// 团队跑步记录
    /// </summary>
    public class RunTeamRecord
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 当天时间
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 跑步长度
        /// </summary>
        public float Distance { get; set; }

        /// <summary>
        /// 消耗能量
        /// </summary>
        public float Calories { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        public int RunTeamId { get; set; }

        public RunTeam RunTeam { get; set; }
    }
}
