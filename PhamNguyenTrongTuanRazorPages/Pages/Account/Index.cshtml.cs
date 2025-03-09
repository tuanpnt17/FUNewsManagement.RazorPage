using Microsoft.AspNetCore.Authorization;
using PhamNguyenTrongTuanRazorPages.Models.Account;
using ServiceLayer.Account;

namespace PhamNguyenTrongTuanRazorPages.Pages.Account;

[Authorize(Roles = "Admin")]
public class IndexModel(IAccountService accountService, IMapper mapper) : PageModel
{
    public IList<ViewAccountViewModel> SystemAccount { get; set; } = null!;

    public async Task OnGetAsync()
    {
        var accountDtos = await accountService.ListAllAccounts();
        SystemAccount = mapper.Map<IList<ViewAccountViewModel>>(accountDtos);
    }
}
