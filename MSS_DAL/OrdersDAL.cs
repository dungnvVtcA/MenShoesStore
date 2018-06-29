using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MSS_Persistence;
namespace MSS_DAL
{
    public class OrdersDAl
    {

        public bool CreateOrders(Orders orders)
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
            trans = cmd.Transaction;
            MySqlDataReader reader = null;
            if(orders.customer == null || orders.customer.Customer_name == null || orders.customer.Customer_name == "")
            {
                // mac dinh id khach hang  =1 
                orders.customer = new Customer(){Customer_id = 1};
            }
            try
            {
                if(orders.customer == null ) 
                {
                    throw new Exception("Can't find Customer");

                }
                //insert order
                cmd.CommandText = "insert into Orders(User_id,Or_Date,Or_Status) values (@Customer_id,@Date_Order,@Order_status);";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@User_id", orders.customer.Customer_id);
                cmd.Parameters.AddWithValue("@Date_Order", orders.Date_Order);
                cmd.Parameters.AddWithValue("@Order_status", orders.Order_status);
                cmd.ExecuteNonQuery();
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
                    cmd.CommandText=@"insert into OrderDetail(Or_ID,Shoes_id,Amount,Unitprice,OD_status) values
                    (" +orders.Order_id +","+shoes.Shoes_id+","+shoes.Price+","+shoes.Amount+","+orders.Order_status+");";
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
        public bool DeleteOrder(int id )
        {
            Orders order = new Orders();
            if( order == null )
            {
                return false;

            }
            bool a = true;
            MySqlConnection connection = DBHelper.OpenConnection();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = "lock tables Users write , Orders write, Shoes write, OrderDetail write;";
            cmd.ExecuteNonQuery();
            MySqlTransaction trans =null;
            trans = connection.BeginTransaction();
            trans = cmd.Transaction;
            MySqlDataReader reader = null;
            try{
                cmd.CommandText = "select Or_ID from OrderDetail where Or_ID="+id+";";
                reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                        throw new Exception("Not Exists  id Orders");
                }
                reader.Close();
                
                    cmd.CommandText ="Delete from OrderDetail where Or_ID="+id+";";
                    cmd.ExecuteNonQuery();
                    Console.Write("thanhcong");
                    cmd.CommandText = "Delete from Orders where Or_ID ="+id+";";
                    cmd.ExecuteNonQuery();
                    Console.Write("thanhcong2");
                    trans.Commit();
                    Console.Write("thanhcong3");
                    a = true;

            }
            catch(Exception e){
                Console.Write(e);
                a = false;
                try{
                    trans.Rollback();
                }
                catch{}
            }
            finally{
                cmd.CommandText="unlock tables;";
                cmd.ExecuteNonQuery();
                DBHelper.CloseConnection();
            }
            return a;
        }
        private Orders GetOrders(MySqlDataReader reader)
        {
            Orders order = new Orders();
            order.Order_id = reader.GetInt32("Or_ID");
            order.customer.Customer_id = reader.GetInt32("User_id");
            order.Date_Order = reader.GetDateTime("Or_Date");
            order.Order_status = reader.GetInt32("Or_Status");
            return order; 

        }
        public List<Orders> GetAllOrder()
        {
           string  query = @"select  *from Orders;";
           DBHelper.OpenConnection();
           MySqlDataReader reader = DBHelper.ExecQuery(query);
           List<Orders> or = new List<Orders>();
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