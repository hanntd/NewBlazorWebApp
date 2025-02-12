using NewBlazorWebApp.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NewBlazorWebApp.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class NewBlazorWebAppController : AbpControllerBase
{
    protected NewBlazorWebAppController()
    {
        LocalizationResource = typeof(NewBlazorWebAppResource);
    }
}
