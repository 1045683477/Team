using System;

namespace Team.Model.Model
{
    /// <summary>
    /// 统计总路程、卡路里、时间
    /// </summary>
    public class Statistical
    {
        public int Id { get; set; }

        /// <summary>
        /// 跑步项目
        /// </summary>
        public SportFreeModel SportFreeModel { get; set; }

        /// <summary>
        /// 总路程
        /// </summary>
        public float Distance { get; set; }

        /// <summary>
        /// 总消耗卡路里
        /// </summary>
        public float Kcal { get; set; }

        /// <summary>
        /// 总消耗时间
        /// </summary>
        public float AllTime { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
