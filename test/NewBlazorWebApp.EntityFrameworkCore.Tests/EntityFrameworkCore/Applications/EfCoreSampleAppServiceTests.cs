using NewBlazorWebApp.Samples;
using Xunit;

namespace NewBlazorWebApp.EntityFrameworkCore.Applications;

[Collection(NewBlazorWebAppTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<NewBlazorWebAppEntityFrameworkCoreTestModule>
{

}
