using BookApi.Models;
using Microsoft.AspNetCore.Mvc;

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
        
        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            // Data som ska returneras finns redan tillgänglig i listan med böcker
            return await Task.FromResult(Books);
        }
      
        public async Task<Book> GetBookAsync(Guid id)
        {
            var book = Books.Find(book => book.Id == id);
            return await Task.FromResult(book);
        }
                
        public async Task CreateBookAsync(Book book)
        {
            Books.Add(book);

            await Task.CompletedTask;
        }
        
        public async Task UpdateBookAsync(Book book)
        {
            var index = Books.FindIndex(b => b.Id == book.Id);
            Books[index] = book;
            
            await Task.CompletedTask;
        }
        
        public async Task DeleteBookAsync(Book book)
        {
            Books.Remove(book);

            await Task.CompletedTask;

        }
    }
}
