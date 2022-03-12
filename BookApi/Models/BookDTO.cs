using System.ComponentModel.DataAnnotations;

namespace BookApi.Models
{
    // Ett data transfer object används för att inte exponera ID-egenskapen
    // när den inte ska modifieras, exempelvis när ett nytt objekt skapas eller
    // när ett objekt ändras.    
    public class BookDTO
    {        
        [Required]
        [RegularExpression(@"^[a-öA-Ö'0-9 :-]+$", ErrorMessage = "Titeln innehåller otillåtna tecken")]
        public string? Title { get; set; }
        
        [Required]
        [RegularExpression(@"^[a-öA-Ö'0-9 ]+$", ErrorMessage = "Författar-fältet innehåller otillåtna tecken")]
        public string? Author { get; set; }
        
        [Required]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Sidantalet kan bara bestå av siffror")]
        public int PageCount { get; set; }
        
        [Required]
        [RegularExpression(@"^[a-öA-Ö'0-9 ]+$", ErrorMessage = "Fältet för avdelning innehåller otillåtna tecken")]
        public string? Departement { get; set; }
        
        [Required]
        [RegularExpression(@"[0-9]+", ErrorMessage = "ISBN kan bara bestå av siffror")]
        [StringLength(13, ErrorMessage = "Längden på ISBN måste vara mellan {2} och {1} siffror", MinimumLength = 10)]
        public string? ISBN { get; set; }
    }
}
