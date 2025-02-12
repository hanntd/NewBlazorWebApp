using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace NewBlazorWebApp.Customers
{
    public abstract class CustomerBase : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Code { get; set; }

        [CanBeNull]
        public virtual string? Name { get; set; }

        [CanBeNull]
        public virtual string? Address { get; set; }

        public virtual float Balance { get; set; }

        public virtual Guid DocumentsId { get; set; }

        protected CustomerBase()
        {

        }

        public CustomerBase(Guid id, string code, float balance, Guid documentsId, string? name = null, string? address = null)
        {

            Id = id;
            Check.NotNull(code, nameof(code));
            Code = code;
            Balance = balance;
            DocumentsId = documentsId;
            Name = name;
            Address = address;
        }

    }
}