using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using NewBlazorWebApp.Shared;

namespace NewBlazorWebApp.Customers
{
    public partial interface ICustomersAppService : IApplicationService
    {
        Task<IRemoteStreamContent> GetFileAsync(GetFileInput input);

        Task<AppFileDescriptorDto> UploadFileAsync(IRemoteStreamContent input);

        Task<PagedResultDto<CustomerDto>> GetListAsync(GetCustomersInput input);

        Task<CustomerDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<CustomerDto> CreateAsync(CustomerCreateDto input);

        Task<CustomerDto> UpdateAsync(Guid id, CustomerUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(CustomerExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> customerIds);

        Task DeleteAllAsync(GetCustomersInput input);
        Task<NewBlazorWebApp.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}