using Microsoft.AspNetCore.Authorization;

namespace PhamNguyenTrongTuanRazorPages.Pages.Account;

[Authorize(Roles = "Admin")]
public class DeleteModel(Repository.Data.FuNewsDbContext context) : PageModel
{
    [BindProperty]
    public SystemAccount SystemAccount { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(short? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var systemaccount = await context.SystemAccounts.FirstOrDefaultAsync(m =>
            m.AccountId == id
        );

        if (systemaccount == null)
        {
            return NotFound();
        }
        else
        {
            SystemAccount = systemaccount;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(short? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var systemaccount = await context.SystemAccounts.FindAsync(id);
        if (systemaccount != null)
        {
            SystemAccount = systemaccount;
            context.SystemAccounts.Remove(SystemAccount);
            await context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
