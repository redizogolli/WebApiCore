using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace WebApiCore
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<BookDto, Book>();
        }
    }
}
