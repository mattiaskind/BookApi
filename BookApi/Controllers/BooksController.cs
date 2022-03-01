using BookApi.Data;
using BookApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{   
    //[Route("api/[controller]")]
    [Route("books")]
    [ApiController]
    public class BooksController : ControllerBase
    {        
        private IBooksDb booksDb;

        public BooksController(IBooksDb booksDb)
        {
            // Instans av klassen som agerar databas
            this.booksDb = booksDb;
        }

        // GET
        // /books
        [HttpGet]
        public IEnumerable<Book> GetBooks()
        {
            return booksDb.GetBooks();
        }

        // GET
        // /books/{id}
        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(Guid id)
        {
            Book book = booksDb.GetBook(id);            
            if(book is null)
            {
                return NotFound();
            }
            return book;
        }

        // POST
        // /books
        [HttpPost]
        public ActionResult<Book> CreateBook(BookDTO bookDTO)
        {
            // Skapa ett nytt bok-objekt
            Book book = new Book
            {
                Id = Guid.NewGuid(),
                Title = bookDTO.Title,
                Author = bookDTO.Author,
                PageCount = bookDTO.PageCount,
                Departement = bookDTO.Departement,
                ISBN = bookDTO.ISBN,
            };

            booksDb.CreateBook(book);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // PUT
        // /book/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateBook(Guid id, BookDTO bookDTO)
        {
            // Hämta den bok som ska uppdateras
            Book bookToUpdate = booksDb.GetBook(id);
            if(bookToUpdate is null) return NotFound();

            // Skapa ett nytt objekt baserat på det nya dto-objektet men
            // utgå från existerande id
            Book updatedBook = new Book
            {
                Id = bookToUpdate.Id,
                Title = bookDTO.Title,
                Author = bookDTO.Author,
                PageCount = bookDTO.PageCount,
                Departement = bookDTO.Departement,
                ISBN = bookDTO.ISBN,
            };

            // Updatera aktuell bok med den nya informationen
            booksDb.UpdateBook(updatedBook);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteBook(Guid id)
        {
            Book book = booksDb.GetBook(id);
            if (book is null) return NotFound();

            booksDb.DeleteBook(book);

            return NoContent();
        }
    }    
}
