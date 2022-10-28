using ShopApi.DTOs;
using ShopApi.Entities;
using AutoMapper;

namespace ShopApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterDto, AppUser>()//<source,destination>
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(source => source.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(source => source.Email));
        }
    }
}
