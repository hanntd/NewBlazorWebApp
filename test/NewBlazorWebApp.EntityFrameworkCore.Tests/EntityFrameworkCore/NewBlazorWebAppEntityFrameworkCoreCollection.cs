using Xunit;

namespace NewBlazorWebApp.EntityFrameworkCore;

[CollectionDefinition(NewBlazorWebAppTestConsts.CollectionDefinitionName)]
public class NewBlazorWebAppEntityFrameworkCoreCollection : ICollectionFixture<NewBlazorWebAppEntityFrameworkCoreFixture>
{

}
