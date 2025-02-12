using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Web;
using Blazorise;
using Blazorise.DataGrid;
using Volo.Abp.BlazoriseUI.Components;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using NewBlazorWebApp.Customers;
using NewBlazorWebApp.Permissions;
using NewBlazorWebApp.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Volo.Abp;
using Volo.Abp.Content;



namespace NewBlazorWebApp.Blazor.Client.Pages
{
    public partial class Customers
    {
        [Inject]
        protected IJSRuntime JsRuntime { get; set; }
            
        private IJSObjectReference? _jsObjectRef;
            
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        protected bool ShowAdvancedFilters { get; set; }
        private IReadOnlyList<CustomerDto> CustomerList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateCustomer { get; set; }
        private bool CanEditCustomer { get; set; }
        private bool CanDeleteCustomer { get; set; }
        private CustomerCreateDto NewCustomer { get; set; }
        private Validations NewCustomerValidations { get; set; } = new();
        private CustomerUpdateDto EditingCustomer { get; set; }
        private Validations EditingCustomerValidations { get; set; } = new();
        private Guid EditingCustomerId { get; set; }
        private Modal CreateCustomerModal { get; set; } = new();
        private Modal EditCustomerModal { get; set; } = new();
        private GetCustomersInput Filter { get; set; }
        private DataGridEntityActionsColumn<CustomerDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "customer-create-tab";
        protected string SelectedEditTab = "customer-edit-tab";
        private CustomerDto? SelectedCustomer;
        
        
        
        
        
        private List<CustomerDto> SelectedCustomers { get; set; } = new();
        private bool AllCustomersSelected { get; set; }
        
