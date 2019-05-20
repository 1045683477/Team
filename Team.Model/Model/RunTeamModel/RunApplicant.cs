using System;
using Team.Model.Model.UserModel;

namespace Team.Model.Model.RunTeamModel
{
    /// <summary>
    /// 申请者名单
    /// </summary>
    public class RunApplicant
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 参加者 Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplicationDate { get; set; }

        /// <summary>
        /// 组队表外键
        /// </summary>
        public int RunTeamId { get; set; }

        public RunTeam RunTeam { get; set; }
    }
}
