using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace LinkAggregatorMVC.Models
{
    public class Links
    {
        public int ID { get; set; }
        [StringLength(80, MinimumLength = 3)]
        [Required]
        public string? Caption { get; set; }
        [Required]
        public string? URL { get; set; }
        [Display(Name = "Link Posting Date")]
        [DataType(DataType.Date)]
        public DateTime LinkPostingDate { get; set; }
        public int? Rating { get; set; }
        public int? Importance { get; set; }
        public int? TotalClicks { get; set; }
        public ICollection<LinkCategories>? LinkCategories { get; set; } = new List<LinkCategories>();
        public ICollection<LinkImages>? LinkImages { get; set; } = new List<LinkImages>();
        [NotMapped]
        public List<IFormFile>? Files { get; set; }
        [NotMapped]
        public string? Message { get; set; }
        [NotMapped]
        public List<SelectListItem> Categories { get; set; } = [];
        [NotMapped]
        public int[]? SelectedCategories { get; set; }
    }
    
    public class LinkCategories
    {
        public int ID { get; set; }
        public Links? Links { get; set; }
        public int? CategoriesLookup { get; set; }

    }
    public class CategoriesLookup
    {
        public CategoriesLookup() { }
        public int ID { get; set; }
        [StringLength(40)]
        [Required]
        public string? Category;
    }
    public class LinkImages
    {
        public int ID { get; set; }
        public string? ImagePath { get; set; }
        public Links? Links { get; set; }
    }
  
}
