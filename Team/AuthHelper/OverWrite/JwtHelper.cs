using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Team.Model;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Team.AuthHelper.OverWrite
{
    public class JwtHelper:IJwtHelper
    {
        public static string secretKey { get; set; } = "sdfsdfsrty45634kkhllghtdgdfss345t678fs";

        /// <summary>
        /// 颁发 token
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public string IssueJWT(TokenModelJWT tokenModel)
        {
            var dataTime = DateTime.UtcNow;

            var claims = new Claim[]
            {
                //下边为claim的默认配置
                new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                //这个就是过期时间，目前是过期100秒，可自定义，注意JWT有自己的缓冲过期时间
                //new Claim(JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(100)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Exp,
                    $"{new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Iss, "OnePiece"),
                new Claim(JwtRegisteredClaimNames.Aud, "wr"),
                new Claim(ClaimTypes.Role, tokenModel.Role),
            };

            //密钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtHelper.secretKey));
            var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: "OnePiece",
                claims: claims,
                signingCredentials: creds);

            var jwtHandler=new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>
        /// 解析字符串
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public TokenModelJWT SerizlizeJWT(string jwtStr)
        {
            var jwtHandler=new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            object role=new object();
            try
            {
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var tm=new TokenModelJWT
            {
                Id = (jwtToken.Id).ObjToInt(),
                Role = role!=null?role.ObjToString():""
            };
            return tm;
        }
    }
}
