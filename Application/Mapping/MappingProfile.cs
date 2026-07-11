
using AutoMapper;
using Domain.Entity;
using Domain.Models;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        { 
            CreateMap<PersonDto, Person>()
                .ForMember(dest => dest.ChildOrder, opt => opt.MapFrom(src => src.Order));
            CreateMap<Person, PersonDto>()
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.ChildOrder));
            CreateMap<PersonDto1,Person>().ReverseMap();
            CreateMap<Spouse, SpouseViewDto>()
                .ForMember(dest => dest.HusbandName, opt => opt.MapFrom(src => src.Husband != null ? $"{src.Husband.FirstName} {src.Husband.LastName} {src.Husband.MiddleName}".Trim() : string.Empty))
                .ForMember(dest => dest.WifeName, opt => opt.MapFrom(src => src.Wife != null ? $"{src.Wife.FirstName} {src.Wife.LastName} {src.Wife.MiddleName}".Trim() : string.Empty));
            CreateMap<SpouseViewDto, Spouse>();
            CreateMap<SpouseDto,Spouse>().ReverseMap();
            CreateMap<GenerationDto, Generation>().ReverseMap();
        }
    }
}
