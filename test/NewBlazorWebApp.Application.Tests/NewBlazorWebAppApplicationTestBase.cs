using Volo.Abp.Modularity;

namespace NewBlazorWebApp;

public abstract class NewBlazorWebAppApplicationTestBase<TStartupModule> : NewBlazorWebAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
