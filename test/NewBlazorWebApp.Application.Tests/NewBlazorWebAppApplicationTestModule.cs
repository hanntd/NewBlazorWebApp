using Volo.Abp.Modularity;

namespace NewBlazorWebApp;

[DependsOn(
    typeof(NewBlazorWebAppApplicationModule),
    typeof(NewBlazorWebAppDomainTestModule)
)]
public class NewBlazorWebAppApplicationTestModule : AbpModule
{

}
