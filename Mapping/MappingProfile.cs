using AutoMapper;
using WebEnterprise.Models.Entities;
using WebEnterprise.ViewModels.Contribution;
using WebEnterprise.ViewModels.Faculty;
using WebEnterprise.ViewModels.Megazine;

namespace WebEnterprise.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Faculty, CreateFacultyModel>().ReverseMap();
            CreateMap<Faculty, GetFacultyModel>()
                .ForMember(dest => dest.FacultyId, act => act.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<Megazine, GetMegazineModel>()
                .ForMember(dest => dest.FacultyName, act => act.MapFrom(src => src.Faculty.Name))
                .ReverseMap();
            CreateMap<Contribution, CreateContribution>().ReverseMap();
        }


    }
}
