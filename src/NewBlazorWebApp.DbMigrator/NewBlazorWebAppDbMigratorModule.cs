using NewBlazorWebApp.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace NewBlazorWebApp.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(NewBlazorWebAppEntityFrameworkCoreModule),
    typeof(NewBlazorWebAppApplicationContractsModule)
)]
public class NewBlazorWebAppDbMigratorModule : AbpModule
{
}
