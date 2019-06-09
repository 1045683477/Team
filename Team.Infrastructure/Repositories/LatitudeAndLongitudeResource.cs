using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Team.Infrastructure.DbContext;
using Team.Infrastructure.IRepositories;
using Team.Model.AutoMappers.UserMapper;
using Team.Model.Model.UserModel;
namespace Team.Infrastructure.Repositories
{
    public class LatitudeAndLongitudeResource:ILatitudeAndLongitudeResource
    {
        #region Initial

        private readonly MyContext _myContext;

        public LatitudeAndLongitudeResource(
            MyContext myContext)
        {
            _myContext = myContext;
        }

        #endregion


        /// <summary>
        /// 上传经纬度
        /// </summary>
        /// <param name="lAndLUpLoadMapper"></param>
        /// <param name="userId"></param>
        public async Task UpLoadingAsync(LAndLUpLoadMapper lAndLUpLoadMapper, int userId)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("Latitude", lAndLUpLoadMapper.Latitude),
                new SqlParameter("Longitude", lAndLUpLoadMapper.Longitude),
                new SqlParameter("userId", userId),
            };
            var result=await _myContext.Database.ExecuteSqlCommandAsync(
                "update LatitudeAndLongitudes set Latitude=@Latitude,Longitude=@Longitude where UserId=@userId",
                parameters);
            
        }

        /// <summary>
        /// 查看全部
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LatitudeAndLongitude> LAndLSearchAll()
        {
            return _myContext.LatitudeAndLongitudes.ToList();
        }
    }
}
