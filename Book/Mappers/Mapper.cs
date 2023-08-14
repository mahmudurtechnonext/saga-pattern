using AutoMapper;
using Book.DTOs;
using Book.ViewModels;

namespace Book.Mappers
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Models.Book, BookDTO>();
            CreateMap<BookDTO, Models.Book>();
            CreateMap<Models.Book, BookViewModel>();
            CreateMap<BookViewModel, Models.Book>();
        }
    }
}
