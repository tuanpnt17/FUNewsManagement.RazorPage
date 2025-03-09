using Microsoft.AspNetCore.Authorization;
using PhamNguyenTrongTuanRazorPages.Models.Account;
using ServiceLayer.Account;

namespace PhamNguyenTrongTuanRazorPages.Pages.Account;

[Authorize(Roles = "Admin")]
public class DetailsModel(IAccountService accountService, IMapper mapper) : PageModel
{
    public ViewAccountViewModel SystemAccount { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var accountDto = await accountService.GetAcountByIdAsync(id);
        if (accountDto == null)
        {
            return NotFound();
        }
        SystemAccount = mapper.Map<ViewAccountViewModel>(accountDto);
        return Page();
    }
}
