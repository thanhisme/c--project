using WebApplication1.Entities;
using WebApplication1.Interfaces.Repositories;

namespace WebApplication1.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshTokenEntity>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ASMContext context) : base(context) { }
    }
}
