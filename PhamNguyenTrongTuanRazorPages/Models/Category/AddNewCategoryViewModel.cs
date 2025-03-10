using ServiceLayer.Enums;

namespace PhamNguyenTrongTuanRazorPages.Models.Category
{
    public class AddNewCategoryViewModel
    {
        [Display(Name = "Category Name")]
        [Required]
        public required string CategoryName { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [Required]
        public required string CategoryDesciption { get; set; }

        [Display(Name = "Status")]
        [Required]
        public required CategoryStatus CategoryStatus { get; set; }

        [Display(Name = "Parent")]
        public int? ParentCategoryId { get; set; }
    }
}
