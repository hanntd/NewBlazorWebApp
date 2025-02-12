using NewBlazorWebApp.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace NewBlazorWebApp.Permissions;

public class NewBlazorWebAppPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(NewBlazorWebAppPermissions.GroupName);

        myGroup.AddPermission(NewBlazorWebAppPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
        myGroup.AddPermission(NewBlazorWebAppPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

        var booksPermission = myGroup.AddPermission(NewBlazorWebAppPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(NewBlazorWebAppPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(NewBlazorWebAppPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(NewBlazorWebAppPermissions.Books.Delete, L("Permission:Books.Delete"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(NewBlazorWebAppPermissions.MyPermission1, L("Permission:MyPermission1"));

        var customerPermission = myGroup.AddPermission(NewBlazorWebAppPermissions.Customers.Default, L("Permission:Customers"));
        customerPermission.AddChild(NewBlazorWebAppPermissions.Customers.Create, L("Permission:Create"));
        customerPermission.AddChild(NewBlazorWebAppPermissions.Customers.Edit, L("Permission:Edit"));
        customerPermission.AddChild(NewBlazorWebAppPermissions.Customers.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<NewBlazorWebAppResource>(name);
    }
}