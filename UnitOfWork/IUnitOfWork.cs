using WebApplication1.Interfaces.Repositories;

namespace WebApplication1.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }

        public Task<int> SaveChangesAsync();
        public void Dispose();
    }
}
