using NewBlazorWebApp.Localization;
using Volo.Abp.AspNetCore.Components;

namespace NewBlazorWebApp.Blazor.Client;

public abstract class NewBlazorWebAppComponentBase : AbpComponentBase
{
    protected NewBlazorWebAppComponentBase()
    {
        LocalizationResource = typeof(NewBlazorWebAppResource);
    }
}
