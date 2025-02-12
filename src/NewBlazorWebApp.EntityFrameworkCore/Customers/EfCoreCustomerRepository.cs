using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using NewBlazorWebApp.EntityFrameworkCore;

namespace NewBlazorWebApp.Customers
{
    public abstract class EfCoreCustomerRepositoryBase : EfCoreRepository<NewBlazorWebAppDbContext, Customer, Guid>
    {
        public EfCoreCustomerRepositoryBase(IDbContextProvider<NewBlazorWebAppDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task DeleteAllAsync(
            string? filterText = null,
                        string? code = null,
            string? name = null,
            string? address = null,
            float? balanceMin = null,
            float? balanceMax = null,
            CancellationToken cancellationToken = default)
        {

            var query = await GetQueryableAsync();

            query = ApplyFilter(query, filterText, code, name, address, balanceMin, balanceMax);

            var ids = query.Select(x => x.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Customer>> GetListAsync(
            string? filterText = null,
            string? code = null,
            string? name = null,
            string? address = null,
            float? balanceMin = null,
            float? balanceMax = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, code, name, address, balanceMin, balanceMax);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? CustomerConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? code = null,
            string? name = null,
            string? address = null,
            float? balanceMin = null,
            float? balanceMax = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, code, name, address, balanceMin, balanceMax);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Customer> ApplyFilter(
            IQueryable<Customer> query,
            string? filterText = null,
            string? code = null,
            string? name = null,
            string? address = null,
            float? balanceMin = null,
            float? balanceMax = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Code!.Contains(filterText!) || e.Name!.Contains(filterText!) || e.Address!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(code), e => e.Code.Contains(code))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.Name.Contains(name))
                    .WhereIf(!string.IsNullOrWhiteSpace(address), e => e.Address.Contains(address))
                    .WhereIf(balanceMin.HasValue, e => e.Balance >= balanceMin!.Value)
                    .WhereIf(balanceMax.HasValue, e => e.Balance <= balanceMax!.Value);
        }
    }
}