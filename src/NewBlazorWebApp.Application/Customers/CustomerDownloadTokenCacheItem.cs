using System;

namespace NewBlazorWebApp.Customers;

public abstract class CustomerDownloadTokenCacheItemBase
{
    public string Token { get; set; } = null!;
}