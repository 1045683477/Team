using System;
using Team.Model.Model.TeamModel;

namespace Team.Model.AutoMappers.TeamMapper
{
    public class RunRecordMap
    {
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
        public float Calories { get; set; }

        /// <summary>
        /// 配速
        /// </summary>
        public float Speed { get; set; }
    }
}
