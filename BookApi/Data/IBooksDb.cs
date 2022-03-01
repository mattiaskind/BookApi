using BookApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Data
{
    // Ett interface som databas-klassen implementerar och som används för dependency injection
    public interface IBooksDb
    {
        /// <summary>
        /// Söker efter ett bok-objekt med angivet id
        /// </summary>
        /// <param name="id">Unikt id för önskad bok</param>
        /// <returns>Objekt av typ Book om bok hittas med aktuellt id, annars null</returns>
        /// 

        /// <summary>
        /// Hämtar en bok i samlingen
        /// </summary>
        /// <param name="id">Bokens id</param>
        /// <returns>Ett objekt av typen Book, annars null</returns>
        Book GetBook(Guid id);

        /// <summary>
        /// Lägger till en bok till samlingen
        /// </summary>
        /// <param name="book">Boken som ska läggas till. Ett objekt av typen Book</param>
        public void CreateBook(Book book);

        /// <summary>
        /// Hämtar böckerna som finns lagrade i instansen av klassen
        /// </summary>
        /// <returns>IEnumerable av typen Book</returns>
        IEnumerable<Book> GetBooks();

        /// <summary>
        /// Uppdatera informationen för en befintlig bok i samlingen
        /// </summary>
        /// <param name="book">Boken som ska uppdateras. Ett objekt av typen Book</param>
        public void UpdateBook(Book book);

        /// <summary>
        /// Ta bort en bok i samlingen
        /// </summary>
        /// <param name="book">Boken som ska tas bort. Ett objekt av typen Book</param>
        public void DeleteBook(Book book);
        
    }
}