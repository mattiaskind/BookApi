using BookApi.Models;

namespace BookApi.Data
{
    // Ett interface som databas-klassen implementerar och som används för dependency injection
    public interface IBooksDb
    {
        Book GetBook(Guid id);
        IEnumerable<Book> GetBooks();
    }
}