namespace NewBlazorWebApp.Customers
{
    public static class CustomerConsts
    {
        private const string DefaultSorting = "{0}Code asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Customer." : string.Empty);
        }

    }
}