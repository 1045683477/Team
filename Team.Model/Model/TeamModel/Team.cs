using System;
using System.Collections.Generic;
using Team.Model.Model.UserModel;

namespace Team.Model.Model.TeamModel
{
    /// <summary>
    /// 历史约球记录
    /// </summary>
    public class Team
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 队伍名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 创建队伍时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 约定时间
        /// </summary>
        public DateTime AgreedTime { get; set; }

        /// <summary>
        /// 地址（自己输入地址）
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 队伍简介
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 总人数
        /// </summary>
        public int AllCount { get; set; }

        /// <summary>
        /// 当前数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 运动类型
        /// </summary>
        public Sport Sport { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<Participants> Participantses { get; set; }
    }
}
