using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Team.AuthHelper.OverWrite;
using Team.Infrastructure.IRepositories;
using Team.Model;
using Team.Model.AutoMappers.RunTeamMapper;
using Team.Model.Model.RunTeamModel;

namespace Team.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Client,Captain,Admin,SuperAdministrator")]
    //[Authorize(Policy = "ClientOrCaptain")]
    public class RunTeamController:Controller
    {
        #region Initial

        private readonly IRunTeamResource _runTeamResource;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RunTeamController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtHelper _jwtHelper;
        private readonly IImagesResource _imagesResource;

        public RunTeamController(
            IRunTeamResource runTeamResource,
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<RunTeamController> logger,
            IUnitOfWork unitOfWork,
            IJwtHelper jwtHelper,
            IImagesResource imagesResource)
        {
            _runTeamResource = runTeamResource;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _jwtHelper = jwtHelper;
            _imagesResource = imagesResource;
        }

        #endregion

        /// <summary>
        /// 创建队伍，调用此api后调用UpdateUserRunTeamId  唯有非队长才可调用此api
        /// </summary>
        /// <param name="runTeam">队伍模板</param>
        /// <returns></returns>
        [HttpPost("RunTeamCreateApi", Name = "RunTeamCreateApi")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> RunTeamCreateApiAsync([FromBody] RunTeamCreateMap runTeam)
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
                _logger.LogInformation($"用户 {jwtStr.Id} 创建跑步队伍失败，已有队伍");
                code = new CustomStatusCode
                {
                    Status = "500",
                    Message = $"用户 {jwtStr.Id} 创建跑步队伍失败，已有队伍"
                };
                return StatusCode(500, code);
            }

            if (!await _unitOfWork.SaveChanged())
            {
                _logger.LogInformation($"用户 {jwtStr.Id} 创建跑步队伍保存失败");
                code = new CustomStatusCode
                {
                    Status = "500",
                    Message = $"用户 {jwtStr.Id} 创建跑步队伍保存失败"
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
        public async Task<IActionResult> UpdateUserRunTeamIdAsync()
        {
            CustomStatusCode code;

            var jwtStr = HttpRequest();

            var user = _userRepository.UserSearch(jwtStr.Id);


            var teamById=_userRepository.TeamSearchByUserIdTeaming(jwtStr.Id);

            _runTeamResource.UpdateUserRunTeamState(user, teamById);

            if (!await _unitOfWork.SaveChanged())
            {
                _logger.LogInformation($"用户 {jwtStr.Id} 修改自己跑步队伍失败");

                code = new CustomStatusCode
                {
                    Status = "500",
                    Message = $"用户 {jwtStr.Id} 修改自己跑步队伍失败",
                };
                return StatusCode(500, code);
            }

            _logger.LogInformation($"用户 {jwtStr.Id} 修改自己跑步队伍 Id 成功");

            code = new CustomStatusCode
            {
                Status = "200",
                Message = $"用户 {jwtStr.Id} 修改自己跑步队伍 Id 成功",
                Data = $"队伍 Id {teamById}"
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 查询自身跑步队伍 Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchRunTeamId",Name = "SearchRunTeamId")]
        public IActionResult SearchRunTeamId()
        {
            var jwtStr = HttpRequest();

            CustomStatusCode code;

            var user = _userRepository.UserSearch(jwtStr.Id);

            if (user.RunTeamId==0)
            {
                _logger.LogInformation($"用户 {jwtStr.Id} 未参加跑步队伍");

                code = new CustomStatusCode
                {
                    Status = 200,
                    Message = $"用户 {jwtStr.Id} 未参加跑步队伍",
                };
                return StatusCode(200, code);
            }

            _logger.LogInformation($"用户 {jwtStr.Id} 已参加跑步队伍");

            code = new CustomStatusCode
            {
                Status = 200,
                Message = $"用户 {jwtStr.Id} 已参加跑步队伍",
                Data = $"队伍id {user.RunTeamId}"
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 删除自身创建的队伍,队长权限，删除后请重新登陆
        /// </summary>
        /// <returns></returns>
        [HttpGet("DeleteTeamApi", Name = "DeleteTeamApi")]
        [Authorize(Roles = "Captain")]
        public async Task<IActionResult> DeleteTeamApiAsync()
        {
            CustomStatusCode code;
            var jwt = HttpRequest();
            var success = _runTeamResource.DeleteTeamAsync(jwt.Id);
            if (success.Equals(false))
            {
                _logger.LogInformation($"用户 {jwt.Id} 自身并没有创建队伍，或者删除失败！");
                code=new CustomStatusCode
                {
                    Status = "404",
                    Message = $"用户 {jwt.Id} 自身并没有创建队伍，或者删除失败！"
                };
                return StatusCode(404, code);
            }

            if (!await _unitOfWork.SaveChanged())
            {
                _logger.LogInformation($"用户 {jwt.Id} 保存删除队伍失败");

                code = new CustomStatusCode
                {
                    Status = "500",
                    Message = $"用户 {jwt.Id} 保存删除队伍失败",
                };
                return StatusCode(500, code);
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
        /// 获取所有开放加入战队信息
        /// </summary>
        /// <returns></returns>
        //[HttpGet("SearchAllTeamApi",Name = "SearchAllTeamApi")]
        //public IActionResult SearchAllTeamApi()
        //{

        //}

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
        /// 修改队伍信息,队长权限
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateTeamApi",Name = "UpdateTeamApi")]
        [Authorize(Roles= "Captain")]
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
        /// 上传队伍图片,队长权限
        /// </summary>
        /// <param name="file">图片</param>
        /// <returns></returns>
        [HttpPost("UpLoadPhoto",Name = "UpLoadPhoto")]
        [Authorize(Roles= "Captain")]
        public IActionResult UpLoadPhoto(IFormFile file)
        {
            var jwt = HttpRequest();
            var user = _userRepository.UserSearch(jwt.Id);

            var code = _imagesResource.UpLoadPhoto(file, "\\Images\\RunTeam\\", user.RunTeamId);
            if (code.Status.ToString() == "400")
            {
                _logger.LogInformation($"图片 {user.RunTeamId} 上传失败，格式错误");
                return StatusCode(400, code);
            }
            _logger.LogInformation($"图片 {user.RunTeamId} 上传成功");
            return StatusCode(200, code);
        }

        /// <summary>
        /// 加载自身战队图片
        /// </summary>
        /// <returns></returns>
        [HttpGet("UpLoadPhoto", Name = "UpLoadPhoto")]
        public IActionResult LoadingPhoto(int runteamId)
        {
            var image = _imagesResource.LoadingPhoto("\\Images\\RunTeam\\", runteamId);
            if (image == null)
            {
                _logger.LogInformation($"获取战队 {runteamId} 图片失败");
                var code = new CustomStatusCode
                {
                    Status = "404",
                    Message = $"获取战队 {runteamId} 图片失败"
                };
                return StatusCode(404, code);
            }
            _logger.LogInformation($"获取战队 {runteamId} 图片成功");
            return image;
        }

        /// <summary>
        /// 申请加入队伍
        /// </summary>
        /// <param name="teamId">队伍 Id</param>
        /// <returns></returns>
        [HttpGet("ApplyToJoinTeamApi",Name = "ApplyToJoinTeamApi")]
        public async Task<IActionResult> ApplyToJoinTeamApiAsync(int teamId)
        {
            CustomStatusCode code;
            var jwt = HttpRequest();
            var user = _userRepository.UserSearch(jwt.Id);

            //判断是否已经加入队伍
            if (user.RunTeamId != 0)
            {
                _logger.LogInformation($"用户 {user.Id} 申请加入 {teamId} 战队失败，已加入队伍");
                code = new CustomStatusCode
                {
                    Status = "204",
                    Message = $"用户 {user.Id} 申请加入 {teamId} 战队失败，已加入队伍"
                };
                return StatusCode(204, code);
            }

            //判断申请者队伍是否存在此人
            var listApplicant = _runTeamResource.SearchAllParticipate(teamId);
            List<int> list = new List<int>();
            int i=0;
            foreach(var l in listApplicant)
            {
                list[i] = l.Id;
                i++;
            }
            if (list.Contains(user.Id))
            {
                _logger.LogInformation($"用户 {user.Id} 申请加入 {teamId} 战队失败,已经申请");
                code = new CustomStatusCode
                {
                    Status = "204",
                    Message = $"用户 {user.Id} 申请加入 {teamId} 战队失败,已经申请"
                };
                return StatusCode(204, code);
            }

            var applicant = new RunApplicant
            {
                UserId = user.Id,
                Name = user.Name,
                Sex = user.Sex,
                ApplicationDate = DateTime.Now,
                RunTeamId = teamId
            };
            _runTeamResource.ParticipateInTeam(applicant);
            if(!await _unitOfWork.SaveChanged())
            {
                _logger.LogError($"用户 {user.Id} 申请加入 {teamId} 战队保存失败");
                code = new CustomStatusCode
                {
                    Status = "500",
                    Message = $"用户 {user.Id} 申请加入 {teamId} 战队保存失败"
                };
                return StatusCode(500, code);
            }
            _logger.LogError($"用户 {user.Id} 申请加入 {teamId} 战队");
            code = new CustomStatusCode
            {
                Status = "201",
                Message = $"用户 {user.Id} 申请加入 {teamId} 战队"
            };
            return StatusCode(201, code);
        }

        /// <summary>
        /// 获取队伍所有申请者列表,队长权限
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Captain")]
        [HttpGet("SearchAllApplicant",Name = "SearchAllApplicant")]
        public IActionResult SearchAllApplicant()
        {
            CustomStatusCode code;
            var jwt = HttpRequest();
            var user = _userRepository.UserSearch(jwt.Id);
            var applicantList = _runTeamResource.SearchAllParticipate(user.RunTeamId);
            if (applicantList == null)
            {
                _logger.LogInformation($"用户 {jwt.Id} 查询 {user.RunTeamId} 队伍申请者为空");
                code = new CustomStatusCode
                {
                    Status = "404",
                    Message = $"用户 {jwt.Id} 查询 {user.RunTeamId} 队伍申请者为空"
                };
                return StatusCode(404, code);
            }
            var applicantModel = _mapper.Map<IEquatable<RunApplicantMapper>>(applicantList);

            _logger.LogInformation($"用户 {jwt.Id} 查询 {user.RunTeamId} 队伍申请者成功");

            code = new CustomStatusCode
            {
                Status = "200",
                Message = $"用户 {jwt.Id} 查询 {user.RunTeamId} 队伍申请者成功",
                Data= applicantModel
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 同意入队,队长权限
        /// </summary>
        /// <param name="userId">申请者 Id</param>
        /// <returns></returns>
        [Authorize(Roles = "Captain")]
        [HttpGet("AgreeWithTheTeamApi", Name = "AgreeWithTheTeamApi")]
        public async Task<IActionResult> AgreeWithTheTeamApi(int userId)
        {
            CustomStatusCode code;
            var user = _userRepository.UserSearch(userId);

            //判断是否已经加入队伍
            if(user.RunTeamId!=0)
            {
                _logger.LogInformation($"用户 {user.Id} 已加入队伍，申请失败");
                code = new CustomStatusCode
                {
                    Status = "204",
                    Message = $"用户 {user.Id} 已加入队伍，申请失败"
                };
                return StatusCode(204, code);
            }

            //判断是都申请了此战队
            var applicant = _runTeamResource.SearchByIdApplicant(userId);
            if(applicant==null)
            {
                _logger.LogError($"用户 {user.Id} 并没有申请此战队");
                code = new CustomStatusCode
                {
                    Status = "404",
                    Message = $"用户 {user.Id} 并没有申请此战队"
                };
                return StatusCode(404, code);
            }

            //判断事都保存成功
            var participants = _mapper.Map<RunParticipants>(applicant);
            _runTeamResource.AgreeWithTheTeam(participants);
            if (!await _unitOfWork.SaveChanged())
            {
                _logger.LogError($"用户 {user.Id} 申请加入 {applicant.RunTeamId} 战队保存失败");
                code = new CustomStatusCode
                {
                    Status = "500",
                    Message = $"用户 {user.Id} 申请加入 {applicant.RunTeamId} 战队保存失败"
                };
                return StatusCode(500, code);
            }

            //加入成功
            _logger.LogError($"用户 {user.Id} 申请加入 {applicant.RunTeamId} 战队成功");
            code = new CustomStatusCode
            {
                Status = "201",
                Message = $"用户 {user.Id} 申请加入 {applicant.RunTeamId} 战队保存成功"
            };
            return StatusCode(201, code);

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
    }
}
