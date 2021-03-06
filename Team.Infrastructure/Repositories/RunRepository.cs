﻿using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using Team.Infrastructure.DbContext;
using Team.Infrastructure.IRepositories;
using Team.Model.Model.RunTeamModel;
using Team.Model.Model.TeamModel;

namespace Team.Infrastructure.Repositories
{
    public class RunRepository : IRunRepository
    {
        #region Initial

        private readonly MyContext _myContext;
        private readonly IUserRepository _userRepository;

        public RunRepository(
            MyContext myContext,
            IUserRepository userRepository)
        {
            _myContext = myContext;
            _userRepository = userRepository;
        }

        #endregion

        /// <summary>
        /// 记录跑步
        /// </summary>
        /// <param name="userId">用户 Id</param>
        /// <param name="run"></param>
        public void FreeRecord(int userId, Run run)
        {
            var user = _myContext.Statisticals.SingleOrDefault(x => x.UserId == userId&& x.SportFreeModel==run.SportFreeModel);
            user.AllTime += (float)(run.EndTime - run.StarTime).TotalMinutes;
            user.Distance += run.Distance;
            user.Calories += run.Calories;
            run.UserId = userId;
            _myContext.Runs.Add(run);
            if (run.SportFreeModel==SportFreeModel.Running)
            {
                RunTeamRecord(userId, run);
            }
            
        }

        /// <summary>
        /// 查询跑步记录统计
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sport"></param>
        /// <returns></returns>
        public Statistical FreeAllStatistical(int userId,SportFreeModel sport)
        {
            return _myContext.Statisticals.SingleOrDefault(x => x.SportFreeModel == sport && x.UserId == userId);
        }

        /// <summary>
        /// 查询所有记录
        /// </summary>
        /// <param name="userId">用户 Id</param>
        /// <returns></returns>
        public IEnumerable<Run> FreeSearch(int userId)
        {
            return _myContext.Runs.Where(x => x.UserId == userId).ToList();
        }

        /// <summary>
        /// 达标记录
        /// </summary>
        /// <param name="userId">用户 Id</param>
        /// <returns></returns>
        public IEnumerable<Run> ExerciseList(int userId)
        {
            return _myContext.Runs.Where(x =>
                x.UserId == userId && x.SportFreeModel == SportFreeModel.Running && x.Distance >= 4000).ToList();
        }

        /// <summary>
        /// 记录团队部署
        /// </summary>
        private void RunTeamRecord(int userId, Run run)
        {
            var user = _userRepository.UserSearch(userId);
            //先判断是否参团
            if (user.RunTeamId!=0)
            {
                //判断今日队伍数据是否存在
                var exit = from s in _myContext.RunTeamRecords
                    where s.DateTime == DateTime.Today && s.RunTeamId == user.RunTeamId
                    select s;
                //不存在
                if (!exit.Any())
                {
                    var record = new RunTeamRecord
                    {
                        Calories = run.Calories,
                        DateTime = DateTime.Now,
                        Distance = run.Distance,
                        RunTeamId = user.RunTeamId,
                    };
                    _myContext.RunTeamRecords.Add(record);
                }
                else
                {
                    //存在
                    var one = exit.FirstOrDefault();
                    one.Calories += run.Calories;
                    one.Distance += run.Distance;
                    _myContext.RunTeamRecords.Update(one);
                }
                
            }
        }
    }
}
