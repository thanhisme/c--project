using System.Linq.Expressions;
using WebApplication1.Dtos;
using WebApplication1.Entities;

namespace WebApplication1.Interfaces.Services
{
    public interface IUserService
    {
        public List<UserEntity> GetWithKeyword(
            string keyword,
            int page,
            int pageSize,
            Expression<Func<UserEntity, UserEntity>>? projection = null
        );

        public UserEntity? GetById(
            Guid id,
            Expression<Func<UserEntity, UserEntity>>? projection = null
        );

        public Task<Guid?> Remove(Guid id);

        public Task<Guid?> Update(Guid id, UserUpdateDto dataToUpdate);
    }
}
