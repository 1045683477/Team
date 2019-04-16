using System.Threading.Tasks;
using Team.Infrastructure.DbContext;
using Team.Infrastructure.IRepositories;

namespace Team.Infrastructure.Repositories
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly MyContext _myContext;

        public UnitOfWork(MyContext myContext)
        {
            _myContext = myContext;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveChanged()
        {
            return await _myContext.SaveChangesAsync() > 0;
        }
    }
}
