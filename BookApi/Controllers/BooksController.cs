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
    }

    
}
