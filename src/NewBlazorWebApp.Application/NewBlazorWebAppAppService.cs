using NewBlazorWebApp.Localization;
using Volo.Abp.Application.Services;

namespace NewBlazorWebApp;

/* Inherit your application services from this class.
 */
public abstract class NewBlazorWebAppAppService : ApplicationService
{
    protected NewBlazorWebAppAppService()
    {
        LocalizationResource = typeof(NewBlazorWebAppResource);
    }
}
