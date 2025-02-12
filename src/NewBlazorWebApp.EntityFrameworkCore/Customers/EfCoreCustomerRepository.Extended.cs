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
    public class EfCoreCustomerRepository : EfCoreCustomerRepositoryBase, ICustomerRepository
    {
        public EfCoreCustomerRepository(IDbContextProvider<NewBlazorWebAppDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}