﻿using AutoMapper;
using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.TokenService;
using Identity.BusinessLogic.Models.UserService;
using Identity.DataAccess.Entities;
using Identity.Grpc;
using Microsoft.AspNetCore.Identity;
using MusicService.Grpc;

namespace Identity.BusinessLogic.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterUserRequest, User>();
            CreateMap<User, UserDto>();
            CreateMap<UpdateUserRequest, User>();
            CreateMap<UserDto, GetTokensRequest>().ForMember(s => s.Roles, opt => opt.Ignore());
            CreateMap<IdentityRole, RoleDto>();
            CreateMap<User, AddUserRequest>();
            CreateMap<UserDto, UserInfo>();
        }
    }
}