        public Customers()
        {
            NewCustomer = new CustomerCreateDto();
            EditingCustomer = new CustomerUpdateDto();
            Filter = new GetCustomersInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            CustomerList = new List<CustomerDto>();
            
            
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsObjectRef = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "/Pages/Customers.razor.js");
                await SetBreadcrumbItemsAsync();
                await SetToolbarItemsAsync();
                await InvokeAsync(StateHasChanged);
            }
        }  

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Customers"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewCustomer"], async () =>
            {
                await OpenCreateCustomerModalAsync();
            }, IconName.Add, requiredPolicyName: NewBlazorWebAppPermissions.Customers.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateCustomer = await AuthorizationService
                .IsGrantedAsync(NewBlazorWebAppPermissions.Customers.Create);
            CanEditCustomer = await AuthorizationService
                            .IsGrantedAsync(NewBlazorWebAppPermissions.Customers.Edit);
            CanDeleteCustomer = await AuthorizationService
                            .IsGrantedAsync(NewBlazorWebAppPermissions.Customers.Delete);
                            
                            
        }

        private async Task GetCustomersAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await CustomersAppService.GetListAsync(Filter);
            CustomerList = result.Items;
            TotalCount = (int)result.TotalCount;
            
            await ClearSelection();
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetCustomersAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task DownloadAsExcelAsync()
        {
            var token = (await CustomersAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("NewBlazorWebApp") ?? await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if(!culture.IsNullOrEmpty())
            {
                culture = "&culture=" + culture;
            }
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/app/customers/as-excel-file?DownloadToken={token}&FilterText={HttpUtility.UrlEncode(Filter.FilterText)}{culture}&Code={HttpUtility.UrlEncode(Filter.Code)}&Name={HttpUtility.UrlEncode(Filter.Name)}&Address={HttpUtility.UrlEncode(Filter.Address)}&BalanceMin={Filter.BalanceMin}&BalanceMax={Filter.BalanceMax}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<CustomerDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetCustomersAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateCustomerModalAsync()
        {
            NewCustomer = new CustomerCreateDto{
                
                
            };

            SelectedCreateTab = "customer-create-tab";
            
            await _jsObjectRef!.InvokeVoidAsync("FileCleanup.clearInputFiles");
            await NewCustomerValidations.ClearAll();
            await CreateCustomerModal.Show();
        }

        private async Task CloseCreateCustomerModalAsync()
        {
            NewCustomer = new CustomerCreateDto{
                
                
            };
            await CreateCustomerModal.Hide();
        }

        private async Task OpenEditCustomerModalAsync(CustomerDto input)
        {
            SelectedEditTab = "customer-edit-tab";
            
            await _jsObjectRef!.InvokeVoidAsync("FileCleanup.clearInputFiles");
            var customer = await CustomersAppService.GetAsync(input.Id);
            
            EditingCustomerId = customer.Id;
            EditingCustomer = ObjectMapper.Map<CustomerDto, CustomerUpdateDto>(customer);
            HasSelectedCustomerDocuments = EditingCustomer.DocumentsId != null && EditingCustomer.DocumentsId != Guid.Empty;

            await EditingCustomerValidations.ClearAll();
            await EditCustomerModal.Show();
        }

        private async Task DeleteCustomerAsync(CustomerDto input)
        {
            await CustomersAppService.DeleteAsync(input.Id);
            await GetCustomersAsync();
        }

        private async Task CreateCustomerAsync()
        {
            try
            {
                if (await NewCustomerValidations.ValidateAll() == false)
                {
                    return;
                }

                await CustomersAppService.CreateAsync(NewCustomer);
                await GetCustomersAsync();
                await CloseCreateCustomerModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditCustomerModalAsync()
        {
            await EditCustomerModal.Hide();
        }

        private async Task UpdateCustomerAsync()
        {
            try
            {
                if (await EditingCustomerValidations.ValidateAll() == false)
                {
                    return;
                }

                await CustomersAppService.UpdateAsync(EditingCustomerId, EditingCustomer);
                await GetCustomersAsync();
                await EditCustomerModal.Hide();                
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private void OnSelectedCreateTabChanged(string name)
        {
            SelectedCreateTab = name;
        }

        private void OnSelectedEditTabChanged(string name)
        {
            SelectedEditTab = name;
        }


        private bool IsCreateFormDisabled()
        {
            return OnNewCustomerDocumentsLoading ||NewCustomer.DocumentsId == Guid.Empty ;
        }
        
        private bool IsEditFormDisabled()
        {
            return OnEditCustomerDocumentsLoading ||EditingCustomer.DocumentsId == Guid.Empty ;
        }



        private int MaxCustomerDocumentsFileUploadSize = 1024 * 1024 * 10; //10MB
        private bool OnNewCustomerDocumentsLoading = false;
        private async Task OnNewCustomerDocumentsChanged(InputFileChangeEventArgs e)
        {
            try
            {
                if (e.FileCount is 0 or > 1 || e.File.Size > MaxCustomerDocumentsFileUploadSize)
                {
                    throw new UserFriendlyException(L["UploadFailedMessage"]);
                }
    
                OnNewCustomerDocumentsLoading = true;
                
                var result = await UploadFileAsync(e.File!);
    
                NewCustomer.DocumentsId = result.Id;
                OnNewCustomerDocumentsLoading = false;            
            }
            catch(Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }
        private bool HasSelectedCustomerDocuments = false;
        private bool OnEditCustomerDocumentsLoading = false;
        private async Task OnEditCustomerDocumentsChanged(InputFileChangeEventArgs e)
        {
            try
            {
                if (e.FileCount is 0 or > 1 || e.File.Size > MaxCustomerDocumentsFileUploadSize)
                {
                    throw new UserFriendlyException(L["UploadFailedMessage"]);
                }
    
                OnEditCustomerDocumentsLoading = true;
                
                var result = await UploadFileAsync(e.File!);
    
                EditingCustomer.DocumentsId = result.Id;
                OnEditCustomerDocumentsLoading = false;            
            }
            catch(Exception ex)
            {
                await HandleErrorAsync(ex);
            }            
        }




        private async Task<AppFileDescriptorDto> UploadFileAsync(IBrowserFile file)
        {
            using (var ms = new MemoryStream())
            {
                await file.OpenReadStream(long.MaxValue).CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
                
                return await CustomersAppService.UploadFileAsync(new RemoteStreamContent(ms, file.Name, file.ContentType));
            }
        }



        private async Task DownloadFileAsync(Guid fileId)
        {
            var token = (await CustomersAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("NewBlazorWebApp") ?? await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/app/customers/file?DownloadToken={token}&FileId={fileId}", forceLoad: true);
        }

        protected virtual async Task OnCodeChangedAsync(string? code)
        {
            Filter.Code = code;
            await SearchAsync();
        }
        protected virtual async Task OnNameChangedAsync(string? name)
        {
            Filter.Name = name;
            await SearchAsync();
        }
        protected virtual async Task OnAddressChangedAsync(string? address)
        {
            Filter.Address = address;
            await SearchAsync();
        }
        protected virtual async Task OnBalanceMinChangedAsync(float? balanceMin)
        {
            Filter.BalanceMin = balanceMin;
            await SearchAsync();
        }
        protected virtual async Task OnBalanceMaxChangedAsync(float? balanceMax)
        {
            Filter.BalanceMax = balanceMax;
            await SearchAsync();
        }
        





        private Task SelectAllItems()
        {
            AllCustomersSelected = true;
            
            return Task.CompletedTask;
        }

        private Task ClearSelection()
        {
            AllCustomersSelected = false;
            SelectedCustomers.Clear();
            
            return Task.CompletedTask;
        }

        private Task SelectedCustomerRowsChanged()
        {
            if (SelectedCustomers.Count != PageSize)
            {
                AllCustomersSelected = false;
            }
            
            return Task.CompletedTask;
        }

        private async Task DeleteSelectedCustomersAsync()
        {
            var message = AllCustomersSelected ? L["DeleteAllRecords"].Value : L["DeleteSelectedRecords", SelectedCustomers.Count].Value;
            
            if (!await UiMessageService.Confirm(message))
            {
                return;
            }

            if (AllCustomersSelected)
            {
                await CustomersAppService.DeleteAllAsync(Filter);
            }
            else
            {
                await CustomersAppService.DeleteByIdsAsync(SelectedCustomers.Select(x => x.Id).ToList());
            }

            SelectedCustomers.Clear();
            AllCustomersSelected = false;

            await GetCustomersAsync();
        }


    }
}
