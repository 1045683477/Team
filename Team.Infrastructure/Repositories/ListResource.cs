using System.Collections.Generic;
using System.Linq;
using Team.Infrastructure.DbContext;
using Team.Infrastructure.IRepositories;
using Team.Model.Model.RunTeamModel;

namespace Team.Infrastructure.Repositories
{
    public class ListResource: IListResource
    {
        #region Initial

        private readonly MyContext _myContext;

        public ListResource(
            MyContext myContext)
        {
            _myContext = myContext;
        }

        #endregion
        

        public IEnumerable<RunTimeDailyCharts> SearchAllDailyCharts()
        {
            return from s in _myContext.RunTimeDailyChartses select s;
        }

        public IEnumerable<RunTimeWeekCharts> SearchAllWeekCharts()
        {
            return from chartse in _myContext.RunTimeWeekChartses select chartse;
        }
    }
}
