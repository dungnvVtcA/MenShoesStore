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
        private string query;
        private MySqlConnection connection;
        public OrdersDAl()
        {
            connection = DBHelper.OpenConnection();
        }

        private MySqlDataReader reader;
        
        User u = new User();
        public bool CreateOrders(Orders orders)
        {
            //orders = new Orders();
            lock (this)
            {
                if( orders == null || orders.shoesList == null || orders.shoesList.Count == 0)
            {
                return false;
            }
            bool a = true;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
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
                    cmd.CommandText = "insert into OrderDetail(Or_ID,Shoes_id,Amount,Unitprice,OD_status) values("+orders.Order_id+", "+shoes.Shoes_id+","+shoes.Amount+","+shoes.Price*shoes.Amount+"," + orders.Order_status +");";
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
                connection.Close();
            }
            return a;
            }

        }
        public bool Update(int order_id)
        {
            if(connection!= null && connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            Orders order = new Orders();
            bool a = true;
            MySqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = "lock tables Users write , Orders write, Shoes write, OrderDetail write;";
            cmd.ExecuteNonQuery();
            MySqlTransaction trans = connection.BeginTransaction();
            cmd.Transaction = trans;
            try
            { 
                cmd.CommandText = "update Orders set Or_Status = 0 where  Or_ID = @Order_id;";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Order_id", order_id);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "update OrderDetail set OD_status = 0 where Or_ID = @Order_id;";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Order_id", order_id);
                cmd.ExecuteNonQuery();
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
                cmd.CommandText="unlock tables;";
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            return a;
            }
        private Orders GetOrder(MySqlDataReader reader)
        {
            Orders order = new Orders();
            order.Order_id = reader.GetInt32("Or_ID");
            order.user = new User();
            order.user.User_id = reader.GetInt32("User_id");
            order.Date_Order = reader.GetDateTime("Or_Date");
            order.Order_status = reader.GetInt32("Or_Status");
            return order; 

        }
        public List<Orders> GetOrders(MySqlCommand cmd)
        {
            List<Orders> list = new List<Orders>();
            reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                list.Add(GetOrder(reader));
            }
            reader.Close();
            connection.Close();
            return list;
            
        }
        public List<Orders> GetOrders(int orderFilter, Orders or)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            MySqlCommand cmd = new MySqlCommand(" ",connection);
            switch(orderFilter)
            {
                case  0 :
                query = @"Select * from Orders;";
                break;
                case 1 :
                query = @"select *from Orders where User_id = @user_id;";
                
                cmd.Parameters.AddWithValue("@user_id",or.user.User_id);
                break;
            }
            cmd.CommandText = query;
            
            return GetOrders(cmd);
        }
        public List<Orders> GetOrderDetailsByID(int id)
        {
            if(connection.State == System.Data.ConnectionState.Closed){
                connection.Open();
            }
            List<Orders> lisOr = new List<Orders>();
            string query = "Select Orders.Or_ID ,Orders.User_id,Orders.Or_Date,Shoes.Shoes_name,OrderDetail.Amount,OrderDetail.Unitprice,Orders.Or_Status from Orders inner join OrderDetail on Orders.Or_ID = OrderDetail.Or_ID inner join Shoes on Orderdetail.Shoes_id = Shoes.Shoes_id where OrderDetail.Or_ID="+id+";";
            Orders or = new Orders();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            reader = cmd.ExecuteReader();
                
            if(reader.Read())
            {
                or.Order_id = reader.GetInt32("Or_ID");
                or.user = new User();
                or.user.User_id = reader.GetInt32("User_id");
                or.Date_Order = reader.GetDateTime("Or_Date");
                or.Order_status = reader.GetInt32("Or_Status");
                Shoes s = new Shoes();
                or.shoesList = new List<Shoes>();
                s.Shoes_name = reader.GetString("Shoes_name");
                s.Amount = reader.GetInt32("Amount");
                s.Price = reader.GetDecimal("Unitprice");
                or.shoesList.Add(s);
                lisOr.Add(or);
            }
            reader.Close();
            
            connection.Close();
            return lisOr;
        }

    }
}