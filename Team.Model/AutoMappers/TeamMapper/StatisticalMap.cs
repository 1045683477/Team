using Team.Model.Model.TeamModel;

namespace Team.Model.AutoMappers.TeamMapper
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
        public float Calories { get; set; }

        /// <summary>
        /// 总消耗时间
        /// </summary>
        public float AllTime { get; set; }
    }
}
