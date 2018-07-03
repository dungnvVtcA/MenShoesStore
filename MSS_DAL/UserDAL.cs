using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MSS_Persistence;
using System.Threading.Tasks;

namespace MSS_DAL
{
    public class UserDAL
    {
        public User Login(string id , string pass)
        {
            lock (this)
            {
                string query = "Select User_id, User_Password ,User_Type from Users where AccountName='"+ id +"';";
            DBHelper.OpenConnection();
            MySqlDataReader reader = DBHelper.ExecQuery(query);
                
            User u = null;
            if(reader.Read())
            {  
                u = new User();
                u.User_id = reader.GetInt32("User_id");
                u.Type  =  reader.GetInt32("User_Type");
                u.Password = reader.GetString("User_Password");
                if(pass == u.Password)
                {
                    DBHelper.CloseConnection();
                    return u;
                }
                else if( pass != u.Password)
                {
                    DBHelper.CloseConnection();
                    return null;
                }
            }
            reader.Close();
            DBHelper.CloseConnection();
            return u;
            }
        }
       
    }
    
}
