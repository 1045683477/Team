using System;
using System.Collections.Generic;
using Team.Model.Model.RunTeamModel;
using Team.Model.Model.UserModel;

namespace Team.Model.AutoMappers.RunTeamMapper
{
    public class RunTeamSuccessCreateMapper
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
        /// 队长设置队伍状态，是否允许扩充,yes 1 允许，no 2不允许 ，默认允许
        /// </summary>
        public ApplicationStatus ApplicationStatus { get; set; }

        /// <summary>
        /// 参与者
        /// </summary>
        public ICollection<RunParticipants> RunParticipantses { get; set; }
    }
}
