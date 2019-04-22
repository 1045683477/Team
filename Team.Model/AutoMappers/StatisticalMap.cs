using System;
using Team.Model.Model;

namespace Team.Model.AutoMappers
{
    public class StatisticalMap
    {
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
    }
}
