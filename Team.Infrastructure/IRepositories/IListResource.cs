using System.Collections.Generic;
using Team.Model.Model.RunTeamModel;

namespace Team.Infrastructure.IRepositories
{
    public interface IListResource
    {
        /// <summary>
        /// 查询每日前一百战队榜单
        /// </summary>
        /// <returns></returns>
        IEnumerable<RunTimeDailyCharts> SearchAllDailyCharts();

        /// <summary>
        /// 查询每周榜单
        /// </summary>
        /// <returns></returns>
        IEnumerable<RunTimeWeekCharts> SearchAllWeekCharts();
    }
}
