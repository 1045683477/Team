using System.Threading.Tasks;

namespace Team.Infrastructure.IRepositories
{
    /// <summary>
    /// 保存
    /// </summary>
    public interface IUnitOfWork
    {
        Task<bool> SaveChanged();
    }
}
