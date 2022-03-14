using BookApi.Data;
using BookApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApi.Test
{   
    
    [TestClass]
    public class DataServiceTest
    {

        public BooksDb BooksDatabase;

        public DataServiceTest()
        {
            BooksDatabase = new BooksDb();
        }

        [TestMethod]
        public async Task GetBooks_Returns_List_Of_Two_Items()
        {
            var books = await BooksDatabase.GetBooksAsync();
            Assert.AreEqual(2, books.Count);
        }

        [TestMethod]
        public async Task GetBook_Returns_A_Book()
        {
            var books = await BooksDatabase.GetBooksAsync();
            var bookId = books[0].Id;

            var book = await BooksDatabase.GetBookAsync(bookId);
            Assert.IsNotNull(book);
            Assert.AreEqual(book.Id, bookId);
        }

        [TestMethod]
        public async Task Add_One_Book()
        {
            Book book = new Book
            {
                Id = Guid.NewGuid(),
                Title = Guid.NewGuid().ToString(),
                Author = Guid.NewGuid().ToString(),
                Departement = Guid.NewGuid().ToString(),
                PageCount = 500,
                ISBN = "0000000000"
            };
            await BooksDatabase.CreateBookAsync(book);

            var books = await BooksDatabase.GetBooksAsync();
            Assert.AreEqual(3, books.Count);
        }

        [TestMethod]
        public async Task Delete_Book()
        {
            var books = await BooksDatabase.GetBooksAsync();
            var numberOfBooks = books.Count;            
            await BooksDatabase.DeleteBookAsync(books[0]);

            books = await BooksDatabase.GetBooksAsync();
            Assert.AreEqual(books.Count, (numberOfBooks - 1));
        }


    }
}
