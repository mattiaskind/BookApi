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
        Book GetBook(Guid id);

        /// <summary>
        /// Lägger till en bok till samlingen
        /// </summary>
        /// <param name="book">Objektet som ska läggas till</param>
        /// <returns></returns>
        public void CreateBook(Book book);

        /// <summary>
        /// Returnerar samtliga böcker som finns lagrade i samlingen
        /// </summary>
        /// <returns>IEnumerable innehållandes objekt av typen Book</returns>
        IEnumerable<Book> GetBooks();

        /// <summary>
        /// Updaterar en bok
        /// </summary>
        /// <param name="book">Det objekt av typ Book som ska uppdateras</param>
        public void UpdateBook(Book book);
        
    }
}