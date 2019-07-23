using System.Threading.Tasks;
using Team.Model.AutoMappers.UserMapper;
using Team.Model.Model.UserModel;

namespace Team.Infrastructure.IRepositories
{
    /// <summary>
    /// 用户登陆，注册，修改
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Task<User> UserLogin(string account, string password);

        /// <summary>
        /// 判断该账号是否存在，为注册服务,true 表示可注册
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns></returns>
        Task<bool> UserAccountExits(string account);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        void UserRegisterd(User user);

        /// <summary>
        /// 改用户资料,已经过保存，true为保存成功
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userUpdate">修改资料</param>
        /// <returns></returns>
        void UserUpdate(int userId, UserUpdateMap userUpdate);

        /// <summary>
        /// 查询用户个人数据
        /// </summary>
        /// <param name="userId">用户 Id</param>
        /// <returns></returns>
        User UserSearch(int userId);

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns></returns>
        User RetrievePassword(string account);

        /// <summary>
        /// 通过用户 Id 查询某个正在组队的团队信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int TeamSearchByUserIdTeaming(int userId);


        /// <summary>
        /// 上传通信 id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="communicationId"></param>
        /// <returns></returns>
        void UserLoadingCommunicationId(int userId,string communicationId);
    }
}
