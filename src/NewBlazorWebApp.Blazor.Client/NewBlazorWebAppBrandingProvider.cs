using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using NewBlazorWebApp.Localization;

namespace NewBlazorWebApp.Blazor.Client;

public class NewBlazorWebAppBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<NewBlazorWebAppResource> _localizer;

    public NewBlazorWebAppBrandingProvider(IStringLocalizer<NewBlazorWebAppResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
