using System;

namespace Team.AuthHelper.OverWrite
{
    /// <summary>
    /// token 的生成与解密
    /// </summary>
    public interface IJwtHelper
    {
        /// <summary>
        /// 颁发 token
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        string IssueJWT(TokenModelJWT tokenModel);

        /// <summary>
        /// 解析字符串
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        TokenModelJWT SerizlizeJWT(string jwtStr);
    }
}
