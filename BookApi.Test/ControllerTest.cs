using BookApi.Controllers;
using BookApi.Data;
using BookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BookApi.Test
{
    [TestClass]
    public class ControllerTest
    {        
        [TestMethod]
        public async Task Get_All_Books_Successful()
        {
            var mockBooksDb = new Mock<IBooksDb>();
            mockBooksDb.Setup(x => x.GetBooksAsync()).ReturnsAsync(new List<Book>()
            {
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "Title1",
                    Author = "Författare1",
                    Departement = "D1",
                    PageCount = 300,
                    ISBN = "9789180230000"
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "Title2",
                    Author = "Author2",
                    Departement = "D2",
                    PageCount = 350,
                    ISBN = "9789113110000"
                }
            });           

            var controller = new BooksController(mockBooksDb.Object);
            var result = await controller.GetBooksAsync();
            var listOfBooks = (result.Result as OkObjectResult).Value as List<Book>;            
            Assert.AreEqual(2, listOfBooks.Count);
            Assert.IsInstanceOfType(result, typeof(ActionResult<List<Book>>));
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));        
        }

        [TestMethod]
        public async Task Get_All_Books_Empty_List_Return_NotFound()
        {
            var mockBooksDb = new Mock<IBooksDb>();
            mockBooksDb.Setup(x => x.GetBooksAsync()).ReturnsAsync(new List<Book>());

            var controller = new BooksController(mockBooksDb.Object);
            var result = await controller.GetBooksAsync();                        
            var resultStatus = result.Result as NotFoundObjectResult;           

            Assert.IsInstanceOfType(result, typeof(ActionResult<List<Book>>));
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));                        
        }

        [TestMethod]
        public async Task Create_Book_Successful()
        {
            var mockBooksDb = new Mock<IBooksDb>();
            var controller = new BooksController(mockBooksDb.Object);
            
            BookDTO b = new BookDTO
            {
                Title = "Title",
                Author = "Author",
                PageCount = 100,
                Departement = "D",
                ISBN = "0123456789"
            };
            
            var resultCreate = await controller.CreateBookAsync(b);                        
            var createdBook = (resultCreate.Result as CreatedAtActionResult).Value as Book;                       

            
            Assert.AreEqual(b.Title, createdBook.Title);
            Assert.AreEqual(b.Author, createdBook.Author);
            Assert.AreEqual(b.PageCount, createdBook.PageCount);
            Assert.AreEqual(b.Departement, createdBook.Departement);
            Assert.AreEqual(b.ISBN, createdBook.ISBN);
        }

        [TestMethod]
        public async Task Update_Book_Successful()
        {
            var mockBooksDb = new Mock<IBooksDb>();
            var controller = new BooksController(mockBooksDb.Object);
                        
            Book bookInCollection = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Title1",
                Author = "Författare1",
                Departement = "D1",
                PageCount = 300,
                ISBN = "9789180230000"
            };
            mockBooksDb.Setup(controller => controller.GetBookAsync(It.IsAny<Guid>())).ReturnsAsync(bookInCollection);

            string newTitle = "Updated title";            
            BookDTO bookToUpdate = new BookDTO
            {
                Title = newTitle,
                Author = "Författare1",
                Departement = "D1",
                PageCount = 300,
                ISBN = "9789180230000"
            };
            
            var result = controller.UpdateBookAsync(bookInCollection.Id, bookToUpdate);
            
            Assert.IsInstanceOfType(result.Result, typeof(NoContentResult));
        }
    }
}