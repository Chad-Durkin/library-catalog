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

        public void Dispose()
        {
            Book.DeleteAll();
            Author.DeleteAll();
        }
    }
}
