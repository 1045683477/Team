namespace Team.Model.AutoMappers.UserMapper
{
    /// <summary>
    /// 查看经纬度模板
    /// </summary>
    public class LAndLSearchMapper
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 距离
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// 通信编号
        /// </summary>
        public string CommunicationId { get; set; }
    }
}
