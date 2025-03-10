using ServiceLayer.Enums;
using ServiceLayer.Models;

namespace PhamNguyenTrongTuanRazorPages.Models.Category
{
    public class CategoryViewModel
    {
        [Display(Name = "Id")]
        public int CategoryId { get; set; }

        [Display(Name = "Category Name")]
        public required string CategoryName { get; set; }

        [Display(Name = "Total News")]
        public int NewsArticleCount { get; set; }

        [Display(Name = "Description")]
        public required string CategoryDesciption { get; set; }

        [Display(Name = "Status")]
        public required CategoryStatus CategoryStatus { get; set; }

        [Display(Name = "Parent")]
        public CategoryDTO? ParentCategory { get; set; }
    }
}
