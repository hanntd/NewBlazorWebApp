using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace NewBlazorWebApp.Customers
{
    public abstract class CustomersAppServiceTests<TStartupModule> : NewBlazorWebAppApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly ICustomersAppService _customersAppService;
        private readonly IRepository<Customer, Guid> _customerRepository;

        public CustomersAppServiceTests()
        {
            _customersAppService = GetRequiredService<ICustomersAppService>();
            _customerRepository = GetRequiredService<IRepository<Customer, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _customersAppService.GetListAsync(new GetCustomersInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("6e33e628-26a0-4c6b-b3ed-a431a043063f")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("6d0d1be9-07bf-49ca-a7ef-a4980071f70f")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _customersAppService.GetAsync(Guid.Parse("6e33e628-26a0-4c6b-b3ed-a431a043063f"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("6e33e628-26a0-4c6b-b3ed-a431a043063f"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new CustomerCreateDto
            {
                Code = "7d888573c48141dd920bc95d89de",
                Name = "861a051bcfb64978a",
                Address = "4faa8a15ce084e0689bab170610a5d9ca7",
                Balance = 256897460,
                DocumentsId = Guid.Parse("1428283195")
            };

            // Act
            var serviceResult = await _customersAppService.CreateAsync(input);

            // Assert
            var result = await _customerRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Code.ShouldBe("7d888573c48141dd920bc95d89de");
            result.Name.ShouldBe("861a051bcfb64978a");
            result.Address.ShouldBe("4faa8a15ce084e0689bab170610a5d9ca7");
            result.Balance.ShouldBe(256897460);
            result.DocumentsId.ShouldBe(Guid.Parse("1428283195"));
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new CustomerUpdateDto()
            {
                Code = "14ba100ec0e040ba8e88ec6a4fd2ee472e8d34e3c12745039c69bc4ec65f9381575cf49be57d43f48d863eae3",
                Name = "cbab8654d33846f68d8c973df8e52376cebcd70a1f94479d90b5",
                Address = "6303ccd6d94e429f860b6c2c4d553850b1264d",
                Balance = 463815059,
                DocumentsId = Guid.Parse("639875101")
            };

            // Act
            var serviceResult = await _customersAppService.UpdateAsync(Guid.Parse("6e33e628-26a0-4c6b-b3ed-a431a043063f"), input);

            // Assert
            var result = await _customerRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Code.ShouldBe("14ba100ec0e040ba8e88ec6a4fd2ee472e8d34e3c12745039c69bc4ec65f9381575cf49be57d43f48d863eae3");
            result.Name.ShouldBe("cbab8654d33846f68d8c973df8e52376cebcd70a1f94479d90b5");
            result.Address.ShouldBe("6303ccd6d94e429f860b6c2c4d553850b1264d");
            result.Balance.ShouldBe(463815059);
            result.DocumentsId.ShouldBe(Guid.Parse("639875101"));
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _customersAppService.DeleteAsync(Guid.Parse("6e33e628-26a0-4c6b-b3ed-a431a043063f"));

            // Assert
            var result = await _customerRepository.FindAsync(c => c.Id == Guid.Parse("6e33e628-26a0-4c6b-b3ed-a431a043063f"));

            result.ShouldBeNull();
        }
    }
}