using WebApplication1.Entities;
using WebApplication1.Interfaces.Repositories;
using WebApplication1.Repositories;

namespace WebApplication1.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ASMContext _context;
        private bool disposed = false;
        public IUserRepository UserRepository { get; private set; }
        public IRefreshTokenRepository RefreshTokenRepository { get; private set; }

        public UnitOfWork(ASMContext context)
        {
            _context = context;
            UserRepository = new UserRepository(context);
            RefreshTokenRepository = new RefreshTokenRepository(context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
