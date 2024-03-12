using AutoMapper;
using Identity.BusinessLogic.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.BusinessLogic.Mapping
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<IdentityRole, RoleDto>()
                .ForMember(dest => dest.Name, options => options.MapFrom(source => source.Name));
        }
    }
}
