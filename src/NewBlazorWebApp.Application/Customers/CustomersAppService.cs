using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using NewBlazorWebApp.Permissions;
using NewBlazorWebApp.Customers;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using NewBlazorWebApp.Shared;
using Volo.Abp.BlobStoring;

namespace NewBlazorWebApp.Customers
{

    [Authorize(NewBlazorWebAppPermissions.Customers.Default)]
    public abstract class CustomersAppServiceBase : NewBlazorWebAppAppService
    {
        protected IDistributedCache<CustomerDownloadTokenCacheItem, string> _downloadTokenCache;
        protected ICustomerRepository _customerRepository;
        protected CustomerManager _customerManager;
        protected IRepository<AppFileDescriptors.AppFileDescriptor, Guid> _appFileDescriptorRepository;
        protected IBlobContainer<CustomerFileContainer> _blobContainer;

        public CustomersAppServiceBase(ICustomerRepository customerRepository, CustomerManager customerManager, IDistributedCache<CustomerDownloadTokenCacheItem, string> downloadTokenCache, IRepository<AppFileDescriptors.AppFileDescriptor, Guid> appFileDescriptorRepository, IBlobContainer<CustomerFileContainer> blobContainer)
        {
            _downloadTokenCache = downloadTokenCache;
            _customerRepository = customerRepository;
            _customerManager = customerManager;
            _appFileDescriptorRepository = appFileDescriptorRepository;
            _blobContainer = blobContainer;
        }

        public virtual async Task<PagedResultDto<CustomerDto>> GetListAsync(GetCustomersInput input)
        {
            var totalCount = await _customerRepository.GetCountAsync(input.FilterText, input.Code, input.Name, input.Address, input.BalanceMin, input.BalanceMax);
            var items = await _customerRepository.GetListAsync(input.FilterText, input.Code, input.Name, input.Address, input.BalanceMin, input.BalanceMax, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<CustomerDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Customer>, List<CustomerDto>>(items)
            };
        }

        public virtual async Task<CustomerDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Customer, CustomerDto>(await _customerRepository.GetAsync(id));
        }

        [Authorize(NewBlazorWebAppPermissions.Customers.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _customerRepository.DeleteAsync(id);
        }

        [Authorize(NewBlazorWebAppPermissions.Customers.Create)]
        public virtual async Task<CustomerDto> CreateAsync(CustomerCreateDto input)
        {

            var customer = await _customerManager.CreateAsync(
            input.Code, input.Balance, input.DocumentsId, input.Name, input.Address
            );

            return ObjectMapper.Map<Customer, CustomerDto>(customer);
        }

        [Authorize(NewBlazorWebAppPermissions.Customers.Edit)]
        public virtual async Task<CustomerDto> UpdateAsync(Guid id, CustomerUpdateDto input)
        {

            var customer = await _customerManager.UpdateAsync(
            id,
            input.Code, input.Balance, input.DocumentsId, input.Name, input.Address, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Customer, CustomerDto>(customer);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(CustomerExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _customerRepository.GetListAsync(input.FilterText, input.Code, input.Name, input.Address, input.BalanceMin, input.BalanceMax);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Customer>, List<CustomerExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Customers.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(NewBlazorWebAppPermissions.Customers.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> customerIds)
        {
            await _customerRepository.DeleteManyAsync(customerIds);
        }

        [Authorize(NewBlazorWebAppPermissions.Customers.Delete)]
        public virtual async Task DeleteAllAsync(GetCustomersInput input)
        {
            await _customerRepository.DeleteAllAsync(input.FilterText, input.Code, input.Name, input.Address, input.BalanceMin, input.BalanceMax);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetFileAsync(GetFileInput input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var fileDescriptor = await _appFileDescriptorRepository.GetAsync(input.FileId);
            var stream = await _blobContainer.GetAsync(fileDescriptor.Id.ToString("N"));

            return new RemoteStreamContent(stream, fileDescriptor.Name, fileDescriptor.MimeType);
        }

        public virtual async Task<AppFileDescriptorDto> UploadFileAsync(IRemoteStreamContent input)
        {
            var id = GuidGenerator.Create();
            var fileDescriptor = await _appFileDescriptorRepository.InsertAsync(new AppFileDescriptors.AppFileDescriptor(id, input.FileName, input.ContentType));

            await _blobContainer.SaveAsync(fileDescriptor.Id.ToString("N"), input.GetStream());

            return ObjectMapper.Map<AppFileDescriptors.AppFileDescriptor, AppFileDescriptorDto>(fileDescriptor);
        }

        public virtual async Task<NewBlazorWebApp.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new CustomerDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new NewBlazorWebApp.Shared.DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}