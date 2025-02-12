using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace NewBlazorWebApp.Customers
{
    public abstract class CustomerDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Code { get; set; } = null!;
        public string? Name { get; set; }
        public string? Address { get; set; }
        public float Balance { get; set; }
        public Guid DocumentsId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

    }
}