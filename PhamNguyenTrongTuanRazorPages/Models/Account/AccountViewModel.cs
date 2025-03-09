using System.ComponentModel.DataAnnotations;
using ServiceLayer.Enums;

namespace PhamNguyenTrongTuanRazorPages.Models.Account
{
    public class AccountViewModel
    {
        public IEnumerable<ViewAccountViewModel>? AllAccounts { get; set; }

        [Display(Name = "Name")]
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [DataType(DataType.Text)]
        public required string AccountName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public required string AccountEmail { get; set; }

        [Display(Name = "Role")]
        [Required]
        public AccountRole AccountRole { get; set; }
    }
}
