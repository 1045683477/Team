using System;
using Team.Model.Model;

namespace Team.Model.AutoMappers
{
    /// <summary>
    /// 创建队伍模板
    /// </summary>
    public class TeamCreateMap
    {
        /// <summary>
        /// 运动类型
        /// </summary>
        public Sport Sport { get; set; }

        /// <summary>
        /// 参加总人数
        /// </summary>
        public int AllCount { get; set; }

        /// <summary>
        /// 备注地点
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 约定时间
        /// </summary>
        public DateTime AgreedTime { get; set; }
    }
}
