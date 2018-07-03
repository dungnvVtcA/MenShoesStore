using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MSS_Persistence;
using System.Threading.Tasks;

namespace MSS_DAL
{
    public class OrdersDAl
    {
        User u = new User();
        public bool CreateOrders(Orders orders)
        {
            lock (this)
            {
                if( orders == null || orders.shoesList == null || orders.shoesList.Count == 0)
            {
                return false;
            }
            bool a = true;
            MySqlConnection connection = DBHelper.OpenConnection();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;
            // khoa cac bang k cho phep nguoi dung sua?
            cmd.CommandText = "lock tables Users write , Orders write, Shoes write, OrderDetail write;";
            cmd.ExecuteNonQuery();
            MySqlTransaction trans = connection.BeginTransaction();
            cmd.Transaction = trans;
            MySqlDataReader reader = null;
            try
            { 
                //insert order
                cmd.CommandText = "insert into Orders(User_id,Or_Status) values (@User_id,@Order_status);";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@User_id", orders.user.User_id);
                cmd.Parameters.AddWithValue("@Order_status", orders.Order_status);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "select LAST_INSERT_ID() as order_id";
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    orders.Order_id = reader.GetInt32("Order_id");
                }
                reader.Close();


                //insert orderdetail table 
                foreach (var shoes in orders.shoesList)
                {
                    if( shoes.Amount <=0)
                    {
                        throw new Exception("NOT exists Shoes");

                    }
                    cmd.CommandText = "select Price from Shoes where Shoes_id =@Shoes_id";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Shoes_id", shoes.Shoes_id);
                    reader = cmd.ExecuteReader();
                    if(!reader.Read())
                    {
                        throw new Exception("not exists shoes");

                    }
                    shoes.Price = reader.GetDecimal("Price");
                    reader.Close();

                    //insert to orderdetail
                    cmd.CommandText = "insert into OrderDetail(Or_ID,Shoes_id,Amount,Unitprice,OD_status) values("+orders.Order_id+", "+shoes.Shoes_id+","+shoes.Amount+","+shoes.Price+"," + orders.Order_status +");";
                    cmd.ExecuteNonQuery();
                    // update so luong
                    cmd.CommandText = "update Shoes set Amount=Amount-@quantity where Shoes_id=" + shoes.Shoes_id + ";";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@quantity", shoes.Amount);
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
                a = true;


            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                a = false;
                try{
                    trans.Rollback();
                }
                catch{}
            }
            finally
            {
                // mo lai tat ca cac bang
                cmd.CommandText="unlock tables;";
                cmd.ExecuteNonQuery();
                DBHelper.CloseConnection();
            }
            return a;
            }

        }
        private Orders GetOrders(MySqlDataReader reader)
        {
            Orders order = new Orders();
            order.Order_id = reader.GetInt32("Or_ID");
            order.user.User_id = reader.GetInt32("User_id");
            order.Date_Order = reader.GetDateTime("Or_Date");
            order.Order_status = reader.GetInt32("Or_Status");
            return order; 

        }
        public List<Orders> GetAllOrder()
        {
            lock (this)
            {
                string  query = @"Select * from Orders;";
            List<Orders> or = new List<Orders>();
            DBHelper.OpenConnection();
            MySqlDataReader reader = DBHelper.ExecQuery(query);
            while(reader.Read())
            {
                or.Add(GetOrders(reader));
           
            }
            reader.Close();
            DBHelper.CloseConnection();
            return or;
            }

        }

    }
}