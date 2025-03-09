using PhamNguyenTrongTuanRazorPages.Models.Account;
using ServiceLayer.Account;
using ServiceLayer.Models;

namespace PhamNguyenTrongTuanRazorPages.Pages.Account;

public class EditModel(IAccountService accountService, IMapper mapper) : PageModel
{
    [BindProperty]
    public UpdateAccountViewModel SystemAccount { get; set; } = null!;

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
        SystemAccount = mapper.Map<UpdateAccountViewModel>(accountDto);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var accountDto = mapper.Map<AccountDTO>(SystemAccount);
        var result = await accountService.UpdateAccountAsync(accountDto);
        if (result == null)
        {
            ModelState.AddModelError("SystemAccount.AccountEmail", "Email is already existed");
            return Page();
        }
        return RedirectToPage("./Index");
    }
}
