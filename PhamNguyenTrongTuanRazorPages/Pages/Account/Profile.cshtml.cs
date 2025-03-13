using System.Security.Claims;
using PhamNguyenTrongTuanRazorPages.Models.Account;
using ServiceLayer.Account;
using ServiceLayer.Models;

namespace PhamNguyenTrongTuanRazorPages.Pages.Account
{
    [Authorize(Roles = "Staff")]
    public class ProfileModel(IAccountService accountService, IMapper mapper) : PageModel
    {
        [BindProperty]
        public UpdateProfileViewModel UpdateProfileViewModel { get; set; } = null!;

        public async Task OnGetAsync()
        {
            var email = HttpContext.User.FindFirstValue(ClaimTypes.Email)!;
            var accountDto = await accountService.GetAcountByEmailAsync(email);
            UpdateProfileViewModel = mapper.Map<UpdateProfileViewModel>(accountDto);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var accountDto = mapper.Map<AccountDTO>(UpdateProfileViewModel);
            var effectedRow = await accountService.UpdateProfile(accountDto);
            if (effectedRow is null)
            {
                TempData["Error"] = "Update profile failed!!!";
                return Page();
            }
            TempData["Success"] = "Update profile successfully!!!";
            return Page();
        }
    }
}
