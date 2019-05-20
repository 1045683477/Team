using System;
using Team.Model.Model.UserModel;

namespace Team.Model.Model.RunTeamModel
{
    /// <summary>
    /// 组队列表
    /// </summary>
    public class TeamList
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
        /// 队伍简介
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 总人数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
