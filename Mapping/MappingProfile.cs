using AutoMapper;
using WebEnterprise.Models.Entities;
using WebEnterprise.ViewModels.Comment;
using WebEnterprise.ViewModels.Contribution;
using WebEnterprise.ViewModels.Faculty;
using WebEnterprise.ViewModels.Imgae;
using WebEnterprise.ViewModels.Megazine;
using WebEnterprise.ViewModels.Semester;

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
            CreateMap<CreateMegazineModel, Megazine>()
                .ReverseMap();
            CreateMap<EditMegazineModel, Megazine>()
                .ReverseMap();

            CreateMap<Contribution, CreateContribution>().ReverseMap();
            CreateMap<Contribution, DetailContribution>()
                .ForMember(dest => dest.FullName, act => act.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.ProfilePicture, act => act.MapFrom(src => src.User.ProfilePicture))
                .ForMember(dest => dest.numberContribution, act => act.MapFrom(src => src.User.Contributions.Count()))
                .ReverseMap();
            CreateMap<Contribution, UpdateContribution>().ReverseMap();

            CreateMap<Image, CreateImage>().ReverseMap();

            CreateMap<Comment, CreateComment>().ReverseMap();

            CreateMap<Semester, CreateSemester>().ReverseMap();
            CreateMap<Semester, UpdateSemester>().ReverseMap();
            CreateMap<Semester, GetSemesterAdmin>()
              .ForMember(dest => dest.SemesterId, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
              .ReverseMap();

        }


    }
}
