using System;
using Xunit;
using MySql.Data.MySqlClient;
using MSS_DAL;

namespace DAL.test
{
    public class DBHelperTest
    {
        [Fact]
        public void GetConnectionTest()
        {
            Assert.NotNull(DBHelper.GetConnection());
        }
        
        [Fact]
        public void OpenConnectionTest()
        {      
            Assert.NotNull(DBHelper.OpenConnection());
        }
        
    }
}
