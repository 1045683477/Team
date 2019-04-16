using System.Collections.Generic;

namespace Team.Model.Model
{
    /// <summary>
    /// 用户资料
    /// </summary>
    public class User
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Images { get; set; }

        /// <summary>
        /// 性别，1为男，0为女
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// 身份
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public int Province { get; set; }

        /// <summary>
        /// 大学 Id
        /// </summary>
        public int UniversityId{ get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public int studentId { get; set; }

        /// <summary>
        /// 组队
        /// </summary>
        public ICollection<Team> Teams { get; set; }

        /// <summary>
        /// 跑步记录
        /// </summary>
        public ICollection<Run> Runs { get; set; }
    }
}
