using AutoMapper;
using System.Linq.Expressions;
using WebApplication1.Dtos;
using WebApplication1.Entities;
using WebApplication1.Interfaces.Repositories;
using WebApplication1.Interfaces.Services;
using WebApplication1.UnitOfWork;

namespace WebApplication1.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
        {
            _userRepository = unitOfWork.UserRepository;
        }

        public List<UserEntity> GetWithKeyword(
            string keyword,
            int page,
            int pageSize,
            Expression<Func<UserEntity, UserEntity>>? projection = null
        )
        {
            return _userRepository.GetMany(
                predicate: (user) => user.Username.Contains(keyword),
                page: page,
                pageSize: pageSize,
                projection: projection,
                orderBy: query => query.OrderBy(user => user.Username)
            );
        }

        public UserEntity? GetById(
            Guid id,
            Expression<Func<UserEntity, UserEntity>>? projection = null
        )
        {
            return _userRepository.GetOne(
                (user) => user.Id == id,
                projection
            );
        }

        public async Task<Guid?> Remove(Guid id)
        {
            var user = GetById(id, projection: (user) => new UserEntity() { Id = user.Id });

            if (user == null)
            {
                return null;
            }

            _userRepository.Remove(user);
            await _unitOfWork.SaveChangesAsync();

            return id;
        }

        public async Task<Guid?> Update(Guid id, UserUpdateDto dataToUpdate)
        {
            var user = GetById(id);
            if (user == null)
            {
                return null;
            }

            _mapper.Map(dataToUpdate, user);
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return id;
        }
    }
}
