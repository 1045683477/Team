﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team.AuthHelper.OverWrite;
using Team.Infrastructure.IRepositories;
using Team.Model;
using Team.Model.AutoMappers;
using Team.Model.Model;

namespace Team.Controllers
{
    [ApiController]
    [Authorize(Roles = "Client,Admin,SuperAdministrator")]
    [Route("api/[controller]")]
    public class TeamController:Controller
    {
        #region Initial

        private readonly ITeamBall _teamBall;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtHelper _jwtHelper;
        private readonly ILogger<TeamController> _logger;
        private readonly IMapper _mapper;

        public TeamController(
            ITeamBall teamBall,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IJwtHelper jwtHelper,
            ILogger<TeamController> logger,
            IMapper mapper)
        {
            _teamBall = teamBall;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _jwtHelper = jwtHelper;
            _logger = logger;
            _mapper = mapper;
        }

        #endregion

        /// <summary>
        /// 创建运动队伍
        /// </summary>
        /// <param name="teamCreate">创建队伍模板</param>
        /// <returns></returns>
        [HttpPost("TeamCreateApi",Name = "TeamCreateApi")]
        public async Task<IActionResult> TeamCreateApi([FromBody]TeamCreateMap teamCreate)
        {
            CustomStatusCode code;
            var jwtStr = HttpRequest();
            var user=_userRepository.UserSearch(jwtStr.Id);
            var participants=new Participants
            {
                UserId = user.Id,
                Name = user.Name
            };
            var team = _mapper.Map<Model.Model.Team>(teamCreate);
            bool changed = _teamBall.TeamCreate(participants,team);
            if (changed==false)
            {
                _logger.LogError($"用户 {jwtStr.Id} 创建队伍失败");
                code=new CustomStatusCode
                {
                    Status = "500",
                    Message = $"用户 {jwtStr.Id} 创建队伍失败"
                };
                return StatusCode(500, code);
            }

            if (!await _unitOfWork.SaveChanged())
            {
                _logger.LogError($"用户 {jwtStr.Id} 保存队伍失败");
                code = new CustomStatusCode
                {
                    Status = "500",
                    Message = $"用户 {jwtStr.Id} 保存队伍失败"
                };
                return StatusCode(500, code);
            }

            _logger.LogInformation($"用户 {jwtStr.Id} 创建队伍成功");
            code =new CustomStatusCode
            {
                Status = "200",
                Message = $"用户 {jwtStr.Id} 创建队伍成功"
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 查询用户自身所有创建队伍
        /// </summary>
        /// <returns></returns>
        [HttpGet("TeamSearchUserCreateAllApi", Name = "TeamSearchUserCreateAllApi")]
        public IActionResult TeamSearchUserCreateAllApi()
        {
            CustomStatusCode code;
            var jwtStr = HttpRequest();
            var team = _teamBall.TeamSearchUserCreateAll(jwtStr.Id);
            if (!team.Any())
            {
                _logger.LogInformation($"用户 {jwtStr.Id} 查询自身队伍结果为空");
                code = new CustomStatusCode
                {
                    Status = "404",
                    Message = $"用户 {jwtStr.Id} 查询自身队伍结果为空"
                };
                return StatusCode(404, code);
            }
            var teamSearch = _mapper.Map<IEnumerable<TeamSearchMap>>(team);

            _logger.LogInformation($"用户 {jwtStr.Id} 查询自身队伍成功");
            code = new CustomStatusCode
            {
                Status = "200",
                Message = $"用户 {jwtStr.Id} 查询自身队伍结果成功",
                Data = teamSearch
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 查询该用户所有参加记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("TeamSearchUserJoinAllApi",Name = "TeamSearchUserJoinAllApi")]
        public IActionResult TeamSearchUserJoinAllApi()
        {
            CustomStatusCode code;
            var jwtStr = HttpRequest();
            var ball = _teamBall.TeamSearchUserJoinAll(jwtStr.Id);
            if (ball==null)
            {
                _logger.LogInformation($"用户 {jwtStr.Id} 查询所有参加记录为空");
                code=new CustomStatusCode
                {
                    Status = "404",
                    Message = $"用户 {jwtStr.Id} 查询所有参加记录为空"
                };
                return StatusCode(404, code);
            }
            var source = _mapper.Map<IEnumerable<ParticipantsSearchByJoinMap>>(ball);
            _logger.LogInformation($"用户 {jwtStr.Id} 查询所有参加记录");
            code = new CustomStatusCode
            {
                Status = "200",
                Message = $"用户 {jwtStr.Id} 查询所有参加记录",
                Data = source
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 查询某个运动所有正在组队队伍,查询同校队伍
        /// </summary>
        /// <param name="sport">运动类型</param>
        /// <returns></returns>
        [HttpPost("TeamSearchTeamingApi",Name = "TeamSearchTeamingApi")]
        public IActionResult TeamSearchTeamingApi(Sport sport)
        {
            CustomStatusCode code;
            var jwtStr = HttpRequest();
            var teams = _teamBall.TeamSearchTeaming(sport,jwtStr.Id);
            if (!teams.Any())
            {
                _logger.LogInformation($"{sport} 运动没有队伍");
                code=new CustomStatusCode
                {
                    Status = "404",
                    Message = $"{sport} 运动没有队伍"
                };
                return StatusCode(404, code);
            }

            var source = _mapper.Map<IEnumerable<TeamSearchMap>>(teams);

            code=new CustomStatusCode
            {
                Status = "200",
                Message = $"{sport} 运动没有队伍",
                Data = source
            };
            return StatusCode(200, code);
        }


        /// <summary>
        /// 查询某个正在组队的团队信息
        /// </summary>
        /// <param name="teamId">团队 Id</param>
        /// <returns></returns>
        [HttpPost("TeamSearchByIdTeamingApi",Name = "TeamSearchByIdTeamingApi")]
        public IActionResult TeamSearchByIdTeamingApi(int teamId)
        {
            CustomStatusCode code;
            var teams = _teamBall.TeamSearchByIdTeaming(teamId);
            if (teams==null)
            {
                code=new CustomStatusCode
                {
                    Status = "404",
                    Message = "抱歉没有查到该团队"
                };
                return StatusCode(404, code);
            }
            var resource=_mapper.Map<TeamSearchMap>(teams);
            code=new CustomStatusCode
            {
                Status = "200",
                Message = "查询团队成功",
                Data = resource
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 参加队伍
        /// </summary>
        /// <param name="teamId">队伍 Id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ParticipateInTeamApi(int teamId)
        {
            CustomStatusCode code;
            var jwtStr = HttpRequest();
            var user = _userRepository.UserSearch(jwtStr.Id);
            bool changed = _teamBall.ParticipateInTeam(user, teamId);
            if (changed==false)
            {
                _logger.LogInformation($"用户 {jwtStr.Id} 参加队伍 {teamId} 失败");
                code=new CustomStatusCode
                {
                    Status = "409",
                    Message = $"用户 {jwtStr.Id} 参加队伍 {teamId} 失败"
                };
                return StatusCode(409, code);
            }

            if (!await _unitOfWork.SaveChanged())
            {
                _logger.LogError($"用户 {jwtStr.Id} 参加队伍 {teamId} 保存失败");
                code = new CustomStatusCode
                {
                    Status = "500",
                    Message = $"用户 {jwtStr.Id} 参加队伍 {teamId} 失败"
                };
                return StatusCode(500, code);
            }
            _logger.LogInformation($"用户 {jwtStr.Id} 参加队伍 {teamId} 成功");
            code = new CustomStatusCode
            {
                Status = "200",
                Message = $"用户 {jwtStr.Id} 参加队伍 {teamId} 成功"
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 解析 header 里面的 token
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