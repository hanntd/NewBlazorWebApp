using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace NewBlazorWebApp.Data;

/* This is used if database provider does't define
 * INewBlazorWebAppDbSchemaMigrator implementation.
 */
public class NullNewBlazorWebAppDbSchemaMigrator : INewBlazorWebAppDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
