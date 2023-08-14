using AutoMapper;
using Student.DTOs;
using Student.ViewModels;

namespace Student.Mappers
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Models.Student, StudentDTO>();
            CreateMap<StudentDTO, Models.Student>();
            CreateMap<Models.Student, StudentViewModel>();
            CreateMap<StudentViewModel, Models.Student>();
        }
    }
}
