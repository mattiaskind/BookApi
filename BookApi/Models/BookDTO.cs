﻿using System.ComponentModel.DataAnnotations;

namespace BookApi.Models
{
    // Ett data transfer object används för att inte exponera ID-egenskapen
    // ID:t skapas automatiskt när ett nytt objekt instansieras    
    public class BookDTO
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Author { get; set; }
        [Required]
        public int PageCount { get; set; }
        [Required]
        public string? Departement { get; set; }
        [Required]
        [StringLength(13, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 10)]
        public string? ISBN { get; set; }
    }
}