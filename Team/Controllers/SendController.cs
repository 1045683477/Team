using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Team.Infrastructure.IRepositories;
using Team.Model;

namespace Team.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SendController:Controller
    {
        private readonly ILogger<SendController> _logger;

        public SendController(
            ILogger<SendController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="cpacha">短信内容</param>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        [HttpGet("Send",Name = "Send")]
        public IActionResult Send(string cpacha, string phone)
        {
            string ret = null;
            CCPRestSDK.CCPRestSDK api = new CCPRestSDK.CCPRestSDK();
            bool isInit = api.init("app.cloopen.com", "8883");
            api.setAccount("8aaf07086ab0c082016ad31e9cdc16f2", "4bc6f13fd81343e38bf6b758817de76d");
            api.setAppId("8aaf07086ab0c082016ad31e9d2a16f8");
            try
            {
                if (isInit)
                {
                    string[] strings = { cpacha, "2" };
                    Dictionary<string, object> retData = api.SendTemplateSMS(phone, "1", strings);
                    ret = getDictionaryData(retData);
                }
                else
                {
                    ret = "初始化失败";
                }
            }
            catch (Exception exc)
            {
                ret = exc.Message;
            }
            finally
            {
                Console.WriteLine(ret);//Response.Write(ret);
            }

            _logger.LogInformation("短信发送成功");
            CustomStatusCode code = new CustomStatusCode
            {
                Status = "200",
                Message = "短信发送成功"
            };

            return StatusCode(200, code);
        }


        private string getDictionaryData(Dictionary<string, object> data)
        {
            string ret = null;
            foreach (KeyValuePair<string, object> item in data)
            {
                if (item.Value != null && item.Value.GetType() == typeof(Dictionary<string, object>))
                {
                    ret += item.Key.ToString() + "={";
                    ret += getDictionaryData((Dictionary<string, object>) item.Value);
                    ret += "};";
                }
                else
                {
                    ret += item.Key.ToString() + "=" + (item.Value == null ? "null" : item.Value.ToString()) + ";";
                }
            }

            return ret;
        }

    }


}
