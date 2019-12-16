using AutoMapper;
using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.Utils.AutoMapper;
using Skeleton.ServiceName.Utils.Security;
using Skeleton.ServiceName.ViewModel.People;
using Skeleton.ServiceName.ViewModel.User;

namespace Skeleton.ServiceName.Business.Profiles
{
    public class AutoMapperDomainProfile : Profile
    {
        public AutoMapperDomainProfile()
        {
            CreateMap<User, UserViewModel>()
                .IgnoreAllNonExisting()
                .ReverseMap()
                .IgnoreAllNonExisting();

            CreateMap<User, SecurityUserModel>()
                .IgnoreAllNonExisting()
                .ReverseMap()
                .IgnoreAllNonExisting();

            CreateMap<Person, PersonViewModel>()
                .IgnoreAllNonExisting()
                .ReverseMap()
                .IgnoreAllNonExisting();
        }
    }
}
