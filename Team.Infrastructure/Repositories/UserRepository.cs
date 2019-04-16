using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Team.Infrastructure.DbContext;
using Team.Infrastructure.IRepositories;
using Team.Model.AutoMappers;
using Team.Model.Model;

namespace Team.Infrastructure.Repositories
{
    public class UserRepository:IUserRepository
    {
        #region Initial

        private readonly MyContext _myContext;

        public UserRepository(MyContext myContext)
        {
            _myContext = myContext;
        }

        #endregion
        

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<User> UserLogin(string account, string password)
        {
            return await _myContext.Users.SingleOrDefaultAsync(x=>x.Account==account && x.Password==password);
        }

        /// <summary>
        /// 判断该账号是否存在，为注册服务,true 表示可注册
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns></returns>
        public async Task<bool> UserAccountExits(string account)
        {
            var exits = await _myContext.Users.SingleOrDefaultAsync(x => x.Account == account);
            if (exits==null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async void UserRegisterd(User user)
        {
            user.Role = Role.Client;
            await _myContext.Users.AddAsync(user);
        }

        /// <summary>
        /// 修改用户资料,已经过保存，true为保存成功
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userUpdate">修改资料</param>
        /// <returns></returns>
        public async Task<bool> UserUpdate(int userId, UserUpdateMap userUpdate)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("Password", userUpdate.Password),
                new SqlParameter("Name", userUpdate.Name),
                new SqlParameter("Sex",userUpdate.Sex),
                new SqlParameter("UniversityId",userUpdate.UniversityId),
                new SqlParameter("studentId",userUpdate.studentId),
                new SqlParameter("Province",userUpdate.Province), 
            };
            
            var update = await _myContext.Database.ExecuteSqlCommandAsync(
                "update Users set Password=@Password,Name=@Name,Sex=@Sex,UniversityId=@UniversityId,studentId=@studentId",
                parameters);
            if (update>0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 查询用户个人数据
        /// </summary>
        /// <param name="userId">用户 Id</param>
        /// <returns></returns>
        public User UserSearch(int userId)
        {
            return _myContext.Users.SingleOrDefault(x => x.Id == userId);
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public User RetrievePassword(string account)
        {
            return _myContext.Users.SingleOrDefault(x => x.Account == account);
        }
    }
}
