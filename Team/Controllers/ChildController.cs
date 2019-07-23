using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Team.AuthHelper.OverWrite;
using Team.Infrastructure;
using Team.Infrastructure.IRepositories;
using Team.Model;
using Team.Model.AutoMappers.UserMapper;
using Team.Model.Model.ParentModel;

namespace Team.Controllers
{
    /// <summary>
    /// 家长模式
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Client,Captain,Admin,SuperAdministrator")]
    public class ChildController:Controller
    {
        #region Initial

        private readonly IUnitOfWork _unitOfWork;
        private readonly IParentResource _parentResource;
        private readonly ILogger<ChildController> _logger;
        private readonly IJwtHelper _jwtHelper;
        private readonly IMapper _mapper;

        public ChildController(
            IUnitOfWork unitOfWork,
            IParentResource parentResource,
            ILogger<ChildController> logger,
            IJwtHelper jwtHelper,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _parentResource = parentResource;
            _logger = logger;
            _jwtHelper = jwtHelper;
            _mapper = mapper;
        }

        #endregion


        /// <summary>
        /// 添加孩子
        /// </summary>
        /// <param name="account">被绑定者得账号</param>
        /// <param name="relationShip">关系</param>
        /// <returns></returns>
        [HttpGet("AddChildrenAsyncApi",Name = "AddChildrenAsyncApi")]
        public async Task<IActionResult> AddChildrenAsyncApi(string account, RelationShip relationShip)
        {
            var jwt = HttpRequest();
            CustomStatusCode code;
            bool exit=_parentResource.Binding(account,jwt.Id,relationShip);
            if (!exit)
            {
                _logger.LogInformation($"用户{jwt.Id} 绑定 {account} 失败");
                code=new CustomStatusCode
                {
                    Status = "404",
                    Message = $"用户 {jwt.Id} 绑定 {account} 失败"
                };
                return StatusCode(404, code);
            }

            if (!await _unitOfWork.SaveChanged())
            {
                _logger.LogInformation($"用户 {jwt.Id} 绑定 {account} 保存失败");
                code = new CustomStatusCode
                {
                    Status = "500",
                    Message = $"用户 {jwt.Id} 绑定 {account} 保存失败"
                };
                return StatusCode(500, code);
            }

            _logger.LogInformation($"用户 {jwt.Id} 绑定 {account} 成功");
            code = new CustomStatusCode
            {
                Status = "201",
                Message = $"用户 {jwt.Id} 绑定 {account} 成功"
            };
            return StatusCode(201, code);
        }

        /// <summary>
        /// 查看绑定孩子的前十条经纬度与距离
        /// </summary>
        /// <param name="sonId">孩子Id</param>
        /// <param name="latitude">经度</param>
        /// <param name="longitude">纬度</param>
        /// <returns></returns>
        [HttpGet("SearchChildrenLalApi",Name = "SearchChildrenLalApi")]
        public IActionResult SearchChildrenLalApi(int sonId, double latitude, double longitude)
        {
            CustomStatusCode code;
            var log=_parentResource.SearchChildrenLal(sonId);
            if (log==null)
            {
                _logger.LogInformation($"用户查询 {sonId} 为空");
                code = new CustomStatusCode
                {
                    Status = "404",
                    Message = $"用户查询 {sonId} 为空"
                };
                return StatusCode(404, code);
            }
            var resource = _mapper.Map<IEnumerable<LAndLSearchMapper>>(log);
            foreach (var s in resource)
            {
                s.Distance = LocationUtils.GetDistance(latitude, longitude, s.Latitude, s.Longitude);
            }
            _logger.LogInformation($"用户查询 {sonId} 成功");
            code = new CustomStatusCode
            {
                Status = "200",
                Message = $"用户查询 {sonId} 成功",
                Data = resource
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 查询所有绑定的孩子
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchAllChildrenApi",Name = "SearchAllChildrenApi")]
        public IActionResult SearchAllChildrenApi()
        {
            var jwt = HttpRequest();
            CustomStatusCode code;
            var user=_parentResource.SearchAllChildren(jwt.Id);
            if (user==null)
            {
                _logger.LogInformation($"用户绑定人数为空");
                code = new CustomStatusCode
                {
                    Status = "404",
                    Message = $"用户绑定人数为空"
                };
                return StatusCode(404, code);
            }
            _logger.LogInformation($"用户绑定查询成功");
            code = new CustomStatusCode
            {
                Status = "200",
                Message = $"用户绑定查询成功",
                Data = user
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
    }
}
