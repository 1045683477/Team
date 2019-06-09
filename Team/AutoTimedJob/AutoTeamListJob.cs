using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pomelo.AspNetCore.TimedJob;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Team.Infrastructure.DbContext;
using Team.Model.Model.RunTeamModel;

namespace Team.AutoTimedJob
{
    public class AutoTeamListJob : Job
    {
        #region Initial

        private readonly MyContext _myContext;
        private readonly ILogger<AutoTeamListJob> _logger;

        public AutoTeamListJob(
            MyContext myContext,
            ILogger<AutoTeamListJob> logger
        )
        {
            _myContext = myContext;
            _logger = logger;
        }

        #endregion


        /// <summary>
        /// 跑团组队列表每十分钟更新一次
        /// </summary>
        /// <returns></returns>
        [Invoke(Begin = "2019-05-28 00:00", Interval = 1000 *60*10)]//, SkipWhileExecuting = true
        public Task UpdateRunTeamList()
        {
            _myContext.Database.ExecuteSqlCommand("delete TeamLists;");

            var runTeams = from s in _myContext.RunTeams
                           where s.ApplicationStatus == ApplicationStatus.Yes && s.Count>5
                           select s;

            
            foreach (var a in runTeams)
            {
                SqlParameter[] sql = new[]
                {
                    new SqlParameter("Name", a.Name),
                    new SqlParameter("CreationTime", a.CreationTime),
                    new SqlParameter("Introduction", a.Introduction),
                    new SqlParameter("Count", a.Count),
                    new SqlParameter("UserId", a.UserId)
                };
                var ee = _myContext.Database.ExecuteSqlCommand("insert into TeamLists values(@Name,@CreationTime,@Introduction,@Count,@UserId);", sql);
            }

            _myContext.Database.CloseConnection();
            _logger.LogInformation($"时间 {DateTime.Now} 更新跑团组队列表");
            return Task.CompletedTask;
        }

        
    }
}
