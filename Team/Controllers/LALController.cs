using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Team.AuthHelper.OverWrite;
using Team.Infrastructure.IRepositories;
using Team.Model;
using Team.Model.AutoMappers.UserMapper;

namespace Team.Controllers
{
    /// <summary>
    /// 经纬度上传
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Client,Captain,Admin,SuperAdministrator")]
    public class LALController:Controller
    {
        #region Initial

        private readonly ILatitudeAndLongitudeResource _longitudeResource;
        private readonly IJwtHelper _jwtHelper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public LALController(
            ILatitudeAndLongitudeResource longitudeResource,
            IJwtHelper jwtHelper,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            //不加上<LALController>会报错
            ILogger<LALController> logger)
        {
            _longitudeResource = longitudeResource;
            _jwtHelper = jwtHelper;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        /// <summary>
        /// 经纬度上传
        /// </summary>
        /// <param name="lAndLUpLoadMapper"></param>
        /// <returns></returns>
        [HttpPost("UpLoadApi",Name = "UpLoadApi")]
        public async Task<IActionResult> UpLoadApiAsync(LAndLUpLoadMapper lAndLUpLoadMapper)
        {
            var jwt = HttpRequest();
            CustomStatusCode code;
            await _longitudeResource.UpLoadingAsync(lAndLUpLoadMapper,jwt.Id);
            //if (!await _unitOfWork.SaveChanged())
            //{
            //    _logger.LogError($"用户 {jwt.Id} 上传经纬度失败");
            //    code = new CustomStatusCode
            //    {
            //        Status = "500",
            //        Message = $"用户 {jwt.Id} 上传经纬度失败"
            //    };
            //    return StatusCode(500, code);
            //}
            _logger.LogInformation($"用户 {jwt.Id} 上传经纬度成功");
            code = new CustomStatusCode
            {
                Status = "200",
                Message = $"用户 {jwt.Id} 上传经纬度成功"
            };
            return StatusCode(200, code);
        }

        /// <summary>
        /// 获取所有用户的经纬度
        /// </summary>
        /// <returns></returns>
        [HttpGet("ObtainAll",Name = "ObtainAll")]
        public IActionResult ObtainAll()
        {
            var all = _longitudeResource.LAndLSearchAll();
            var resource = _mapper.Map<IEnumerable<LAndLSearchMapper>>(all);
            var code = new CustomStatusCode
            {
                Status = 200,
                Message = $"获取所有经纬度成功",
                Data = resource
            };
            return StatusCode(200, code);
        }

        private TokenModelJWT HttpRequest()
        {
            var jwt = HttpContext.Request.Headers["Authorization"];
            jwt = jwt.ToString().Substring("Bearer ".Length).Trim();
            return _jwtHelper.SerizlizeJWT(jwt);
        }
    }
}
