using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LibraryCatalog
{
    public class BooksTest : IDisposable
    {
        public BooksTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_BooksEmptyAtFirst()
        {
            //Arrange, ACt
            int result = Book.GetAll().Count;

            //Assert
            Assert.Equal(0, result);
        }


        [Fact]
        public void Test_Save()
        {
            //Arrange
            Book testBook = new Book("The Giver", "1970-09-01");
            testBook.Save();

            //Act
            List<Book> result = Book.GetAll();
            List<Book> testList = new List<Book>{testBook};

            //Assert
            Assert.Equal(testList, result);
        }

        [Fact]
        public void Test_Add_AssignsAuthorToABook()
        {
            //Arrange
            Author testAuthor = new Author("John Hancock");
            testAuthor.Save();
            Book testBook = new Book("The Giver", "1970-09-01");
            testBook.Save();

            //Act
            testBook.AddAuthor(testAuthor.GetId());
            List<Author> allAuthors = testBook.GetAuthors();
            List<Author> result = new List<Author>{testAuthor};

            //Assert
            Assert.Equal(result, allAuthors);
        }

        [Fact]
        public void Test_FindByIdFindsBooksInDatabaseById()
        {
            //Arrange
            Book testBook = new Book("The Giver", "1970-09-01");
            testBook.Save();

            //Act
            Book result = Book.FindById(testBook.GetId());

            //Assert
            Assert.Equal(testBook, result);
        }

        [Fact]
        public void Test_DeleteBook_DeleteBookFromDatabase()
        {
            //Arrange
            Book testBook = new Book("The Giver", "1970-09-01");
            testBook.Save();

            Book testBook1 = new Book("Grapes of Wrath", "1960-09-01");
            testBook1.Save();

            //Act
            testBook1.DeleteBook();

            List<Book> allBooks = Book.GetAll();
            List<Book> expected = new List<Book>{testBook};

            //Assert
            Assert.Equal(expected, allBooks);
        }

        [Fact]
        public void Test_UpdateTitle_UpdateBookTitleFromDatabase()
        {
            //Arrange
            Book testBook = new Book("The Giver", "1970-09-01");
            testBook.Save();

            Book testBook1 = new Book("Grapes of Wrath", "1970-09-01");
            testBook.Save();

            //Act
            testBook1.UpdateTitle("Grapes of Wrath");

            List<Book> allBooks = Book.GetAll();
            List<Book> expected = new List<Book>{testBook1};

            int testCount = testBook.GetCopyCount();

            //Assert
            Assert.Equal(testCount, 2);
        }

        public void Dispose()
        {
            Book.DeleteAll();
            Author.DeleteAll();
        }
    }
}
