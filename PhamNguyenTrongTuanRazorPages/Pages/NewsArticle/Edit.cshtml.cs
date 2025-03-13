using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using PhamNguyenTrongTuanRazorPages.Hubs;
using PhamNguyenTrongTuanRazorPages.Models.NewsArticle;
using ServiceLayer.Account;
using ServiceLayer.Category;
using ServiceLayer.Models;
using ServiceLayer.NewsArticle;
using ServiceLayer.Tag;

namespace PhamNguyenTrongTuanRazorPages.Pages.NewsArticle
{
    [Authorize(Roles = "Staff")]
    public class EditModel(
        ICategoryService categoryService,
        IAccountService accountService,
        ITagService tagService,
        INewsArticleService newsArticleService,
        IMapper mapper,
        IHubContext<SignalRServer> hubContext
    ) : PageModel
    {
        private readonly IHubContext<SignalRServer> _hubContext = hubContext;

        [BindProperty]
        public UpdateNewsArticleViewModel NewsArticle { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticleDto = await newsArticleService.GetNewsArticleByIdAsync(id);
            if (newsArticleDto == null)
            {
                return NotFound();
            }
            var updateNewsArticleViewModel = mapper.Map<UpdateNewsArticleViewModel>(newsArticleDto);

            NewsArticle = updateNewsArticleViewModel;

            ViewData["CategoryId"] = new SelectList(
                await categoryService.GetCategoriesAsync(),
                "CategoryId",
                "CategoryName"
            );
            ViewData["Tags"] = await tagService.GetAllTagsAsync();
            ViewData["UpdatedByName"] = (
                (await accountService.GetAcountByIdAsync(newsArticleDto.UpdatedById))!
            ).AccountName;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(
                    await categoryService.GetCategoriesAsync(),
                    "CategoryId",
                    "CategoryName"
                );
                ViewData["Tags"] = await tagService.GetAllTagsAsync();
                return Page();
            }
            var newsArticleDto = mapper.Map<NewsArticleDTO>(NewsArticle);
            var currentUserId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Sid)!.Value;
            var updatedNewsArticle = await newsArticleService.UpdateNewsArticleAsync(
                newsArticleDto,
                currentUserId
            );
            if (updatedNewsArticle <= 0)
                return BadRequest();
            await _hubContext.Clients.All.SendAsync("LoadArticles");
            return RedirectToPage("./Index");
        }
    }
}
