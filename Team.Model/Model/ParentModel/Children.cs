namespace Team.Model.Model.ParentModel
{
    /// <summary>
    /// 父母模式模板
    /// </summary>
    public class Children
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Account { get; set; }

        /// <summary>
        /// 关系
        /// </summary>
        public RelationShip RelationShip { get; set; }

        public int SonId { get; set; }

        public int ParentId { get; set; }
    }
}
