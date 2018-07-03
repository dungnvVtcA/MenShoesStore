using System;
using MSS_Bl;
using MSS_DAL;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MSS_Persistence;
using System.Text;
using System.Security;

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

        // UserBl ubl = new  UserBl(); // cn đang nhập.
        // User u = new User();
        // u.AccountName = "Nguyenvandung";
        // u.Password = "dung12345678";
        
        // var result = ubl.login(u.AccountName, u.Password);
        // if(result == null)
        // {
        //     Console.WriteLine("Dang nhap that bai");
        // }
        // else 
        // {
        //     Console.WriteLine("dang nhap thanh cong");
        //     if( result.Type == 1)
        //     {
        //         Console.Write("chao mung ad");
        //     }else if(result.Type ==0)
        //     {
        //         Console.Write("chao mung khách hàng ");
        //     }
            
        // }
      
        // create 

        // OrderBL obl = new OrderBL();
        // Orders order = new Orders();
        // ShoesBL sbl = new ShoesBL();
        // Shoes sh = new Shoes();
        // order.Order_status = 1;
        // order.user.User_id = result.User_id ;
        // order.shoesList.Add(sbl.GetShoesById(3));
        // order.shoesList[0].Amount =1;


        // Console.WriteLine("Create Order: " + (obl.CreateOrder(order) ? "completed!" : "not complete!"));
        
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

        // ShoesBL sbl = new ShoesBL();
        // Shoes s = new Shoes();
        // List<Shoes> lish = new List<Shoes>();
        // lish = sbl.GetAllShoes();
        // Console.Write(lish.Count);
        // //
        // OrderBL or = new OrderBL();
        // List<Orders> lis = new List<Orders>();
        // lis = or.GetAllOrder();
        // Console.WriteLine(lis.Count);

        MENU menu = new MENU();
        menu.MainMENU();

    }
    class valueexception: Exception
        {
            public valueexception(string massage): base(massage)
            {
            }
        }
    public class MENU
    {
        User user = null;
        UserBl ubl =new UserBl();
        private static int id ;

        private static int i;
        
        private static short Menu(string title, string[] menu)
        {
            short choose = 0;
            string line = "========================================";
            Console.WriteLine(line);
            Console.WriteLine(" " + title);
            Console.WriteLine(line);
            for (int i = 0; i < menu.Length; i++)
            {
                Console.WriteLine(" " + (i + 1) + ". " + menu[i]);
            }
            Console.WriteLine(line);
            do
            {
                Console.Write("Your choice: ");
                try
                {
                    choose = Int16.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Your Choose is wrong!");
                    continue;
                }
            } while (choose <= 0 || choose > menu.Length);
            return choose;
        }
        public void MainMENU()
        {
            
            while(true)
            {
                Console.Clear();
                user = new User();
                var result = ubl.login("Nguyenvandung","dung12345678");
                id = result.User_id;

                if( result != null)
                {
                    Console.Write("Log In successful !");
                    if(result.Type == 0)
                    {
                        MenuCustomer();
                        break;

                    }else if( result.Type ==1)
                    {
                        
                        MenuStaff();
                        break;
                    }
                }
                else 
                {
                    Console.Write("Username or password is incorrect, please enter again!");
                    Console.ReadLine();
                }

            }
        }
        public static void MenuStaff()
        {
            OrderBL obl = new OrderBL();
            var result = obl.GetAllOrder();
            while(true)
            {
                Console.Clear();
                int b = result.Count;
                Console.WriteLine("1.Browse orders({0})",b);
                Console.WriteLine("2.The list of approved orders");
                Console.WriteLine("3.Reload page");
                Console.WriteLine("4.Exits");
                // string[] mainMenu = { "Browse orders", "The list of approved orders", "Reload page" };
                // while( mainChoose!= mainMenu.Length)
                // {
                //     Console.Clear();
                //     mainChoose = Menu("Men Shoes Store ", mainMenu);
                int choice = Convert.ToInt32(Console.ReadLine());
                switch(choice)
                {
                    case 1 : MENU.BrowseOrders();
                    break;
                    case 2 :MENU.ListApprovedorders();
                    break;
                    case 3 :MENU.Reload();
                    break;
                    case 4 :
                    Environment.Exit(4);
                    break;
                    default :
                    break;
                }
                
            }
        }
        public static void BrowseOrders(){}

        public static void ListApprovedorders(){}

        public static void Reload(){}
        public static  void MenuCustomer()
        {
            MENU menu = new MENU();
            
            while(true)
            {
                Console.Clear();
                short mainChoose = 0;
                string[] mainMenu = { "Display list shoes", "Crete Orders ", "Display list orders", "Exit" };
                while( mainChoose!= mainMenu.Length)
                {
                    Console.Clear();
                    mainChoose = Menu("Men Shoes Store ", mainMenu);
                    switch(mainChoose)
                    {
                        case 1 :menu.Displaylistshoes();
                        break;
                        case 2 : menu.CreateOrders();
                        break;
                        case 3 : menu.DisplayListOrders();
                        break;
                        case 4 :
                        Environment.Exit(4);
                        break;
                        default :
                        break;
                    }

                }
            }
            

        } 
        public static string GetConsolePassword( )
        {
            StringBuilder sb = new StringBuilder( );
            while ( true )
            {
                ConsoleKeyInfo cki = Console.ReadKey( true );
                if ( cki.Key == ConsoleKey.Enter )
                {
                    Console.WriteLine( );
                    break;
                }

                if ( cki.Key == ConsoleKey.Backspace )
                {
                    if ( sb.Length > 0 )
                    {
                        Console.Write( "\b\0\b" );
                        sb.Length--;
                    }

                    continue;
                }

                Console.Write( '*' );
                sb.Append( cki.KeyChar );
            }
            return sb.ToString( );
        }
        public  void Displaylistshoes()
        {
            Console.Clear();
            ShoesBL sbl = new ShoesBL();
            var lists = sbl.GetAllShoes();
            string line =("====================================================================================================================================================================================\n");
            Console.WriteLine(line);
            Console.Write(" Shoes_Id  |  Trademark_Id |  Shoes_Name       |  Material        |    Price    |  Size |             Manufacture                 |      Style            |   Color        | Amount |\n");
            Console.WriteLine(line);
            foreach (var Shoes in lists)
            {
                Console.WriteLine("{0,-13}{1,-17}{2,-20}{3,-20}{4,-14}{5,-9}{6,-40}{7,-25}{8,-17}{9} ",Shoes.Shoes_id,Shoes.TM.Trademark_id,Shoes.Shoes_name,Shoes.Material,Shoes.Price,Shoes.Size,Shoes.Manufacture,Shoes.Style,Shoes.Color,Shoes.Amount);
            }
            Console.WriteLine(line);
            Console.Write("\n    Press Enter key to back menu... !");
            Console.ReadLine();

        }
        
        public  void CreateOrders()
        {
            Console.Clear();
            OrderBL obl = new OrderBL();
            Orders order = new Orders();
            ShoesBL sbl = new ShoesBL();
            Shoes sh = new Shoes();
            order.Order_status = 1;
            order.user.User_id =  id;
            var result = sbl.GetAllShoes();
            if( result != null)
            {
                    Console.Write("- iD: ");
                    while (true)
                    {
                        try
                        {
                            int sh_id = Convert.ToInt32(Console.ReadLine());
                            for( i = 0 ; i < result.Count ; i++)
                            {
                                if( sh_id != result[i].Shoes_id )
                                {
                                    throw (new valueexception("Mã khong ton tai, mời bạn nhập lại: "));
                                }
                                else if( sh_id == result[i].Shoes_id)
                                {
                                    order.shoesList.Add(sbl.GetShoesById(sh_id));
                                    break;
                                }
                            }
                        }
                        catch(valueexception e)
                        {
                            Console.Write(e.Message);
                            continue;
                        }
                        catch
                        {
                            Console.Write("Vui long nhap ID hop le: ");
                            continue;
                        }
                        break;
                    }
                    Console.Write("Amount: ");
                    while (true)
                    {
                        try
                        {
                            int amount = Convert.ToInt32(Console.ReadLine());
                            for( i = 0 ; i < result.Count;i++)
                            {
                                if( amount > result[i].Amount )
                                {
                                    Console.WriteLine("So luong con : {0}",result[i].Amount);
                                    throw (new valueexception("So luong chi con :  , mời bạn nhập lại: "));
                                }else if( 0 < amount || amount <= result[i].Amount)
                                {
                                    order.shoesList[0].Amount = amount;
                                    break;
                                }
                            }
                        }
                        catch(valueexception e)
                        {
                            Console.Write(e.Message);
                            continue;
                        }
                        catch
                        {
                            Console.Write("vui long nhap so luong hop le :");
                            continue;
                        }
                        break;
                    }
                    Console.WriteLine("Create Order: " + (obl.CreateOrder(order) ? "completed!" : "not complete!"));
                    Console.Write("Nhan enter !");
                    Console.ReadLine();
            }
            else if( result == null)
            {
                Console.Write("Shoes not exists!");
                Console.ReadLine();
                MenuCustomer();
            }
            
        }
        public  void  DisplayListOrders()
        {
            Console.Clear();
            OrderBL obl = new OrderBL();
            var list = obl.GetAllOrder();
            string line = "============================================================================================";
            Console.WriteLine(line);
            Console.WriteLine(" Order_id        |   User_ID        |  Date                            |   Order Status  |");
            Console.WriteLine(line);
            foreach (var orders in list)
            {
                Console.WriteLine("{0,-20} {1,-20} {2,-29} {3}",orders.Order_id,orders.user.User_id,orders.Date_Order,orders.Order_status);
                
            }
            Console.WriteLine(line);
            Console.Write("Nhan enter !");
            Console.ReadLine();
            
        }
    }
}
