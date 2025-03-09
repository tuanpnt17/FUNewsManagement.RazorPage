using PhamNguyenTrongTuanRazorPages.Models.Account;
using ServiceLayer.Account;
using ServiceLayer.Models;

namespace PhamNguyenTrongTuanRazorPages.Pages.Account;

public class CreateModel(IAccountService accountService, IMapper mapper) : PageModel
{
    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public AddNewAccountViewModel SystemAccount { get; set; } = null!;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (await IsExistedEmail(SystemAccount.AccountEmail))
        {
            ModelState.AddModelError("SystemAccount.AccountEmail", "Email is already existed");
            return Page();
        }

        var accountDto = mapper.Map<AccountDTO>(SystemAccount);
        var result = await accountService.CreateNewAccountAsync(accountDto);
        return RedirectToPage("./Index");
    }

    private async Task<bool> IsExistedEmail(string systemAccountAccountEmail)
    {
        var account = await accountService.GetAcountByEmailAsync(systemAccountAccountEmail);
        return account != null;
    }
}
