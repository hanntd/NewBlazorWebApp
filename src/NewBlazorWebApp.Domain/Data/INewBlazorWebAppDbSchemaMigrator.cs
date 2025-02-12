using System.Threading.Tasks;

namespace NewBlazorWebApp.Data;

public interface INewBlazorWebAppDbSchemaMigrator
{
    Task MigrateAsync();
}
