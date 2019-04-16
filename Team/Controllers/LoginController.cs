using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Team.AuthHelper.OverWrite;
using Team.Infrastructure.IRepositories;
using Team.Model;
using Team.Model.AutoMappers;
using Team.Model.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Team.Controllers
{
    /// <summary>
    /// 登陆,注册，修改密码 API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        #region Initial

        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtHelper _jwtHelper;
        private readonly ILogger<LoginController> _logger;

        public LoginController(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IJwtHelper jwtHelper,
            ILogger<LoginController> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtHelper = jwtHelper;
            _logger = logger;
        }

        #endregion
        

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="userRegistered"></param>
        /// <returns></returns>
        [HttpPost("UserRegisteredApi",Name = "UserRegisteredApi")]
        public async Task<IActionResult> UserRegisteredApi([FromBody]UserRegisteredMap userRegistered)
        {
            CustomStatusCode code;

            bool exit =await _userRepository.UserAccountExits(userRegistered.Account);
            if (exit==false)
            {
                _logger.LogInformation($"注册失败，{userRegistered.Account} 账号已被注册！");
                code =new CustomStatusCode
                {
                    Status = "409",
                    Message = $"{userRegistered.Account} 该账号已被注册"
                };
                return StatusCode(409, code);
            }

            var user = _mapper.Map<User>(userRegistered);
            _userRepository.UserRegisterd(user);
            if (!await _unitOfWork.SaveChanged())
            {
                _logger.LogError($"{userRegistered.Account} 注册保存失败");
                code=new CustomStatusCode
                {
                    Status = "500",
                    Message = $"{userRegistered.Account} 注册保存失败"
                };
                return StatusCode(500, code);
            }

            TokenModelJWT tokenModel=new TokenModelJWT();
            tokenModel.Id = user.Id;
            tokenModel.Role = user.Role.ToString();
            string jwtStr = _jwtHelper.IssueJWT(tokenModel);
            _logger.LogInformation($"{userRegistered.Account} 注册成功");
            code=new CustomStatusCode
            {
                Status = "200",
                Message = $"用户 {userRegistered.Account} 注册成功",
                Data = "Bearer " + jwtStr
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userLogin">登陆模板</param>
        /// <returns></returns>
        [HttpPost("UserLoginApi",Name = "UserLoginApi")]
        public async Task<IActionResult> UserLoginApi([FromBody]UserLoginMap userLogin)
        {
            CustomStatusCode code;

            var user=await _userRepository.UserLogin(userLogin.Account, userLogin.Password);
            if (user==null)
            {
                _logger.LogInformation("登陆失败");
                code = new CustomStatusCode
                {
                    Status = "404",
                    Message = "账号或者密码错误"
                };
                return StatusCode(404, code);
            }
            TokenModelJWT tokenModel=new TokenModelJWT();
            tokenModel.Id = user.Id;
            tokenModel.Role = user.Role.ToString();
            var jwtStr = _jwtHelper.IssueJWT(tokenModel);
            _logger.LogInformation($"用户 {user.Id}登陆成功");
             code =new CustomStatusCode
            {
                Status="200",
                Message = $"用户 {user.Id} 登陆成功",
                Data = ("Bearer " + jwtStr)
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 为修改客户数据提供查询接口
        /// </summary>
        /// <returns></returns>
        [HttpGet("UserForUpdateApi",Name = "UserForUpdateApi")]
        [Authorize(Roles = "Client")]
        public IActionResult UserForUpdateApi()
        {
            CustomStatusCode code;
            var jwtStr = HttpRequest();
            var user = _userRepository.UserSearch(jwtStr.Id);
            var userResource = _mapper.Map<UserUpdateMap>(user);
            _logger.LogInformation($"用户 {user.Id} 获取对应修改信息成功！");
            code =new CustomStatusCode
            {
                Status = "200",
                Message = $"用户 {user.Id} 获取对应修改信息成功！",
                Data = userResource
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 修改个人资料，密码姓名，省份大学
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPatch("UserUpdateApi",Name = "UserUpdateApi")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> UserUpdateApi([FromBody]UserUpdateMap update)
        {
            CustomStatusCode code;
            var jwtStr = HttpRequest();
            var changed=await _userRepository.UserUpdate(jwtStr.Id, update);
            if (changed==false)
            {
                _logger.LogInformation($"用户 {jwtStr.Id} 修改个人资料失败");
                code=new CustomStatusCode
                {
                    Status = "409",
                    Message = $"用户 {jwtStr.Id} 修改个人资料失败"
                };
                return StatusCode(409, code);
            }

            _logger.LogInformation($"用户 {jwtStr.Id} 修改个人资料成功");
            code=new CustomStatusCode
            {
                Status = "200",
                Message = $"用户 {jwtStr.Id} 修改个人资料成功"
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RetrievePasswordApi(string account)
        {
            CustomStatusCode code;
            var exit = _userRepository.RetrievePassword(account);
            if (exit==null)
            {
                _logger.LogInformation($"{account} 账号不存在");
                code=new CustomStatusCode
                {
                    Status = "404",
                    Message = $"{account} 账号不存在"
                };
                return StatusCode(404, code);
            }


            var password = exit.Password;
            TokenModelJWT token=new TokenModelJWT
            {
                Id = exit.Id,
                Role = exit.Role.ToString()
            };
            var jwtStr = _jwtHelper.IssueJWT(token);
            _logger.LogInformation($"{account} 账号验证成功,返回密码与token");
            code=new CustomStatusCode
            {
                Status = "200",
                Message = $"{account} 账号验证成功,返回密码与token",
                Data = new {pwd=password,token=jwtStr}
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
