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

        [HttpGet]
        public IEnumerable<Book> GetBooks()
        {
            return booksDb.GetBooks();
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(Guid id)
        {
            var book = booksDb.GetBook(id);            
            if(book is null)
            {
                return NotFound();
            }
            return book;
        }

        [HttpPost]
        public ActionResult<Book> CreateBook(Book _book)
        {
            Book book = new Book
            {
                Id = Guid.NewGuid(),
                Title = _book.Title,
                Author = _book.Author,
                PageCount = _book.PageCount,
                Departement = _book.Departement,
                ISBN = _book.ISBN,
            };

            booksDb.CreateBook(book);

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }
    }

    
}
