namespace PhamNguyenTrongTuanRazorPages.Models.Category
{
    public class ParentCategoryViewModel
    {
        public int CategoryId { get; set; }

        [Display(Name = "Parent Category")]
        public required string CategoryName { get; set; }
    }
}
