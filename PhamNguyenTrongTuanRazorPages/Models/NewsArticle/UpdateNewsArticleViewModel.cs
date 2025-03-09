﻿using System.ComponentModel.DataAnnotations;
using ServiceLayer.Enums;
using ServiceLayer.Models;

namespace PhamNguyenTrongTuanRazorPages.Models.NewsArticle
{
    public class UpdateNewsArticleViewModel
    {
        [Required]
        [StringLength(20)]
        [Display(Name = "Article Id")]
        public string NewsArticleId { get; set; } = string.Empty;

        [Required]
        [StringLength(400)]
        [Display(Name = "Title")]
        public string NewsTitle { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        [Display(Name = "Headline")]
        public string Headline { get; set; } = string.Empty;

        [DataType(DataType.MultilineText)]
        [Display(Name = "Content")]
        public string? NewsContent { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Thumbnail")]
        public string? NewsSource { get; set; }

        [Required]
        [Display(Name = "Status")]
        public NewsStatus NewsStatus { get; set; } = NewsStatus.Active;

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Tags")]
        public int[] TagIds { get; set; } = [];

        [Display(Name = "Last Modified Date")]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Created By")]
        public string CreatedByName { get; set; } = string.Empty;

        [Display(Name = "Last Modified By")]
        public string UpdatedByName { get; set; } = string.Empty;

        //For dropdown
        public IEnumerable<CategoryDTO> Categories { get; set; } = [];

        public IEnumerable<TagDTO> Tags { get; set; } = [];
    }
}
