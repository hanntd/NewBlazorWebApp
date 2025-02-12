using Microsoft.Extensions.Localization;
using NewBlazorWebApp.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace NewBlazorWebApp.Blazor;

[Dependency(ReplaceServices = true)]
public class NewBlazorWebAppBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<NewBlazorWebAppResource> _localizer;

    public NewBlazorWebAppBrandingProvider(IStringLocalizer<NewBlazorWebAppResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
