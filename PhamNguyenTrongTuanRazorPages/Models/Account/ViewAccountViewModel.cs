using System.ComponentModel.DataAnnotations;
using ServiceLayer.Enums;

namespace PhamNguyenTrongTuanRazorPages.Models.Account
{
    public class ViewAccountViewModel
    {
        [Display(Name = "Account Id")]
        public int AccountId { get; set; }

        [Display(Name = "Name")]
        public required string AccountName { get; set; }

        [Display(Name = "Email")]
        public required string AccountEmail { get; set; }

        [Display(Name = "Role")]
        public AccountRole AccountRole { get; set; }
    }
}
