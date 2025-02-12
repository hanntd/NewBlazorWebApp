using System;

namespace NewBlazorWebApp.Customers
{
    public abstract class CustomerExcelDtoBase
    {
        public string Code { get; set; } = null!;
        public string? Name { get; set; }
        public string? Address { get; set; }
        public float Balance { get; set; }
    }
}