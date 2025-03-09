using System.ComponentModel.DataAnnotations;
using ServiceLayer.Enums;

namespace PhamNguyenTrongTuanRazorPages.Models.Category
{
    public class UpdateCategoryViewModel
    {
        [Required]
        public int CategoryId { get; set; }

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
    }
}
