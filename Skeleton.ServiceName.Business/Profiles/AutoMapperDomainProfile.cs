using AutoMapper;
using Skeleton.ServiceName.Data;
using Skeleton.ServiceName.Utils.AutoMapper;
using Skeleton.ServiceName.ViewModel.People;

namespace Skeleton.ServiceName.Business.Profiles
{
    public class AutoMapperDomainProfile : Profile
    {
        public AutoMapperDomainProfile()
        {
            CreateMap<Person, PersonViewModel>()
                .IgnoreAllNonExisting()
                .ReverseMap()
                .IgnoreAllNonExisting();
        }
    }
}
