namespace BookApi.Models
{
    // Ett data transfer object används för att inte exponera ID egenskapen
    // ID:t skapas automatiskt när ett nytt objekt instansieras    
    public class CreateBookDTO
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int PageCount { get; set; }
        public string? Departement { get; set; }
        public string? ISBN { get; set; }
    }
}
