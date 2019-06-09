using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using Team.Infrastructure.IRepositories;
using Team.Model;

namespace Team.Controllers
{
    [Route("api/[controller]")]
    public class ListController:Controller
    {
        #region Initial

        private readonly ILogger<ListController> _logger;
        private readonly IListResource _listResource;

        public ListController(
            ILogger<ListController> logger,
            IListResource listResource)
        {
            _logger = logger;
            _listResource = listResource;
        }

        #endregion

        /// <summary>
        /// 查询每日前一百战队榜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchDailyList",Name = "SearchDailyList")]
        public IActionResult SearchDailyList()
        {
            CustomStatusCode code;
            var daily = _listResource.SearchAllDailyCharts();
            if (!daily.Any())
            {
                code = new CustomStatusCode
                {
                    Status = "404",
                    Message = "获取每日前一百战队榜单成功,为空",
                    Data = daily
                };
                _logger.LogInformation("获取每日前一百战队榜单成功,为空");
                return StatusCode(404, code);
            }
            code = new CustomStatusCode
            {
                Status = "200",
                Message = "获取每日前一百战队榜单成功",
                Data = daily
            };
            _logger.LogInformation("获取每日前一百战队榜单成功");
            return StatusCode(200, code);
        }

        /// <summary>
        /// 查询每周前一百战队榜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchWeekList", Name = "SearchWeekList")]
        public IActionResult SearchWeekList()
        {
            CustomStatusCode code;
            var daily = _listResource.SearchAllWeekCharts();
            if (!daily.Any())
            {
                code = new CustomStatusCode
                {
                    Status = "404",
                    Message = "获取每周前一百战队榜单成功,为空",
                    Data = daily
                };
                _logger.LogInformation("获取每周前一百战队榜单成功,为空");
                return StatusCode(404, code);
            }
            code = new CustomStatusCode
            {
                Status = "200",
                Message = "获取每周前一百战队榜单成功",
                Data = daily
            };
            _logger.LogInformation("获取每周前一百战队榜单成功");
            return StatusCode(200, code);
        }
    }
}
