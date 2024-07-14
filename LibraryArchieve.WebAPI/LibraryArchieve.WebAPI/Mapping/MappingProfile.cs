using AutoMapper;
using LibraryArchieve.WebAPI.Data.Entities;
using LibraryArchieve.WebAPI.V1.Requests;
using LibraryArchieve.WebAPI.V1.Responses;

namespace LibraryArchieve.WebAPI.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateBookRequest, Book>(); 

        CreateMap<Book, CreateBookResponse>(); 

        CreateMap<Book, QueryBooksResponse>();

        CreateMap<UpdateBookRequest, Book>();

        CreateMap<Book, UpdateBookResponse>();

        CreateMap<CreateBookShelfRequest, BookShelf>();

        CreateMap<BookShelf, CreateBookShelfResponse>();
       
        CreateMap<CreateNoteRequest, Note>();
        CreateMap<UpdateNoteRequest, Note>();
        CreateMap<Note, CreateNoteResponse>();
        CreateMap<Note, UpdateNoteResponse>();
        CreateMap<Note, QueryNoteResponse>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
            .ForMember(dest => dest.BookAuthor, opt => opt.MapFrom(src => src.Book.Author));
    }
}
