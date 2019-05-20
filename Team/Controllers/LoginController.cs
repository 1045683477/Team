using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Team.AuthHelper.OverWrite;
using Team.Infrastructure.IRepositories;
using Team.Model;
using Team.Model.AutoMappers.UserMapper;
using Team.Model.Model.UserModel;

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
        private readonly IImagesResource _imagesResource;

        public LoginController(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IJwtHelper jwtHelper,
            ILogger<LoginController> logger,
            IImagesResource imagesResource)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtHelper = jwtHelper;
            _logger = logger;
            _imagesResource = imagesResource;
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
        [HttpPost("UserUpdateApi",Name = "UserUpdateApi")]
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
        [HttpPost("RetrievePasswordApi",Name = "RetrievePasswordApi")]
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
        /// 上传用户头像
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("UserUpLoadPhoto",Name = "UserUpLoadPhoto")]
        public IActionResult UserUpLoadPhoto(IFormFile formFile)
        {
            var user = HttpRequest();
            CustomStatusCode code;
            code= _imagesResource.UpLoadPhoto(formFile, "\\Images\\HeadPortrait\\", user.Id);
            if (code.Status.ToString() == "400")
            {
                _logger.LogInformation($"图片 {user.Id} 上传失败，格式错误");
                return StatusCode(400, code);
            }
            _logger.LogInformation($"图片 {user.Id} 上传成功");
            return StatusCode(200, code);

            #region 成功的可用的

            /*var currentPictureExtension = Path.GetExtension(formFile.FileName).ToUpper();
            if (LimitPictureType.Contains(currentPictureExtension))
            {
                if (Directory.Exists(upload_path)==false)
                {
                    Directory.CreateDirectory(upload_path);
                }

                var fileName = user.Id + ".jpeg";
                var ImagePath = upload_path + fileName;
                using (var fs=System.IO.File.Create(ImagePath))
                {
                    formFile.CopyTo(fs);
                    fs.Flush();
                }
                _logger.LogInformation($"用户 {user.Id} 上传头像成功");
                code = new CustomStatusCode
                {
                    Status = "200",
                    Message = $"用户 {user.Id} 上传头像成功"
                };
                return StatusCode(200, code);
            }
            _logger.LogInformation($"用户 {user.Id} 上传头像失败，格式错误");
            code = new CustomStatusCode
            {
                Status = "400",
                Message = $"用户 {user.Id} 上传头像失败，格式错误"
            };
            return StatusCode(400, code);*/

            #endregion

        }

        /// <summary>
        /// 加载用户头像
        /// </summary>
        /// <returns></returns>
        [HttpGet("UserLoadingPhoto",Name = "UserLoadingPhoto")]
        public IActionResult UserLoadingPhoto()
        {
            var user = HttpRequest();

            var image=_imagesResource.LoadingPhoto("\\Images\\HeadPortrait\\", user.Id);
            if (image==null)
            {
                _logger.LogInformation($"用户 {user.Id} 没有存入头像");
                var code = new CustomStatusCode
                {
                    Status = "404",
                    Message = $"用户 {user.Id} 没有存入头像"
                };
                return StatusCode(404, code);
            }
            _logger.LogInformation($"用户 {user.Id} 获取头像成功");
            return image;


            #region 可行代码

            /*string path= Directory.GetCurrentDirectory() + $@"\Images\{user.Id}" + ".jpeg";
            FileInfo fi=new FileInfo(path);
            FileStream fs = fi.OpenRead();
            byte[] buffer=new byte[fi.Length];
            //读取图片字节流
            fs.Read(buffer, 0, Convert.ToInt32(fi.Length));
            var response = File(buffer, "image/jpeg");
            fs.Close();
            _logger.LogInformation($"用户 {user.Id} 获取头像成功");
           
            return response;*/

            #endregion
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

        /// <summary>
        /// 保存图片名称
        /// </summary>
        private static IList<string> path=new List<string>();

        /// <summary>
        /// 图片格式
        /// </summary>
        public static string[] LimitPictureType =
            {".PNG", ".JPG", ".JPEG", ".BMP", ".GIF", ".ICO"};

        /// <summary>
        /// 图片路径
        /// </summary>
        private static string _uploadPath = Directory.GetCurrentDirectory() + "\\Images\\";
        private static DirectoryInfo _di = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\Images\\");

        /// <summary>
        /// 遍历文件夹文件
        /// </summary>
        /// <param name="info"></param>
        private static void GetFileName(DirectoryInfo info)
        {
            //获取该路径下所有的文件列表
            FileInfo[] fileInfos = info.GetFiles();

            //开始得到图片名称
            foreach (var subinfo in fileInfos)
            {
                string strname = subinfo.Name;
                path.Add(strname);
            }
        }
    }
}
