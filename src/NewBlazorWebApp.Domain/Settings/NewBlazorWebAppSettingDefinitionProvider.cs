using Volo.Abp.Settings;

namespace NewBlazorWebApp.Settings;

public class NewBlazorWebAppSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(NewBlazorWebAppSettings.MySetting1));
    }
}
