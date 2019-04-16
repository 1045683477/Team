using System;

namespace Team.Model.Model
{
    /// <summary>
    /// 跑步记录
    /// </summary>
    public class Run
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 自由模式
        /// </summary>
        public SportFreeModel SportFreeModel { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StarTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 开始地点
        /// </summary>
        public string StarPlace { get; set; }

        /// <summary>
        /// 结束地点
        /// </summary>
        public string EndPlace { get; set; }

        /// <summary>
        /// 跑步长度
        /// </summary>
        public float Distance { get; set; }

        /// <summary>
        /// 消耗能量
        /// </summary>
        public float Kcal { get; set; }

        /// <summary>
        /// 配速
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
