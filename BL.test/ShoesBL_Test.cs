using System;
using Xunit;
using MSS_Bl;
using MySql.Data.MySqlClient;
using MSS_Persistence;
using System.Collections.Generic;


namespace BL.test
{
    public class ShoesBL_Test
    {
        ShoesBL sbl = new ShoesBL();
        
        
        [Fact]
        public void GetAllShoes_test()
        {
            List<Shoes> listShoes = sbl.GetAllShoes();
            Assert.NotNull(listShoes);
        }
        [Fact]
        public void GetShoesById_Test()
        {
            Shoes sh = sbl.GetShoesById(1);
            Assert.NotNull(sh);                                                                             
        }
        [Fact]
        public void GetShoesById_TestNULL()
        {
            Shoes sh = sbl.GetShoesById(100);
            Assert.Null(sh);                                                                             
        }

    }
}