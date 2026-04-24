
using AutoMapper;
using Domain.Entity;
using Domain.Models;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        { 
            CreateMap<PersonDto,Person>().ReverseMap();
            CreateMap<SpouseViewDto,Spouse>().ReverseMap();
            CreateMap<SpouseDto,Spouse>().ReverseMap();
        }
    }
}
