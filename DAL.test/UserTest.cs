using System;
using Xunit;
using MSS_Persistence;
using MSS_DAL;
using MySql.Data.MySqlClient;

namespace  DAL.test
{
    public class  UserTest
    {
        
        [Fact]

        public void Check_IdPass_true()
        {
            UserDAL u = new UserDAL();
            var result = u.Login("Nguyenvana","A12345678");
            Assert.NotNull(result);
        }        
        
    }
}
