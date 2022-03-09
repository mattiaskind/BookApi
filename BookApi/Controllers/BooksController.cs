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
        public async Task<ActionResult<List<Book>>> GetBooksAsync()
        {            
            var books = await booksDb.GetBooksAsync();
            if (books is null || books.Count == 0) return NotFound("Ingen bok hittades");
            return Ok(books);
        }

        // GET
        // Hämta en bok
        // /books/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookAsync(Guid id)
        {
            var book = await booksDb.GetBookAsync(id);
            if (book is null) return NotFound("Ingen bok hittades med det angivna id:t");
            return Ok(book);                
        }

        // POST
        // Lägg till en bok i listan
        // /books
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBookAsync(BookDTO bookDTO)
        {
            // Skapa ett nytt bok-objekt. Generera ID automatiskt
            Book book = new Book
            {
                Id = Guid.NewGuid(),
                Title = bookDTO.Title,
                Author = bookDTO.Author,
                PageCount = bookDTO.PageCount,
                Departement = bookDTO.Departement,
                ISBN = bookDTO.ISBN,
            };

            await booksDb.CreateBookAsync(book);   
            // Skicka tillbaka objektet som skapades tillsammans med statuskod 201
            return CreatedAtAction(nameof(GetBookAsync), new { id = book.Id }, book);
        }

        // PUT
        // Uppdatera en bok som finns i listan
        // /book/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBookAsync(Guid id, BookDTO bookDTO)
        {
            // Hämta den bok som ska uppdateras
            Book? bookToUpdate = await booksDb.GetBookAsync(id);
            // 404  
            if(bookToUpdate is null) return NotFound("Ingen bok hittades med angivet id");

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

        // DELETE
        // Ta bort en bok från listan
        // /book/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(Guid id)
        {
            // 400 om fel, bad request
            // 404 om not found
            Book? book = await booksDb.GetBookAsync(id);
            if (book is null) return NotFound("Ingen bok hittades med angivet id");

            await booksDb.DeleteBookAsync(book);

            return NoContent();
        }
    }    
}
