using System;
using NewBlazorWebApp.Shared;
using Volo.Abp.AutoMapper;
using NewBlazorWebApp.Customers;
using AutoMapper;
using NewBlazorWebApp.Books;

namespace NewBlazorWebApp;

public class NewBlazorWebAppApplicationAutoMapperProfile : Profile
{
    public NewBlazorWebAppApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Customer, CustomerDto>();
        CreateMap<Customer, CustomerExcelDto>();
        CreateMap<AppFileDescriptors.AppFileDescriptor, AppFileDescriptorDto>();
    }
}