using Volo.Abp.Modularity;

namespace NewBlazorWebApp;

[DependsOn(
    typeof(NewBlazorWebAppDomainModule),
    typeof(NewBlazorWebAppTestBaseModule)
)]
public class NewBlazorWebAppDomainTestModule : AbpModule
{

}
