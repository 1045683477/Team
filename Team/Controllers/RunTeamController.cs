using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Team.AuthHelper.OverWrite;
using Team.Infrastructure.IRepositories;
using Team.Model;
using Team.Model.AutoMappers.RunTeamMapper;
using Team.Model.Model.RunTeamModel;
using Team.Model.Model.UserModel;

namespace Team.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Client,Admin,SuperAdministrator")]
    public class RunTeamController:Controller
    {
        #region Initial

        private readonly IRunTeamResource _runTeamResource;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RunTeamController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtHelper _jwtHelper;

        public RunTeamController(
            IRunTeamResource runTeamResource,
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<RunTeamController> logger,
            IUnitOfWork unitOfWork,
            IJwtHelper jwtHelper)
        {
            _runTeamResource = runTeamResource;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _jwtHelper = jwtHelper;
        }

        #endregion

        /// <summary>
        /// 创建队伍，调用此api后调用UpdateUserRunTeamId
        /// </summary>
        /// <param name="runTeam">队伍模板</param>
        /// <returns></returns>
        [HttpPost("RunTeamCreateApi", Name = "RunTeamCreateApi")]
        public IActionResult RunTeamCreateApi([FromBody] RunTeamCreateMap runTeam)
        {
            CustomStatusCode code;
            var jwtStr = HttpRequest();
            var user = _userRepository.UserSearch(jwtStr.Id);
            var participants = new RunParticipants
            {
                UserId = user.Id,
                Name = user.Name,
                Sex = user.Sex
            };
            var team = _mapper.Map<RunTeam>(runTeam);
            var exits = _runTeamResource.TeamCreate(participants, team, user);

            if (exits == false)
            {
                _logger.LogInformation($"用户 {jwtStr.Id} 创建跑步队伍失败");
                code = new CustomStatusCode
                {
                    Status = "500",
                    Message = $"用户 {jwtStr.Id} 创建跑步队伍失败"
                };
                return StatusCode(500, code);
            }

            _logger.LogInformation($"用户 {jwtStr.Id} 创建跑步队伍成功");

            //var teamSearch = _mapper.Map<RunTeamSuccessCreateUserMapper>(user);

            code = new CustomStatusCode
            {
                Status = "201",
                Message = $"用户 {jwtStr.Id} 创建跑步队伍成功",
                //Data = teamSearch
            };
            return StatusCode(201, code);
        }

        /// <summary>
        /// 修改用户信息 RunTeamId 获取队伍 Id,请务必在调用RunTeamCreateApi之后使用
        /// </summary>
        /// <returns></returns>
        [HttpGet("UpdateUserRunTeamId",Name = "UpdateUserRunTeamId")]
        public IActionResult UpdateUserRunTeamId()
        {
            var jwtStr = HttpRequest();

            CustomStatusCode code;

            var user = _userRepository.UserSearch(jwtStr.Id);


            var teamById=_userRepository.TeamSearchByUserIdTeaming(jwtStr.Id);

            if (teamById.RunTeam.Id==Int32.MinValue)
            {
                _logger.LogInformation($"用户 {jwtStr.Id} 修改自己跑步队伍 Id 失败，还未加入队伍");

                code = new CustomStatusCode
                {
                    Status = 404,
                    Message = $"用户 {jwtStr.Id} 修改自己跑步队伍 Id 失败，还未加入队伍"
                };
                return StatusCode(404, code);
            }

            _runTeamResource.UpdateUserRunTeamState(user, user.RunTeam.Id);

            _logger.LogInformation($"用户 {jwtStr.Id} 修改自己跑步队伍 Id 成功");

            code = new CustomStatusCode
            {
                Status = 200,
                Message = $"用户 {jwtStr.Id} 修改自己跑步队伍 Id 成功",
                Data = $"队伍 Id {teamById.RunTeam.Id}"
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 删除自身创建的队伍
        /// </summary>
        /// <returns></returns>
        [HttpGet("DeleteTeamApi", Name = "DeleteTeamApi")]
        public IActionResult DeleteTeamApi()
        {
            CustomStatusCode code;
            var jwt = HttpRequest();
            var success = _runTeamResource.DeleteTeamAsync(jwt.Id);
            if (success.Result.Equals(false))
            {
                _logger.LogInformation($"用户 {jwt.Id} 自身并没有创建队伍，或者删除失败！");
                code=new CustomStatusCode
                {
                    Status = "404",
                    Message = $"用户 {jwt.Id} 自身并没有创建队伍，或者删除失败！"
                };
                return StatusCode(404, code);
            }
            _logger.LogInformation($"用户 {jwt.Id} 删除队伍成功");
            code = new CustomStatusCode
            {
                Status = "200",
                Message = $"用户 {jwt.Id} 删除队伍成功"
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 查看某个队伍信息
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpGet("SearchTeamApi",Name = "SearchTeamApi")]
        public IActionResult SearchTeamApi(int teamId)
        {
            CustomStatusCode code;
            var team = _runTeamResource.TeamSearchByIdTeaming(teamId);
            if (team==null)
            {
                _logger.LogInformation($"查询跑步团队 {teamId} 不存在");
                code=new CustomStatusCode
                {
                    Status = "404",
                    Message = $"查询跑步团队 {teamId} 不存在"
                };
                return StatusCode(404, code);
            }

            var teamModel = _mapper.Map<RunTeamSearch>(team);
            _logger.LogInformation($"查询跑步团队 {teamId} 成功");
            code = new CustomStatusCode
            {
                Status = "200",
                Message = $"查询跑步团队 {teamId} 成功",
                Data = teamModel
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 修改队伍信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateTeamApi",Name = "UpdateTeamApi")]
        public async Task<IActionResult> UpdateTeamApi([FromBody]RunTeamUpdateMap runTeam)
        {
            CustomStatusCode code;
            var jwt = HttpRequest();
            var user = _userRepository.UserSearch(jwt.Id);
            runTeam.TeamId = user.RunTeamId;
            var update=await _runTeamResource.UpdateTeam(runTeam);
            if (update == false)
            {
                _logger.LogInformation($"用户 {jwt.Id} 修改队伍 {runTeam.TeamId} 失败");
                code = new CustomStatusCode
                {
                    Status = "500",
                    Message = $"用户 {jwt.Id} 修改队伍 {runTeam.TeamId} 失败"
                };
                return StatusCode(500, code);
            }
            _logger.LogInformation($"用户 {jwt.Id} 修改队伍 {runTeam.TeamId} 失败");
            code = new CustomStatusCode
            {
                Status = "200",
                Message = $"用户 {jwt.Id} 修改队伍 {runTeam.TeamId} 成功"
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 解析 header 里面的 token
        /// </summary>
        /// <returns></returns>
        private TokenModelJWT HttpRequest()
        {
            var jwt = HttpContext.Request.Headers["Authorization"];
            jwt = jwt.ToString().Substring("Bearer ".Length).Trim();
            return _jwtHelper.SerizlizeJWT(jwt);
        }

        private void up(User user,int teamId)
        {
            _runTeamResource.UpdateUserRunTeamState(user, teamId);
        }
    }
}
