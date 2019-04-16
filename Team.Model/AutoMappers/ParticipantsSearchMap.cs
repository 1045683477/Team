namespace Team.Model.AutoMappers
{
    /// <summary>
    /// 参加模板
    /// </summary>
    public class ParticipantsSearchMap
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
    }
}
