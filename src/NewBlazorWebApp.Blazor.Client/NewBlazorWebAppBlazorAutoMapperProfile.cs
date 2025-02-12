using Volo.Abp.AutoMapper;
using NewBlazorWebApp.Customers;
using AutoMapper;
using NewBlazorWebApp.Books;

namespace NewBlazorWebApp.Blazor.Client;

public class NewBlazorWebAppBlazorAutoMapperProfile : Profile
{
    public NewBlazorWebAppBlazorAutoMapperProfile()
    {
        CreateMap<BookDto, CreateUpdateBookDto>();

        //Define your AutoMapper configuration here for the Blazor project.

        CreateMap<CustomerDto, CustomerUpdateDto>();
    }
}