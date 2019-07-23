using System.Collections.Generic;
using Team.Model.Model.ParentModel;
using Team.Model.Model.UserModel;

namespace Team.Infrastructure.IRepositories
{
    public interface IParentResource
    {
        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="account"></param>
        /// <param name="parentId">父母 Id</param>
        /// <param name="relationShip"></param>
        /// <returns></returns>
        bool Binding(string account, int parentId,RelationShip relationShip);

        /// <summary>
        /// 查询绑定的某个前十个经纬度
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<LatitudeAndLongitude> SearchChildrenLal(int id);

        /// <summary>
        /// 查询用户所有绑定的孩子
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<Children> SearchAllChildren(int userId);
    }
}
