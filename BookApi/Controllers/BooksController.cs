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
        // Hämta alla böcker i listan, returnera status 200 OK om det
        // finns böcker i listans, annars NotFound 404.
        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetBooksAsync()
        {            
            var books = await booksDb.GetBooksAsync();
            if (books is null || books.Count == 0) return NotFound("Ingen bok hittades");
            return Ok(books);
        }

        // GET
        // /books/{id}
        // Hämta en bok, Returnera boken tillsammans med status 200 OK om en bok hittas
        // annars returnera NotFound status 404.
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookAsync(Guid id)
        {
            var book = await booksDb.GetBookAsync(id);
            if (book is null) return NotFound("Ingen bok hittades med det angivna id:t");
            return Ok(book);                
        }

        // POST
        // /books
        // Lägg till en bok i listan
        // Skapa ett nytt bok-objekt. Returnera boken som skapades tillsammans med
        // status 201
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
        // /book/{id}
        // Uppdatera en bok som finns i listan och returnera status 204. 
        // Om ingen bok hittas returnera NotFound status 404.        
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
        // /book/{id}
        // Ta bort en bok från listan och returnera status 204.
        // Om ingen bok hittas, returnera 404.
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(Guid id)
        {           
            Book? book = await booksDb.GetBookAsync(id);
            if (book is null) return NotFound("Ingen bok hittades med angivet id");

            await booksDb.DeleteBookAsync(book);

            return NoContent();
        }
    }    
}
