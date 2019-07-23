using System.Collections.Generic;
using System.Threading.Tasks;
using Team.Model.AutoMappers.UserMapper;
using Team.Model.Model.UserModel;

namespace Team.Infrastructure.IRepositories
{
    public interface ILatitudeAndLongitudeResource
    {
        /// <summary>
        /// 上传经纬度
        /// </summary>
        /// <param name="lAndLUpLoadMapper"></param>
        /// <param name="userId"></param>
        Task UpLoadingAsync(LAndLUpLoadMapper lAndLUpLoadMapper,int userId);

        /// <summary>
        /// 查看全部
        /// </summary>
        /// <returns></returns>
        IEnumerable<LatitudeAndLongitude> LAndLSearchAll(int userId);
    }
}
