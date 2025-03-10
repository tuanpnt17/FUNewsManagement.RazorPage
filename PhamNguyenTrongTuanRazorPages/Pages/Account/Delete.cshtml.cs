using Microsoft.AspNetCore.Authorization;
using ServiceLayer.Account;
using ServiceLayer.Models;

namespace PhamNguyenTrongTuanRazorPages.Pages.Account;

[Authorize(Roles = "Admin")]
public class DeleteModel(IAccountService accountService, IMapper mapper) : PageModel
{
    [BindProperty]
    public AccountDTO SystemAccount { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var accountDto = await accountService.GetAcountByIdAsync((int)id);

        if (accountDto == null)
        {
            return NotFound();
        }
        else
        {
            SystemAccount = accountDto;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var rowEffected = await accountService.DeleteAccountAsync((int)id);
        if (rowEffected == null)
            return Page();
        return RedirectToPage("./Index");
    }
}
