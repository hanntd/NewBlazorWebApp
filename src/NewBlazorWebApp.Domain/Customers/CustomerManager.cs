using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace NewBlazorWebApp.Customers
{
    public abstract class CustomerManagerBase : DomainService
    {
        protected ICustomerRepository _customerRepository;

        public CustomerManagerBase(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public virtual async Task<Customer> CreateAsync(
        string code, float balance, Guid documentsId, string? name = null, string? address = null)
        {
            Check.NotNullOrWhiteSpace(code, nameof(code));

            var customer = new Customer(
             GuidGenerator.Create(),
             code, balance, documentsId, name, address
             );

            return await _customerRepository.InsertAsync(customer);
        }

        public virtual async Task<Customer> UpdateAsync(
            Guid id,
            string code, float balance, Guid documentsId, string? name = null, string? address = null, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(code, nameof(code));

            var customer = await _customerRepository.GetAsync(id);

            customer.Code = code;
            customer.Balance = balance;
            customer.DocumentsId = documentsId;
            customer.Name = name;
            customer.Address = address;

            customer.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _customerRepository.UpdateAsync(customer);
        }

    }
}