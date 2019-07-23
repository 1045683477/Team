using System.Collections.Generic;
using System.Linq;
using Team.Infrastructure.DbContext;
using Team.Infrastructure.IRepositories;
using Team.Model.Model.ParentModel;
using Team.Model.Model.UserModel;

namespace Team.Infrastructure.Repositories
{
    public class ParentResource: IParentResource
    {
        #region Initial

        private readonly MyContext _myContext;

        public ParentResource(
            MyContext myContext)
        {
            _myContext = myContext;
        }

        #endregion

        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="account"></param>
        /// <param name="parentId">父母 Id</param>
        /// <param name="relationShip"></param>
        /// <returns></returns>
        public bool Binding(string account, int parentId, RelationShip relationShip)
        {
            var user = from u in _myContext.Users
                where u.Account == account
                select u;
            if (user.FirstOrDefault()==null)
            {
                return false;
            }
            else
            {
                var userone = user.FirstOrDefault();
                Children children = new Children
                {
                    Account = account,
                    SonId = userone.Id,
                    ParentId = parentId,
                    Name = userone.Name,
                    RelationShip = relationShip
                };
                _myContext.Children.Add(children);
                return true;
            }
        }

        /// <summary>
        /// 查询绑定的某个前十个经纬度
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<LatitudeAndLongitude> SearchChildrenLal(int id)
        {
            var user = (from longitude in _myContext.LatitudeAndLongitudes
                where longitude.UserId.Equals(id)
                select longitude).TakeLast(10);
            return user;
        }

        /// <summary>
        /// 查询用户所有绑定的孩子
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Children> SearchAllChildren(int userId)
        {
            var user = from child in _myContext.Children
                where child.ParentId == userId
                select child;
            return user;
        }
    }
}
