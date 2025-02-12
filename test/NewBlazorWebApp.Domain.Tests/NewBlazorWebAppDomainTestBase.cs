using Volo.Abp.Modularity;

namespace NewBlazorWebApp;

/* Inherit from this class for your domain layer tests. */
public abstract class NewBlazorWebAppDomainTestBase<TStartupModule> : NewBlazorWebAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
