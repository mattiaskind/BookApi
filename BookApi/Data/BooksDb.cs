using BookApi.Models;

namespace BookApi.Data
{
    public class BooksDb : IBooksDb
    {
        public List<Book> Books = new()
        {
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Snöblind",
                Author = "Ragnar Jónasson",
                Departement = "Deckare",
                PageCount = 272,
                ISBN = "9789180234825"
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Trion",
                Author = "Johanna Hedman",
                Departement = "Skönlitteratur",
                PageCount = 355,
                ISBN = "9789113119526"
            }
        };

        public IEnumerable<Book> GetBooks()
        {
            return Books;
        }

        public Book GetBook(Guid id)
        {
            return Books.Find(book => book.Id == id);
        }

        public void CreateBook(Book book)
        {
            Books.Add(book);
        }
    }
}
