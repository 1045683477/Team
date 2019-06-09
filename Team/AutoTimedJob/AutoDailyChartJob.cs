using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pomelo.AspNetCore.TimedJob;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Team.Infrastructure.DbContext;

namespace Team.AutoTimedJob
{
    public class AutoDailyChartJob:Job
    {
        #region Initial

        private readonly MyContext _myContext;
        private readonly ILogger<AutoDailyChartJob> _logger;

        public AutoDailyChartJob(
            MyContext myContext,
            ILogger<AutoDailyChartJob> logger
        )
        {
            _myContext = myContext;
            _logger = logger;
        }

        #endregion

        /// <summary>
        /// 更新跑团每日排行榜
        /// </summary>
        [Invoke(Begin = "2019-05-28 00:00", Interval = 1000 * 60*30)]//, SkipWhileExecuting = true
        public Task UpdateRunTimeDailyChart()
        {
            _myContext.Database.ExecuteSqlCommand("delete RunTimeDailyChartses;");

            int Ranking = 1;
            var result = from r in _myContext.RunTeamRecords
                         from b in _myContext.RunTeams
                         where b.Id == r.RunTeamId && r.DateTime == DateTime.Today
                         orderby r.Distance descending
                         select new { r.RunTeamId, b.Name, b.Introduction, r.Distance };


            foreach (var a in result.Take(100))
            {
                SqlParameter[] sql = new[]
                {
                    new SqlParameter("Ranking", Ranking),
                    new SqlParameter("TeamName", a.Name),
                    new SqlParameter("Introduction", a.Introduction),
                    new SqlParameter("Distance", a.Distance),
                    new SqlParameter("RunTeamId", a.RunTeamId),
                };
                _myContext.Database.ExecuteSqlCommand("insert into RunTimeDailyChartses (Ranking,TeamName,Introduction,Distance,RunTeamId) values(@Ranking,@TeamName,@Introduction,@Distance,@RunTeamId);", sql);
                Ranking++;
            }
            _myContext.Database.CloseConnection();
            _logger.LogInformation($"时间 {DateTime.Now} 更新跑团每日排行榜");
            return Task.CompletedTask;
        }
    }
}
