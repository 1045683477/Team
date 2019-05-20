using Team.Model.Model.UserModel;

namespace Team.Model.Model.TeamModel
{
    /// <summary>
    /// 参加者表
    /// </summary>
    public class Participants
    {
        /// <summary>
        /// 主键 Id
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
        /// 组队表外键
        /// </summary>
        public int TeamId { get; set; }

        public Team Team { get; set; }
    }
}
