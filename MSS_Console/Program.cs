using System;
using MSS_Bl;
using MSS_DAL;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MSS_Persistence;
class Program {

    static void Main(String[] arg){
        // MySqlConnection connection;
        // connection = new MySqlConnection
        //         {
        //             ConnectionString = "server=localhost;user id=root;password=12345678;port=3306;database=MenShoes;SslMode=None"
        //         };
        // connection.Open();
        // String usrname,pwd;
        // Console.Write(" :");
        // usrname = Console.ReadLine();
        // pwd = Console.ReadLine();
        // string query = "Select User_Password from Users where AccountName='"+usrname+"';";
        
        // MySqlCommand command = new MySqlCommand(query, connection);
        // MySqlDataReader read = command.ExecuteReader();

        // if(read.Read()){
        //   String true_pwd = read.GetString("User_Password");
        //   if(pwd == true_pwd)
        //   {
        //       Console.Write(" ddanwg nhap thanh cong");
        //   }
        //   else
        //   {
        //       Console.WriteLine(" sai me roi");
        //   }
        // }
        // else
        // {
        //     Console.Write("User khong ton tai!");
        // }
        // read.Close();
        // connection.Close();

        // UserBl u = new  UserBl(); // cn đang nhập.

        // var result = u.login("Nguyenvana","A12345678");
        // if(result == null)
        // {
        //     Console.WriteLine("Dang nhap that bai");
        // }
        // else 
        // {
        //     if(result[1] == 1)
        //     {
        //         Console.WriteLine("Hello Staff");
        //     }else if( result[1] == 0)
        //     {
        //         Console.WriteLine("xin chao ban khach hang ");
        //     }
        // }
        // DBHelper.CloseConnection();

        // get shoes all
        // MySqlConnection connection;
        // connection = new MySqlConnection
        //         {
        //             ConnectionString = "server=localhost;user id=root;password=12345678;port=3306;database=MenShoes;SslMode=None"
        //         };
        // connection.Open();
        // int Shoes_id;
        // Shoes_id = Convert.ToInt32(Console.ReadLine());
        // string  query = "select Shoes_id ,Shoes_name,TM_id,Color,Material,Price,Size,Manufacturers,Amount from Shoes where Shoes_id="+Shoes_id+";";
        // MySqlCommand command = new MySqlCommand(query, connection);
        // MySqlDataReader read = command.ExecuteReader();
        // Shoes s = null;
        // if(read.Read())
        // {
        //     s.Shoes_name = read.GetString("Shoes_name");
        //     Console.Write(s.Shoes_name);
        //     s.Amount = read.GetInt32("Amount");
        //     Console.Write(s.Amount);
        // }
        ShoesBL sbl = new ShoesBL();
        Shoes s = new Shoes();
        List<Shoes> lish = new List<Shoes>();
        s = sbl.GetShoesById(2);
        Console.Write(s.Shoes_name);
        //
        OrderBL or = new OrderBL();
        List<Orders> lis = new List<Orders>();
        lis = or.GetAllOrder();
        Console.WriteLine(lis.Count);
        //delete
        int a = Convert.ToInt32(Console.ReadLine());
        or.Delete(a);
        // MySqlConnection connection = DBHelper.OpenConnection();
        // MySqlCommand cmd = connection.CreateCommand();
        // cmd.Connection = connection;
        // MySqlDataReader reader = null;
        // cmd.CommandText = "select Or_ID from OrderDetail where Or_ID="+a+";";
        // reader = cmd.ExecuteReader();
        // if (!reader.Read())
        //     {
        //         throw new Exception("Not Exists  id Orders");
        //     }
        //     reader.Close();
                
        //     cmd.CommandText ="Delete from OrderDetail where Or_ID="+a+";";
        //     cmd.ExecuteNonQuery();
        //     Console.Write("thanhcong");
        //     cmd.CommandText = "Delete from Orders where Or_ID ="+a+";";
        //     cmd.ExecuteNonQuery();
        //     Console.Write("thanhcong2");
        // if(lis.Count ==2)
        // {
        //     Console.Write("deletethanh cong");
        // }

           

        
        
     
    }
}
