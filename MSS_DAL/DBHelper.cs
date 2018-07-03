using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace MSS_DAL
{
    
    public class DBHelper
    {
        private static MySqlConnection connection;
        public static MySqlConnection GetConnection()
        {
            connection = new MySqlConnection
                {
                    ConnectionString = "server=localhost;user id=root;pwd=12345678;port=3306;database=MenShoes;sslmode=none;"
                };
            return connection;
        }
        public static MySqlDataReader ExecQuery(string query)
        {
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            
            return reader;
        }
        
        public static MySqlConnection OpenConnection()
        {
            if (connection == null)
            {
                GetConnection();
            }
            connection.Open();
           
            return connection;
        }
        public static void CloseConnection()
        {
            if (connection != null) 
            {
                connection.Close();
            }
        }
        
        public static bool ExecTransaction(List<string> queries)
        {
            bool result = true;
            OpenConnection();
            MySqlCommand command = connection.CreateCommand();
            MySqlTransaction trans = connection.BeginTransaction();

            command.Connection = connection;
            command.Transaction = trans;

            try
            {
                foreach (var query in queries)
                {
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                    trans.Commit();
                }
                result = true;
            }
            catch
            {
                result = false;
                try
                {
                    trans.Rollback();
                }
                catch
                {
                }
            }
            finally
            {
                CloseConnection();
            }
            return result;
        }
    }
}