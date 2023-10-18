using AutoMapper;
using MiniLMS.Domain.Entities;
using MiniLMS.Domain.Models.StudentDTO;
using MiniLMS.Domain.Models.TeacherDTO;

namespace MiniLMS.Application.Mappings;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        CreateMap<StudentCreateDTO, Student>().
            ForMember(dest => dest.Teachers, opt => opt.MapFrom(src => src.Teachersid
            .Select(x => x!=null ? new Teacher
            {
                Id = x
            }: null )));

        CreateMap<Student, StudentGetDTO>().
            ForMember(dest => dest.Teachersid, opt => opt.MapFrom(src => src.Teachers.Select(x => x.Id)));

        CreateMap<UpdateStudentDTO, Student>().
            ForMember(dest => dest.Teachers, opt => opt.MapFrom(src => src.Teachersid
            .Select(x => new Teacher
            {
                Id = x
            })));

        CreateMap<TeacherCreateDTO, Teacher>().
            ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.StudentIds
            .Select(x => x!=null ? new Student
            {
                Id = x
            } : null )));

        CreateMap<Teacher, TeacherGetDTO>()
            .ForMember(dest => dest.StudentIds, opt => opt.MapFrom(src => src.Students.Select(x => x.Id)));

        CreateMap<UpdateTeacherDTO, Teacher>().
            ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.StudentIds
            .Select(x => new Student
            {
                Id = x
            })));
    }
}
