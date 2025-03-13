using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using PhamNguyenTrongTuanRazorPages.Models.Account;
using ServiceLayer.Account;
using ServiceLayer.Enums;

namespace PhamNguyenTrongTuanRazorPages.Pages.Account;

public class LoginModel(IAccountService accountService, IOptions<AdminOptions> options) : PageModel
{
    private readonly AdminOptions _adminOption = options.Value;

    [BindProperty]
    public LoginAccountViewModel LoginAccountViewModel { get; set; }

    public string? ReturnUrl { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl ??= Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // If user is not admin
        if (_adminOption.Email != LoginAccountViewModel.Email)
        {
            var accountDto = await accountService.LoginAsync(
                LoginAccountViewModel.Email,
                LoginAccountViewModel.Password
            );

            if (accountDto == null)
            {
                ModelState.AddModelError("Email", "Email or password is incorrect");
                return Page();
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Sid, accountDto.AccountId.ToString()),
                new(ClaimTypes.Name, accountDto.AccountName),
                new(ClaimTypes.Email, accountDto.AccountEmail),
                new(ClaimTypes.Role, accountDto.AccountRole.ToString()),
            };

            var principal = new ClaimsPrincipal(
                new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)
            );
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = LoginAccountViewModel.RememberMe }
            );
            if (accountDto.AccountRole == AccountRole.Staff)
            {
                return RedirectToPage("/NewsArticle/Index");
            }

            return RedirectToPage("/Index");
        }

        // If user is admin
        if (_adminOption.Password != LoginAccountViewModel.Password)
        {
            ModelState.AddModelError("Password", "Email or password is incorrect");
            return Page();
        }

        var adminClaims = new List<Claim>
        {
            new(ClaimTypes.Sid, _adminOption.Email),
            new(ClaimTypes.Name, _adminOption.Name),
            new(ClaimTypes.Email, _adminOption.Email),
            new(ClaimTypes.Role, AccountRole.Admin.ToString()),
        };

        var adminPrincipal = new ClaimsPrincipal(
            new ClaimsIdentity(adminClaims, CookieAuthenticationDefaults.AuthenticationScheme)
        );
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            adminPrincipal,
            new AuthenticationProperties { IsPersistent = LoginAccountViewModel.RememberMe }
        );
        return RedirectToPage("/Dashboard/Index");
    }
}
