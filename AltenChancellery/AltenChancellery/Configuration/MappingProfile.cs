using AutoMapper;
using DBLayer.Models;
using ServiceLayer.DTOs;

namespace AltenChancellery.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mappatura diretta tra due classi
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
                .ReverseMap();

        }
    }
}
