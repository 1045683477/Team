using System.Collections.Generic;
using Team.Model.Model;

namespace Team.Infrastructure.IRepositories
{
    /// <summary>
    /// Free Run 服务
    /// </summary>
    public interface IRunRepository
    {
        /// <summary>
        /// 记录跑步
        /// </summary>
        /// <param name="userId">用户 Id</param>
        /// <param name="run"></param>
        void FreeRecord(int userId,Run run);

        /// <summary>
        /// 查询所有记录
        /// </summary>
        /// <param name="userId">用户 Id</param>
        /// <returns></returns>
        IEnumerable<Run> FreeSearch(int userId);

        /// <summary>
        /// 达标记录
        /// </summary>
        /// <param name="userId">用户 Id</param>
        /// <returns></returns>
        IEnumerable<Run> ExerciseList(int userId);
    }
}
