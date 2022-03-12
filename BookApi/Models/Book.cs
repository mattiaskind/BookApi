namespace BookApi.Models
{
    public class Book
    {        
        public Guid Id { get; init; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int PageCount { get; set; }
        public string? Departement { get; set; }
        public string? ISBN { get; set; }
    }
}
