using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MSS_Persistence;

namespace MSS_DAL
{
    public class UserDAL
    {
        public int[] Login(string id , string pass)
        {
            
            string query = "Select User_id, User_Password ,User_Type from Users where AccountName='"+id+"';";
            DBHelper.OpenConnection();
            MySqlDataReader reader = DBHelper.ExecQuery(query);
            if(reader.Read())
            {   
                int[] user = new int[2];
                string true_p = reader.GetString("User_Password");
                user[1] =  reader.GetInt32("User_Type");
                user[0] = reader.GetInt32("User_id");
                if(pass == true_p  )
               {
                   DBHelper.CloseConnection();
                  return user;  
                   
               }
            }
            reader.Close();
            DBHelper.CloseConnection();
             return null;
        }
       
    }
    
}
