using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using PhamNguyenTrongTuanRazorPages.Hubs;
using PhamNguyenTrongTuanRazorPages.Models.NewsArticle;
using ServiceLayer.Category;
using ServiceLayer.Models;
using ServiceLayer.NewsArticle;
using ServiceLayer.Tag;

namespace PhamNguyenTrongTuanRazorPages.Pages.NewsArticle
{
    public class CreateModel(
        INewsArticleService newsArticleService,
        IMapper mapper,
        ICategoryService categoryService,
        ITagService tagService,
        IHubContext<SignalRServer> hubContext
    ) : PageModel
    {
        private readonly IHubContext<SignalRServer> _hubContext = hubContext;

        public async Task<IActionResult> OnGet()
        {
            SelectListItems = new SelectList(
                await categoryService.GetCategoriesAsync(),
                "CategoryId",
                "CategoryName"
            );
            TagDtos = await tagService.GetAllTagsAsync();
            NewsArticle = new();
            return Page();
        }

        [BindProperty]
        public AddNewsArticleViewModel NewsArticle { get; set; } = null!;

        public IEnumerable<TagDTO> TagDtos { get; set; } = [];
        public SelectList SelectListItems { get; set; } = null!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newsArticleDto = mapper.Map<NewsArticleDTO>(NewsArticle);

            var currentUserId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Sid)!.Value;
            await newsArticleService.CreateNewsArticleAsync(newsArticleDto, currentUserId);
            await _hubContext.Clients.All.SendAsync("LoadArticles");
            return RedirectToPage("./Index");
        }
    }
}
