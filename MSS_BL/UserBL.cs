using System;
using System.Collections.Generic;
using MSS_Persistence;
using MSS_DAL;

namespace MSS_Bl
{
    public class UserBl
    {
        public int[] login(string id , string pass)
        {
            UserDAL u = new UserDAL();

            return u.Login(id,pass);
        }
    
    }
}
