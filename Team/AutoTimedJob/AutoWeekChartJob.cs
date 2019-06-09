using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pomelo.AspNetCore.TimedJob;
using Team.Infrastructure.DbContext;

namespace Team.AutoTimedJob
{
    public class AutoWeekChartJob : Job
    {
        #region Initial

        private readonly MyContext _myContext;
        private readonly ILogger<AutoWeekChartJob> _logger;
        private static IConfiguration _configuration;

        public AutoWeekChartJob(
            MyContext myContext,
            ILogger<AutoWeekChartJob> logger,
            IConfiguration configuration
        )
        {
            _myContext = myContext;
            _logger = logger;
            _configuration = configuration;
        }

        #endregion

        /// <summary>
        /// 将每日跑团的信息加入周排行缓冲表中
        /// </summary>
        [Invoke(Begin = "2019-05-26 23:55", Interval = 1000 * 60 * 60*24)]//, SkipWhileExecuting = true
        public Task UpdateRunTimeWeekBufferChart()
        {
            var daily = from s in _myContext.RunTimeDailyChartses select s;

            foreach (var charts in daily)
            {
                SqlParameter[] sql = new[]
                {
                    new SqlParameter("TeamName", charts.TeamName),
                    new SqlParameter("Introduction", charts.Introduction),
                    new SqlParameter("Distance", charts.Distance),
                    new SqlParameter("RunTeamId", charts.RunTeamId),
                };
                var update=_myContext.Database.ExecuteSqlCommand(
                    "update RunTeamWeekChartBuffers set Distance+=@Distance where RunTeamId=@RunTeamId", sql);
                if (update==0)
                {
                    _myContext.Database.ExecuteSqlCommand(
                        "insert into RunTeamWeekChartBuffers values(@TeamName,@Introduction,@Distance,@RunTeamId)", sql);
                }
            }

            _myContext.Database.CloseConnection();
            _logger.LogInformation($"时间 {DateTime.Now} 更新跑团每周排行缓冲榜单数据");
            return Task.CompletedTask;
        }

        /// <summary>
        /// 刷新周榜单
        /// </summary>
        /// <returns></returns>
        [Invoke(Begin = "2019-05-26 00:00", Interval = 1000 * 60 * 60 * 24*7)]//, SkipWhileExecuting = true
        public Task UpdateRunTimeWeekChart()
        {
            //删除上次数据
            _myContext.Database.ExecuteSqlCommand("delete RunTimeWeekChartses");

            int Ranking = 0;

            //排序
            var week = from s in _myContext.RunTeamWeekChartBuffers
                       orderby s.Distance descending 
                        select s;



            foreach (var charts in week)
            {
                Ranking++;
                SqlParameter[] sql = new[]
                {
                    new SqlParameter("TeamName", charts.TeamName),
                    new SqlParameter("Ranking", Ranking),
                    new SqlParameter("Introduction", charts.Introduction),
                    new SqlParameter("Distance", charts.Distance),
                    new SqlParameter("RunTeamId", charts.RunTeamId),
                };
                _myContext.Database.ExecuteSqlCommand(
                    "insert into RunTimeWeekChartses values(@Ranking,@TeamName,@Introduction,@Distance,@RunTeamId)", sql);
            }

            _myContext.Database.ExecuteSqlCommand("delete delete RunTeamWeekChartBuffers;");
            _myContext.Database.CloseConnection();
            _logger.LogInformation($"时间 {DateTime.Now} 更新跑团每周排行榜");
            return Task.CompletedTask;
        }
    }
}
