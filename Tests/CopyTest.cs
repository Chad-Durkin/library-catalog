using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LibraryCatalog
{
    public class CopiesTest : IDisposable
    {
        public CopiesTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
        }


        [Fact]
        public void Test_CopiesEmptyAtFirst()
        {
            //Arrange, ACt
            int result = Copy.GetAll().Count;

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_CopyGetAllReturnsAll()
        {
            //Arrange
            Book testBook = new Book("The Giver", "1970-09-01");
            testBook.Save();
            Copy testCopy = new Copy(testBook.GetId());
            testCopy.Save();
            Book testBook1 = new Book("The Giver", "1970-09-01");
            testBook1.Save();
            Copy testCopy1 = new Copy(testBook1.GetId());
            testCopy1.Save();
            Book testBook2 = new Book("The Giver", "1970-09-01");
            testBook2.Save();
            Copy testCopy2 = new Copy(testBook2.GetId());
            testCopy2.Save();


            //Act
            int result = Copy.GetAll().Count;

            //Assert
            Assert.Equal(3, result);
        }

        // [Fact]
        // public void Test_FindCopiesByBookNameFindsCopiesInDatabaseById()
        // {
        //     //Arrange
        //     Book testBook = new Book("The Giver", "1970-09-01");
        //     testBook.Save();
        //     Copy testCopy = new Copy(testBook.GetId());
        //     testCopy.Save();
        //     Book testBook1 = new Book("The Giver", "1970-09-01");
        //     testBook1.Save();
        //     Copy testCopy1 = new Copy(testBook1.GetId());
        //     testCopy1.Save();
        //     Book testBook2 = new Book("The Giver", "1970-09-01");
        //     testBook2.Save();
        //     Copy testCopy2 = new Copy(testBook2.GetId());
        //     testCopy2.Save();
        //
        //
        //     //Act
        //     Copy result = Copy.GetAll().Count;
        //
        //     //Assert
        //     Assert.Equal(3, result);
        // }

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
