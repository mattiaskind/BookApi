using BookApi.Controllers;
using BookApi.Data;
using BookApi.Models;
using Microsoft.AspNetCore.Http;
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
        public async Task Get_All_Books_Success()
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
            var resultStatus = result.Result as OkObjectResult;

            Assert.IsInstanceOfType(result, typeof(ActionResult<List<Book>>));            
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

            Assert.AreEqual((int)resultStatus.StatusCode, (int)StatusCodes.Status200OK);
        }

        [TestMethod]
        public async Task Get_All_Books_NotFound()
        {
            var mockBooksDb = new Mock<IBooksDb>();
            mockBooksDb.Setup(x => x.GetBooksAsync()).ReturnsAsync(new List<Book>());

            var controller = new BooksController(mockBooksDb.Object);
            var result = await controller.GetBooksAsync();                        
            var resultStatus = result.Result as NotFoundObjectResult;           

            Assert.IsInstanceOfType(result, typeof(ActionResult<List<Book>>));
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
            // Kanske redundant med tanke på ovanstående
            Assert.AreEqual((int)resultStatus.StatusCode, (int)StatusCodes.Status404NotFound);
        }

        [TestMethod]
        public async Task Create_One_Book_Success()
        {
            // Från början ska inga objekt finnas i listan

            // Därefter skapas ett objekt

            // Listan ska returnera ett objekt
        }
    }
}