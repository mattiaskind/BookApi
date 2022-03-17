using BookApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Data
{
    // Ett interface som databas-klassen implementerar och som används för dependency injection
    public interface IBooksDb
    {
        /// <summary>
        /// Hämtar en specifik bok i samlingen
        /// </summary>
        /// <param name="id">Bokens id</param>
        /// <returns>Ett objekt av typen Book, annars null</returns>
        Task<Book?> GetBookAsync(Guid id);

        /// <summary>
        /// Hämtar en specifik bok utifrån ISBN
        /// </summary>
        /// <param name="isbn">Bokens ISBN</param>
        /// <returns>Ett objekt av typen Book, annars null</returns>
        Task<Book?> GetBookByIsbnAsync(string isbn);

        /// <summary>
        /// Lägger till en bok till samlingen
        /// </summary>
        /// <param name="book">Boken som ska läggas till. Ett objekt av typen Book</param>
        Task CreateBookAsync(Book book);

        /// <summary>
        /// Hämtar böckerna som finns lagrade i instansen av klassen
        /// </summary>
        /// <returns>List av typen Book</returns>
        Task<List<Book>> GetBooksAsync();

        /// <summary>
        /// Uppdatera informationen för en befintlig bok i samlingen
        /// </summary>
        /// <param name="book">Boken som ska uppdateras. Ett objekt av typen Book</param>
        Task UpdateBookAsync(Book book);

        /// <summary>
        /// Ta bort en bok i samlingen
        /// </summary>
        /// <param name="book">Boken som ska tas bort. Ett objekt av typen Book</param>
        Task DeleteBookAsync(Book book);
        
    }
}