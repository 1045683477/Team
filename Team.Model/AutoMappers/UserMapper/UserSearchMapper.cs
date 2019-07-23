using Team.Model.Model.UserModel;

namespace Team.Model.AutoMappers.UserMapper
{
    public class UserSearchMapper
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
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别，1为男，0为女
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public int Province { get; set; }

        /// <summary>
        /// 大学 Id
        /// </summary>
        public int UniversityId { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// 跑步队伍 Id
        /// </summary>
        public int RunTeamId { get; set; }
    }
}
