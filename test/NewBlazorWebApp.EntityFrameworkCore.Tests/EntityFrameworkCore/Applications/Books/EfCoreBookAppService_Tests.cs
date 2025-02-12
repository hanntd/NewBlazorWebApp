using NewBlazorWebApp.Books;
using Xunit;

namespace NewBlazorWebApp.EntityFrameworkCore.Applications.Books;

[Collection(NewBlazorWebAppTestConsts.CollectionDefinitionName)]
public class EfCoreBookAppService_Tests : BookAppService_Tests<NewBlazorWebAppEntityFrameworkCoreTestModule>
{

}