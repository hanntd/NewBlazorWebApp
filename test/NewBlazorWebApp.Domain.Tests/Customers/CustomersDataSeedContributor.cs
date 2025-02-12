using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using NewBlazorWebApp.Customers;

namespace NewBlazorWebApp.Customers
{
    public class CustomersDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CustomersDataSeedContributor(ICustomerRepository customerRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _customerRepository = customerRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _customerRepository.InsertAsync(new Customer
            (
                id: Guid.Parse("6e33e628-26a0-4c6b-b3ed-a431a043063f"),
                code: "e6bf1bbecbba4f86be828a745b675d931c99c74c792244aba72777acc24f6b0b7013b",
                name: "2c636df46ceb4b8b907d89b3deab",
                address: "859b9e4a8aae4bb4aa38e4d6510d2bd37e9aeb1fc97a4c919c3ad4f97ae5ad620f8fbbd88de047b79",
                balance: 1500165894,
                documentsId: Guid.Parse("1375434749")
            ));

            await _customerRepository.InsertAsync(new Customer
            (
                id: Guid.Parse("6d0d1be9-07bf-49ca-a7ef-a4980071f70f"),
                code: "8fce12947f0f48eb8673bcc939407b555aab32969f9d44d2bd1747c6a0a34a841754d8eb8e0c4653b8",
                name: "580dd16a78f941858f6b467de6528d75a1c42f5942a5449",
                address: "355738a577ee41e8b0322ff12be9c4d289bd58e8429b45f7b9fe67a3ed256",
                balance: 2071743728,
                documentsId: Guid.Parse("1934544115")
            ));

            await _unitOfWorkManager!.Current!.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}