using NewBlazorWebApp.Samples;
using Xunit;

namespace NewBlazorWebApp.EntityFrameworkCore.Domains;

[Collection(NewBlazorWebAppTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<NewBlazorWebAppEntityFrameworkCoreTestModule>
{

}
