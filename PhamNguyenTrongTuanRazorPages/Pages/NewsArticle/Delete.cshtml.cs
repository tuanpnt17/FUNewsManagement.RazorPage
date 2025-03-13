using Microsoft.AspNetCore.SignalR;
using PhamNguyenTrongTuanRazorPages.Hubs;
using PhamNguyenTrongTuanRazorPages.Models.NewsArticle;
using ServiceLayer.NewsArticle;

namespace PhamNguyenTrongTuanRazorPages.Pages.NewsArticle
{
    [Authorize(Roles = "Staff")]
    public class DeleteModel(
        INewsArticleService newsArticleService,
        IMapper mapper,
        IHubContext<SignalRServer> hubContext
    ) : PageModel
    {
        [BindProperty]
        public ViewNewsArticleViewModel NewsArticle { get; set; } = null!;

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

            NewsArticle = mapper.Map<ViewNewsArticleViewModel>(newsArticleDto);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deleteEffected = await newsArticleService.DeleteNewsArticleAsync(id);
            if (!(deleteEffected > 0))
                return BadRequest();
            await hubContext.Clients.All.SendAsync("LoadArticles");
            return RedirectToPage("./Index");
        }
    }
}
