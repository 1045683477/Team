using System;
using Team.Model.Model.UserModel;

namespace Team.Model.AutoMappers.RunTeamMapper
{
    /// <summary>
    /// 申请者模板
    /// </summary>
    public class RunApplicantMapper
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
    }
}
