using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewBlazorWebApp.Data;
using Volo.Abp.DependencyInjection;

namespace NewBlazorWebApp.EntityFrameworkCore;

public class EntityFrameworkCoreNewBlazorWebAppDbSchemaMigrator
    : INewBlazorWebAppDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreNewBlazorWebAppDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the NewBlazorWebAppDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<NewBlazorWebAppDbContext>()
            .Database
            .MigrateAsync();
    }
}
