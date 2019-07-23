using System;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Team.Infrastructure.DbContext;
using Team.Infrastructure.IRepositories;
using Team.Model.AutoMappers.UserMapper;
using Team.Model.Model.TeamModel;
using Team.Model.Model.UserModel;

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
            Statistical statistical1=new Statistical();
            user.RunTeamId = 0;
            statistical1.SportFreeModel = SportFreeModel.Running;
            statistical1.User = user;
            Statistical statistical2 = new Statistical
            {
                SportFreeModel = SportFreeModel.Riding,
                User = user
            };
            LatitudeAndLongitude latitudeAndLongitude=new LatitudeAndLongitude
            {
                Name = user.Name,
                Phone = user.Account,
                User = user
            };

            await _myContext.Statisticals.AddAsync(statistical1);
            await _myContext.Statisticals.AddAsync(statistical2);
            await _myContext.Users.AddAsync(user);
            await _myContext.LatitudeAndLongitudes.AddAsync(latitudeAndLongitude);
        }

        /// <summary>
        /// 修改用户资料,已经过保存，true为保存成功
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userUpdate">修改资料</param>
        /// <returns></returns>
        public void UserUpdate(int userId, UserUpdateMap userUpdate)
        {
            User user =UserSearch(userId);

            user.Password = userUpdate.Password;
            user.Name = userUpdate.Name;
            user.Sex = userUpdate.Sex;
            user.UniversityId = userUpdate.UniversityId;
            user.StudentId = userUpdate.StudentId;
            user.Province = userUpdate.Province;


            _myContext.Users.Update(user);
            var lal = (from s in _myContext.LatitudeAndLongitudes
                where s.UserId == userId
                select s).FirstOrDefault();
            lal.Name = userUpdate.Name;
            _myContext.LatitudeAndLongitudes.Update(lal);
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

        /// <summary>
        /// 通过用户 Id 查询某个正在组队的团队信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int TeamSearchByUserIdTeaming(int userId)
        {
            var result = from user in _myContext.Users
                join team in _myContext.RunTeams on user.Id equals team.UserId
                         where user.Id==userId
                select team.Id;
            return result.FirstOrDefault();
            //return _myContext.Users.Include(x=>x.RunTeam).SingleOrDefault(x => x.Id == userId);
        }

        /// <summary>
        /// 上传通信 id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="communicationId"></param>
        /// <returns></returns>
        public void UserLoadingCommunicationId(int userId,string communicationId)
        {
            LatitudeAndLongitude latitudeAndLongitude=new LatitudeAndLongitude();
            latitudeAndLongitude = (from s in _myContext.LatitudeAndLongitudes
                                    where s.UserId == userId
                                    select s).First();
            latitudeAndLongitude.CommunicationId = communicationId;
            _myContext.LatitudeAndLongitudes.Update(latitudeAndLongitude);
        }
    }
}
