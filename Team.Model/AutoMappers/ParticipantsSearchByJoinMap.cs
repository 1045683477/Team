namespace Team.Model.AutoMappers
{
    public class ParticipantsSearchByJoinMap
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
        /// 组队表外键
        /// </summary>
        public int TeamId { get; set; }

        public TeamSearchMap Team { get; set; }
    }
}
