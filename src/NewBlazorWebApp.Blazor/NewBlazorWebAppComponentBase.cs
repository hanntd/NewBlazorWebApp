using NewBlazorWebApp.Localization;
using Volo.Abp.AspNetCore.Components;

namespace NewBlazorWebApp.Blazor;

public abstract class NewBlazorWebAppComponentBase : AbpComponentBase
{
    protected NewBlazorWebAppComponentBase()
    {
        LocalizationResource = typeof(NewBlazorWebAppResource);
    }
}
