using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LibraryCatalog
{
    public class CheckoutsTest : IDisposable
    {
        public CheckoutsTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
        }


        [Fact]
        public void Test_CheckoutsEmptyAtFirst()
        {
            //Arrange, ACt
            int result = Checkout.GetAll().Count;

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_FindCheckoutsByPatronNameFindsCheckoutsInDatabaseById()
        {
            //Arrange
            Book testBook = new Book("The Giver", "1970-09-01");
            testBook.Save();
            Copy testCopy = new Copy(testBook.GetId());
            testCopy.Save();
            Patron testPatron = new Patron("Chad");
            testPatron.Save();

            Checkout testCheckout = new Checkout(testPatron.GetId(), testCopy.GetId(), "2017-03-02");
            testCheckout.Save();

            //Act
            List<Checkout> result = Checkout.FindCheckoutByPatronId(testPatron.GetId());
            List<Checkout> expected = new List<Checkout>{testCheckout};

            //Assert
            Assert.Equal(expected[0].GetPatronsId(), result[0].GetPatronsId());
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
