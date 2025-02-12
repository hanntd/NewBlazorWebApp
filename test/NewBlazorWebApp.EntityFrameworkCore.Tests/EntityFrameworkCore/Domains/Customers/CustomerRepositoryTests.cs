using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using NewBlazorWebApp.Customers;
using NewBlazorWebApp.EntityFrameworkCore;
using Xunit;

namespace NewBlazorWebApp.EntityFrameworkCore.Domains.Customers
{
    public class CustomerRepositoryTests : NewBlazorWebAppEntityFrameworkCoreTestBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerRepositoryTests()
        {
            _customerRepository = GetRequiredService<ICustomerRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _customerRepository.GetListAsync(
                    code: "e6bf1bbecbba4f86be828a745b675d931c99c74c792244aba72777acc24f6b0b7013b",
                    name: "2c636df46ceb4b8b907d89b3deab",
                    address: "859b9e4a8aae4bb4aa38e4d6510d2bd37e9aeb1fc97a4c919c3ad4f97ae5ad620f8fbbd88de047b79"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("6e33e628-26a0-4c6b-b3ed-a431a043063f"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _customerRepository.GetCountAsync(
                    code: "8fce12947f0f48eb8673bcc939407b555aab32969f9d44d2bd1747c6a0a34a841754d8eb8e0c4653b8",
                    name: "580dd16a78f941858f6b467de6528d75a1c42f5942a5449",
                    address: "355738a577ee41e8b0322ff12be9c4d289bd58e8429b45f7b9fe67a3ed256"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}