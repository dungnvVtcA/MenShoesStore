using System;
using Xunit;
using MySql.Data.MySqlClient;
using MSS_DAL;

namespace DAL.test
{
    public class DBHelperUnitTest
    {
        [Fact]
        public void GetConnectionTest()
        {
            Assert.NotNull(DBHelper.GetConnection());
        }
        
        [Fact]
        public void OpenConnectionTest()
        {
            MySqlConnection con =  DBHelper.OpenConnection();      
            Assert.NotNull(con);
        }
    }
}
