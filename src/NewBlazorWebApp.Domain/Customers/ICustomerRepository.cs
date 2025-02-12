using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NewBlazorWebApp.Customers
{
    public partial interface ICustomerRepository : IRepository<Customer, Guid>
    {

        Task DeleteAllAsync(
            string? filterText = null,
            string? code = null,
            string? name = null,
            string? address = null,
            float? balanceMin = null,
            float? balanceMax = null,
            CancellationToken cancellationToken = default);
        Task<List<Customer>> GetListAsync(
                    string? filterText = null,
                    string? code = null,
                    string? name = null,
                    string? address = null,
                    float? balanceMin = null,
                    float? balanceMax = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? code = null,
            string? name = null,
            string? address = null,
            float? balanceMin = null,
            float? balanceMax = null,
            CancellationToken cancellationToken = default);
    }
}