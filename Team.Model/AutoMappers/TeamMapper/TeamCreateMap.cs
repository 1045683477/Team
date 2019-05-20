using System;
using Team.Model.Model;
using Team.Model.Model.TeamModel;

namespace Team.Model.AutoMappers.TeamMapper
{
    /// <summary>
    /// 创建队伍模板
    /// </summary>
    public class TeamCreateMap
    {
        /// <summary>
        /// 队伍名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 运动类型
        /// </summary>
        public Sport Sport { get; set; }

        /// <summary>
        /// 参加总人数
        /// </summary>
        public int AllCount { get; set; }

        /// <summary>
        /// 地址（自己输入地址）
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 队伍简介
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 约定时间
        /// </summary>
        public DateTime AgreedTime { get; set; }
    }
}
