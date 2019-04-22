using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Team.AuthHelper.OverWrite;
using Team.Infrastructure.DbContext;
using Team.Infrastructure.IRepositories;
using Team.Model;
using Team.Model.AutoMappers;
using Team.Model.Model;

namespace Team.Controllers
{
    /// <summary>
    /// 跑步 记录与查询
    /// </summary>
    [ApiController]
    [Authorize(Roles = "Client,Admin,SuperAdministrator")]
    [Route("api/[controller]")]
    public class RunController:Controller
    {
        #region Initial

        private readonly MyContext _myContext;
        private readonly IRunRepository _runRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<RunController> _logger;
        private readonly IJwtHelper _jwtHelper;

        public RunController(
            MyContext myContext,
            IRunRepository runRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<RunController> logger,
            IJwtHelper jwtHelper)
        {
            _myContext = myContext;
            _runRepository = runRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _jwtHelper = jwtHelper;
        }

        #endregion

        /// <summary>
        /// 跑步数据记录
        /// </summary>
        /// <param name="recordMap">记录模板</param>
        /// <returns></returns>
        [HttpPost("FreeRecordApi",Name = "FreeRecordApi")]
        public async Task<IActionResult> FreeRecordApi(RunRecordMap recordMap)
        {
            CustomStatusCode code;
            var run = _mapper.Map<Run>(recordMap);
            var user = HttpRequest();
            _runRepository.FreeRecord(user.Id,run);
            if (!await _unitOfWork.SaveChanged())
            {
                _logger.LogError($"{user.Id} 跑步保存失败");
                code=new CustomStatusCode
                {
                    Status = "500",
                    Message = $"{user.Id} 跑步保存失败"
                };
                return StatusCode(500, code);
            }
            _logger.LogInformation($"{user.Id} 跑步保存成功");
            code=new CustomStatusCode
            {
                Status = "200",
                Message = $"{user.Id} 跑步保存成功"
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 查询跑步记录统计
        /// </summary>
        /// <param name="sport"></param>
        /// <returns></returns>
        [HttpPost("FreeAllStatisticalApi",Name = "FreeAllStatisticalApi")]
        public IActionResult FreeAllStatisticalApi([FromBody] SportFreeModel sport)
        {
            CustomStatusCode code;
            var user = HttpRequest();
            var record = _runRepository.FreeAllStatistical(user.Id, sport);
            if (record==null)
            {
                _logger.LogInformation($"{user.Id} 用户查询跑步记录统计为空");
                code=new CustomStatusCode
                {
                    Status = "200",
                    Message = $"{user.Id} 用户查询跑步记录统计为空"
                };
                return StatusCode(200, code);
            }
            _logger.LogInformation($"{user.Id} 用户查询跑步记录统计成功");
            var resource = _mapper.Map<StatisticalMap>(record);
            code = new CustomStatusCode
            {
                Status = "200",
                Message = $"{user.Id} 用户查询跑步记录统计成功",
                Data = resource
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 查询所有跑步记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("FreeSearchApi",Name = "FreeSearchApi")]
        public IActionResult FreeSearchApi()
        {
            CustomStatusCode code;
            var jwtStr = HttpRequest();
            var runList = _runRepository.FreeSearch(jwtStr.Id);
            if (runList==null)
            {
                _logger.LogInformation($"{jwtStr.Id} 查询所有跑步记录为空");
                code=new CustomStatusCode
                {
                    Status = "404",
                    Message = $"{jwtStr.Id} 查询所有跑步记录为空"
                };
                return StatusCode(404, code);
            }
            var runResource = _mapper.Map<IEnumerable<RunRecordMap>>(runList);
            _logger.LogInformation($"{jwtStr.Id} 查询所有跑步成功");
            code = new CustomStatusCode
            {
                Status = "200",
                Message = $"{jwtStr.Id} 查询所有跑步成功",
                Data = runResource
            };
            return StatusCode(404, code);
        }

        /// <summary>
        /// 体育锻炼及格次数与记录 要求：4000km 跑步模式
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExeExerciseApi",Name = "ExeExerciseApi")]
        public IActionResult ExeExerciseApi()
        {
            CustomStatusCode code;
            var user = HttpRequest();
            var run= _runRepository.ExerciseList(user.Id);
            if (!run.Any())
            {
                _logger.LogInformation($"{user.Id} 查询体育锻炼次数与记录为空");
                code=new CustomStatusCode
                {
                    Status = "200",
                    Message = $"{user.Id} 未有合格记录"
                };
                return StatusCode(200, code);
            }
            var runResource = _mapper.Map<IEnumerable<RunRecordMap>>(run);
            _logger.LogInformation($"{user.Id} 查询体育锻炼次数与记录");
            var runRecordMaps = runResource as RunRecordMap[] ?? runResource.ToArray();
            code = new CustomStatusCode
            {
                Status = "200",
                Message = $"{user.Id} 查询体育锻炼次数与记录",
                Data = new {count=runRecordMaps.Count(),data= runRecordMaps}
            };
            return StatusCode(200, code);
        }


        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        private TokenModelJWT HttpRequest()
        {
            var jwtStr = HttpContext.Request.Headers["Authorization"];
            jwtStr = jwtStr.ToString().Substring("Bearer ".Length).Trim();
            return _jwtHelper.SerizlizeJWT(jwtStr);
        }
    }
}
