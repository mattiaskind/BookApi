using System.ComponentModel.DataAnnotations;

namespace BookApi.Models
{
    // Ett data transfer object används för att inte exponera ID-egenskapen
    // när det inte ska modifieras, exempelvis när ett nytt objekt skapas eller
    // när ett objekt ändras.    
    public class BookDTO
    {        
        [Required]
        [RegularExpression(@"^[a-öA-Ö'0-9 ]+$", ErrorMessage = "Otillåtna tecken.")]
        public string? Title { get; set; }
        
        [Required]
        [RegularExpression(@"^[a-öA-Ö'0-9 ]+$", ErrorMessage = "Otillåtna tecken.")]
        public string? Author { get; set; }
        
        [Required]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Otillåtna tecken.")]
        public int PageCount { get; set; }
        
        [Required]
        [RegularExpression(@"^[a-öA-Ö'0-9 ]+$", ErrorMessage = "Otillåtna tecken.")]
        public string? Departement { get; set; }
        
        [Required]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Otillåtna tecken.")]
        [StringLength(13, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 10)]
        public string? ISBN { get; set; }
    }
}
