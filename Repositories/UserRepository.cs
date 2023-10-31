using WebApplication1.Entities;
using WebApplication1.Interfaces.Repositories;

namespace WebApplication1.Repositories
{
    public class UserRepository : GenericRepository<UserEntity>, IUserRepository
    {
        public UserRepository(ASMContext context) : base(context) { }
    }
}
