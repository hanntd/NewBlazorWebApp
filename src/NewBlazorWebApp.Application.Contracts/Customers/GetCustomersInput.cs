using Volo.Abp.Application.Dtos;
using System;

namespace NewBlazorWebApp.Customers
{
    public abstract class GetCustomersInputBase : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public float? BalanceMin { get; set; }
        public float? BalanceMax { get; set; }

        public GetCustomersInputBase()
        {

        }
    }
}