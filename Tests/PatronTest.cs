using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LibraryCatalog
{
    public class PatronsTest : IDisposable
    {
        public PatronsTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
        }


        [Fact]
        public void Test_PatronsEmptyAtFirst()
        {
            //Arrange, ACt
            int result = Patron.GetAll().Count;

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_FindPatronByNameFindsPatronInDatabaseById()
        {
            //Arrange
            Patron testPatron = new Patron("Chad");
            testPatron.Save();

            //Act
            Patron result = Patron.FindPatronByName(testPatron.GetName());

            //Assert
            Assert.Equal(testPatron, result);
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
