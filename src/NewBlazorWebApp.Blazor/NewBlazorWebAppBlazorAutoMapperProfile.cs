using AutoMapper;
using NewBlazorWebApp.Books;

namespace NewBlazorWebApp.Blazor;

public class NewBlazorWebAppBlazorAutoMapperProfile : Profile
{
    public NewBlazorWebAppBlazorAutoMapperProfile()
    {
        CreateMap<BookDto, CreateUpdateBookDto>();
        
        //Define your AutoMapper configuration here for the Blazor project.
    }
}
