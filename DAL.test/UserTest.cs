using System;
using Xunit;
using MSS_Persistence;
using MSS_DAL;
using MySql.Data.MySqlClient;

namespace DAL.test
{
    public class UserTest
    {
        
        [Fact]

        public void LogIN_test()
        {
            UserDAL u = new UserDAL();
            User user = u.Login("Nguyenvana","A12345678");
            
            Assert.NotNull(user);
        }     
   
        
    }
}
