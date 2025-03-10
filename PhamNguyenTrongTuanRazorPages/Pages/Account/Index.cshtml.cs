using Microsoft.AspNetCore.Authorization;
using PhamNguyenTrongTuanRazorPages.Models.Account;
using ServiceLayer.Account;

namespace PhamNguyenTrongTuanRazorPages.Pages.Account;

[Authorize(Roles = "Admin")]
public class IndexModel(IAccountService accountService, IMapper mapper) : PageModel
{
    public PaginatedList<ViewAccountViewModel> SystemAccount { get; set; } = null!;

    public async Task OnGetAsync(
        string sortOrder,
        string currentFilter,
        string? searchString,
        int? pageNumber,
        int? pageSize
    )
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["NameSortParam"] = sortOrder == "name" ? "name_desc" : "name";
        ViewData["EmailSortParam"] = sortOrder == "email" ? "email_desc" : "email";
        ViewData["RoleSortParam"] = sortOrder == "role" ? "role_desc" : "role";

        if (searchString != null)
        {
            pageNumber = 1;
        }
        else
        {
            searchString = currentFilter;
        }
        ViewData["CurrentFilter"] = searchString;
        var accountDtos = await accountService.ListAccountsWithPaginationAndFiltering(
            searchString,
            sortOrder,
            pageNumber,
            pageSize
        );
        var viewAccountViewModel = mapper.Map<PaginatedList<ViewAccountViewModel>>(accountDtos);
        SystemAccount = new PaginatedList<ViewAccountViewModel>(
            viewAccountViewModel,
            accountDtos.TotalPages,
            accountDtos.TotalElements,
            accountDtos.PageIndex
        );
    }
}
