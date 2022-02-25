namespace BookApi.Models
{
    public class Book
    {
        // Init så att inte ID:t kan ändras efter objektets instansiering
        public Guid Id { get; init; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int PageCount { get; set; }
        public string? Departement { get; set; }
        public string? ISBN { get; set; }
    }


}
