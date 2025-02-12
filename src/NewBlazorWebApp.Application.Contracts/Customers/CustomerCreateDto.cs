using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace NewBlazorWebApp.Customers
{
    public abstract class CustomerCreateDtoBase
    {
        [Required]
        public string Code { get; set; } = null!;
        public string? Name { get; set; }
        public string? Address { get; set; }
        public float Balance { get; set; }
        public Guid DocumentsId { get; set; }
    }
}