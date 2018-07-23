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
        private MySqlConnection connection;
        public UserDAL()
        {
            connection = DBHelper.OpenConnection();

        }
        public User Login(string id , string pass)
        {
            string query = "Select User_id, User_Type from Users where AccountName='"+ id +"' and User_Password ='"+pass+"';";
            if( connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            MySqlCommand cmd = new MySqlCommand(query,connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            
            User u = null;
            if(reader.Read())
            {  
                u = new User();
                u.User_id = reader.GetInt32("User_id");
                u.Type  =  reader.GetInt32("User_Type");  
               
            }
            reader.Close();
            connection.Close();
            return u;
        }
        public User GetUserByid(int id_user)
        {
            string query = "Select *from Users where User_id ="+id_user+";";
            if( connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            MySqlCommand cmd = new MySqlCommand(query,connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            
            User u = null;
            if(reader.Read())
            {  
                u = new User();
                u.Phone = reader.GetString("Phone");
                u.Address = reader.GetString("User_Address");
               
            }
            reader.Close();
            connection.Close();
            return u;
        }
       
    }
    
}
