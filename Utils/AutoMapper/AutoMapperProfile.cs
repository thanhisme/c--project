using AutoMapper;
using WebApplication1.Dtos;
using WebApplication1.Entities;

namespace WebApplication1.Utils.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        private static readonly Func<object, object, object, bool> _skipNullProps = (_, __, srcMember) => srcMember != null;

        public AutoMapperProfile()
        {
            CreateMap<SignUpDto, UserEntity>().ForAllMembers(opts => opts.Condition(_skipNullProps));
            CreateMap<UserUpdateDto, UserEntity>().ForAllMembers(opts => opts.Condition(_skipNullProps));
        }
    }
}
