namespace Team.Model.Model.UserModel
{
    /// <summary>
    /// 经纬度
    /// </summary>
    public class LatitudeAndLongitude
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

    }
}
