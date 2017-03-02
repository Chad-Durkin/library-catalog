using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LibraryCatalog
{
    public class AuthorsTest : IDisposable
    {
        public AuthorsTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
        }


        [Fact]
        public void Test_AuthorsEmptyAtFirst()
        {
            //Arrange, ACt
            int result = Author.GetAll().Count;

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_FindByNameFindsAuthorInDatabaseById()
        {
            //Arrange
            Author testAuthor = new Author("Mark Twain");
            testAuthor.Save();

            Book testBook = new Book("The Giver", "1970-09-01");
            testBook.Save();

            //Act
            testBook.AddAuthor(testAuthor.GetId());
            Book result = Book.FindByAuthorName(testAuthor.GetName());

            //Assert
            Assert.Equal(testBook, result);
        }

        [Fact]
        public void Test_FindAuthorByIdFindsAuthorInDatabaseById()
        {
            //Arrange
            Author testAuthor = new Author("Mark Twain");
            testAuthor.Save();

            Book testBook = new Book("The Giver", "1970-09-01");
            testBook.Save();

            //Act
            testBook.AddAuthor(testAuthor.GetId());

            Author result = Author.FindAuthorById(testAuthor.GetId());

            //Assert
            Assert.Equal(testAuthor, result);
        }

        public void Dispose()
        {
            Book.DeleteAll();
            Author.DeleteAll();
            Patron.DeleteAll();
            Copy.DeleteAll();
            Checkout.DeleteAll();
        }
    }
}
