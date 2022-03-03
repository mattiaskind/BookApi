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
        // Hämta alla böcker i listan
        // /books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksAsync()
        {
            try
            {
                var books = await booksDb.GetBooksAsync();
                return Ok(books);
            }
            catch (ArgumentException)
            {

                return NotFound("Books not found");
            }     
            
        }

        // GET
        // /books/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookAsync(Guid id)
        {
            try
            {
                Book book = await booksDb.GetBookAsync(id);
                return Ok(book);
            }
            catch (ArgumentException)
            {

                throw;
            }
            
        }

        // POST
        // /books
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBookAsync(BookDTO bookDTO)
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


            // Return 400 if error

            await booksDb.CreateBookAsync(book);            
            return CreatedAtAction(nameof(GetBookAsync), new { id = book.Id }, book);
        }

        // PUT
        // /book/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBookAsync(Guid id, BookDTO bookDTO)
        {
            // Hämta den bok som ska uppdateras
            Book bookToUpdate = await booksDb.GetBookAsync(id);
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
            await booksDb.UpdateBookAsync(updatedBook);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(Guid id)
        {
            Book book = await booksDb.GetBookAsync(id);
            if (book is null) return NotFound();

            await booksDb.DeleteBookAsync(book);

            return NoContent();
        }
    }    
}
