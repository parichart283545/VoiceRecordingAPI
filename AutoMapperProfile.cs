using AutoMapper;
using VoiceRecordAPI.DTOs;
using VoiceRecordAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceRecordAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Role, RoleDto>()
                .ForMember(x => x.RoleName, x => x.MapFrom(x => x.Name));
            CreateMap<RoleDtoAdd, Role>()
                .ForMember(x => x.Name, x => x.MapFrom(x => x.RoleName)); ;
            CreateMap<UserRole, UserRoleDto>();
            CreateMap<VoiceRecordDetails, GetVoiceRecordDetail>();
            CreateMap<CallType, GetCallTypes>().ForMember(x => x.Value, x => x.MapFrom(x => x.Detail));
            CreateMap<VoiceRecordProviders, GetVoiceRecordProviders>().ForMember(x => x.Value, x => x.MapFrom(x => x.Detail));
            CreateMap<VoiceRecordURLRequest, GetVoiceRecordURLRequest>();
            CreateMap<VoiceRecordURLRequest, InsertVoiceRecordURLRequest>();
        }
    }
}