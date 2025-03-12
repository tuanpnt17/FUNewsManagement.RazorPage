using ServiceLayer.Models;

namespace PhamNguyenTrongTuanRazorPages.Models.NewsArticle
{
    public class NewsArticleViewModel
    {
        public string NewsArticleId { get; set; } = string.Empty;
        public string NewsTitle { get; set; } = string.Empty;
        public string Headline { get; set; } = string.Empty;
        public string NewsContent { get; set; } = string.Empty;
        public AccountDTO CreatedBy { get; set; } = null!;
        public CategoryDTO Category { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public IList<TagDTO> Tags { get; set; } = new List<TagDTO>();
    }
}
