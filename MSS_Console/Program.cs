using System;
using MSS_Bl;
using MSS_DAL;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MSS_Persistence;
using System.Text;
using System.Security;
using System.Text.RegularExpressions;

namespace MSS_Console
{
    class Program {

    static void Main(String[] arg)
    {
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
        User result = new User();
        UserBl ubl =new UserBl();
        private static int id ;
        private static bool validate(string str)
        {
                Regex regex = new Regex("[a-zA-Z0-9_]");
                MatchCollection matchCollectionstr = regex.Matches(str);
                if (matchCollectionstr.Count < str.Length)
                {
                    return false;
                }
                return true;
        }
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
            }while (choose <= 0 || choose > menu.Length);
            return choose;
        }
        public  void MainMENU()
        {
            while(true)
            {
                Console.Clear();
                string line = "===================================================\n";
                Console.Write(line);
                Console.WriteLine("        MEN SHOES STORE   ");
                Console.WriteLine("-------------------------------------------------");
                Console.WriteLine("1 . Log in.");
                Console.WriteLine("2 . Exit");
                Console.Write(line);
                Console.Write("#You Choose :");
                var choose = Console.ReadLine();
                if(choose == " ")
                {
                    Console.ReadLine();
                }
                switch (choose)
                {
                    case "1" : 
                    while(true)
                    {
                        Console.Clear();
                        Console.Write("---------      LOG IN      ---------\n");
                        Console.Write("-Username :");
                        string name = Console.ReadLine();
                        Console.Write("-Password : ");
                        string pass = GetConsolePassword();
                        while ((validate(name) == false) || (validate(pass) == false))
                        {
                            Console.Clear();
                            Console.WriteLine(".  .  .Username or password wrong, please re-enter ! ");
                            Console.Write("-Username: ");
                            name = Console.ReadLine();
                            Console.Write("-Password: ");
                            pass = GetConsolePassword();
                        }
                        result = ubl.login(name,pass);
                        if( result != null   )
                        {
                            if(result.Type == 0)
                            {
                                id = result.User_id;
                                MenuCustomer();
                                break;

                            }else if( result.Type ==1)
                            {
                                
                                MenuStaff();
                                break;
                            }
                        }

                    }
                    break;
                    case "2" :
                    Environment.Exit(2);
                    break;
                    default:
                    break;
                }
            }
        }
        public static void MenuStaff()
        {
            OrderBL obl = new OrderBL();
            int b = 0;
            var result = obl.GetAllOrder();
            foreach (var or in result)
            {
                if(or.Order_status == 1)
                {
                    b++;
                }
            }
            while(true)
            {
                Console.Clear();
                string line = "===================================\n";
                Console.Write(line);
                Console.Write("Men Shoes\n");
                Console.Write(line);
                Console.WriteLine("1.Browse orders({0})",b);
                Console.WriteLine("2.The list of approved orders");
                Console.WriteLine("3.Reload page");
                Console.WriteLine("4.Exits");
                Console.Write(line);
                Console.Write("You Choose :");
                var choice = Console.ReadLine();
                if(choice == " ")
                {
                    Console.ReadLine();
                }
                switch(choice)
                {
                    case "1" : MENU.BrowseOrders();
                    break;
                    case "2" :MENU.ListApprovedorders();
                    break;
                    case "3" :MENU.Reload();
                    break;
                    case "4" :
                    Environment.Exit(4);
                    break;
                    default :
                    break;
                }
                
                
            }
        }
        public static void BrowseOrders()
        {
            Console.Clear();
            int count = 1;
            OrderBL obl = new OrderBL();
            var list = obl.GetAllOrder();
            foreach (var or in list)
            {
                if (or.Order_status == 1)
                {
                    string line = "============================================================================================";
                    Console.WriteLine(line);
                    Console.WriteLine(" Order_id        |   User_ID        |  Date                            |   Order Status  |");
                    Console.WriteLine(line);
                    foreach (var orders in list)
                    {
                        if( orders.Order_status == 1)
                        {
                            Console.WriteLine("{0,-20} {1,-20} {2,-29} {3}",orders.Order_id,orders.user.User_id,orders.Date_Order,orders.Order_status);
                        }
                        
                    }
                    Console.WriteLine(line);
                    while(true){
                        Console.Write("Enter the ID to view the customer's order details: ");
                        while (true)
                        {
                            try
                            {
                                int or_id = Convert.ToInt32(Console.ReadLine());
                                var orderdetail = obl.GetOrderDetailsByID(or_id);
                                if(orderdetail == null)
                                {
                                    throw new valueexception("Not find Order_ID,please re-enter : ");
                                }
                                else if(orderdetail != null)
                                {
                                    Console.Clear();
                                    Console.WriteLine("          Order Detail of Customer Information        ");
                                    Console.WriteLine("-ID                : {0}",orderdetail.Order_id);
                                    Console.WriteLine("-Order date        : {0}",orderdetail.Date_Order);
                                    Console.WriteLine("-Customer name     : {0}",orderdetail.user.User_name );
                                    Console.WriteLine("-Customer phone    : {0}",orderdetail.user.Phone);
                                    Console.WriteLine("-Customer Address  : {0}",orderdetail.user.Address);
                                    Console.WriteLine("-Customer Email    : {0}",orderdetail.user.Email);
                                    foreach (var order in orderdetail.shoesList)
                                    {
                                        Console.WriteLine("-Product name {0}    : {1}",count,order.Shoes_name);
                                        count++;
                                        Console.WriteLine("-Amonut            : {0}",order.Amount);
                                        Console.WriteLine("-Size              : {0}",order.Size);
                                        Console.WriteLine("-Unit price        : {0}vndong",order.Price);
                                    }
                                    Console.WriteLine("Do you want to Browse this Order?(y/n)");
                                    string choice3 = Console.ReadLine();
                                    if( choice3 =="y")
                                    {
                                        obl.update(or_id);
                                        Console.WriteLine("...Browse  Order successful!");
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
                                Console.Write("Please enter a valid value :");
                                continue;
                            }
                            break;
                        }
                        Console.Write("You want to browse order?(y/n)");
                        string choice = Console.ReadLine();
                        if( choice == "n")
                        {
                            break;
                        }
                    
                    }
                }
            }

        }

        public static void ListApprovedorders()
        
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
                if( orders.Order_status == 0)
                {
                    Console.WriteLine("{0,-20} {1,-20} {2,-29} {3}",orders.Order_id,orders.user.User_id,orders.Date_Order,orders.Order_status);
                }
                        
            }
            Console.WriteLine(line);
            Console.Write("\n    Press Enter key to back menu... !");
            Console.ReadLine();
            
        }

        public static void Reload()
        {
            MenuStaff();
        }
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
            string line =("===================================================================================================================\n");
            Console.WriteLine(line);
            Console.Write(" Shoes_Id  |  Shoes_Name           |    Price         |  Size  |      Style             |   Color        | Amount |\n");
            Console.WriteLine(line);
            foreach (var Shoes in lists)
            {
                Console.WriteLine("{0,-15}{1,-25}{2,-17}{3,-10}{4,-24}{5,-18}{6} ",Shoes.Shoes_id,Shoes.Shoes_name,Shoes.Price,Shoes.Size,Shoes.Style,Shoes.Color,Shoes.Amount);
            }
            Console.WriteLine(line);
            Console.Write("Do you want to see product details?(y/n)");
            string choice = Console.ReadLine();
            if(choice == "y" )
            {
                Console.Write("Input  Shoes_ID: ");
                        while (true)
                        {
                            try
                            {
                                    int sh_id = Convert.ToInt32(Console.ReadLine());
                                    var shoes = sbl.GetShoesById(sh_id);
                                    if(shoes == null)
                                    {
                                        throw new valueexception("Not find Shoes_ID,please re-enter : ");
                                    }
                                    else if(shoes != null)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("          Product Information        ");
                                        Console.WriteLine("-ID                : {0}",shoes.Shoes_id);
                                        Console.WriteLine("-Name              : {0}",shoes.Shoes_name);
                                        Console.WriteLine("-Size              : {0}",shoes.Size);
                                        Console.WriteLine("-Amount            : {0}",shoes.Amount);
                                        Console.WriteLine("-Price             : {0}dong",shoes.Price);
                                        Console.WriteLine("-Color             : {0}",shoes.Color);
                                        Console.WriteLine("-Material          : {0}",shoes.Material);
                                        Console.WriteLine("-Manufacture       : {0}",shoes.Manufacture);
                                        Console.WriteLine("-Trademark name    : {0}",shoes.TM.Name);
                                        Console.WriteLine("-Trademark Origin  : {0}",shoes.TM.Origin);
                                    }
                            }
                            catch(valueexception e)
                            {
                                Console.Write(e.Message);
                                continue;
                            }
                            catch
                            {
                                Console.Write("Please enter a valid value :");
                                continue;
                            }
                            break;
                        }
            }
            Console.Write("\n    Press Enter key to back menu... !");
            Console.ReadLine();
        }
        
        public  void CreateOrders()
        {
            Console.Clear();
            OrderBL obl = new OrderBL();
            Orders order = new Orders();
            ShoesBL sbl = new ShoesBL();
            order.shoesList = new List<Shoes>();
            int count1 = 0;
            Shoes sh = new Shoes();
            order.Order_status = 1;
            order.user = new User();
            order.user.User_id =  id;
            var result = sbl.GetAllShoes();
            int count = 0;
            int index = 0;
            if( result != null)
            {
                while(true)
                {
                    while(true)
                    { 
                        Console.WriteLine("Nhap Shoes_id : ");
                        try{
                            
                            int Sh_id = Convert.ToInt32(Console.ReadLine());
                            for (int i = 0; i < result.Count; i++)
                            {
                                if(result[i].Shoes_id == Sh_id)
                                {
                                    index = i;
                                    order.shoesList.Add(sbl.GetShoesById(Sh_id));
                                    count++;
                                }
                            }
                            if (count ==0)
                            {   
                                throw new valueexception("khong tim thay");
                            }
                        }catch(valueexception e)
                        {
                            Console.WriteLine(e.Message);
                            continue;
                        }
                        catch{
                            continue;
                        }
                        break;
                    }
                    
                        while (true)
                        {
                            try
                            {
                                    Console.Write("Input  Amount: ");
                                    int amount = Convert.ToInt32(Console.ReadLine());

                                    if( amount > result[index].Amount )
                                    {
                                        Console.WriteLine("So luong con : {0}",result[index].Amount);
                                        throw (new valueexception("So luong khong du , mời bạn nhập lại: "));
                                    }else if( 0 < amount || amount <= result[index].Amount)
                                    {
                                        order.shoesList[count1].Amount = amount;
                                        count1++;
                                        break;
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
                        Console.Write("Ban co muon dat them san pham ?(y/n) ");
                        char choice = Convert.ToChar(Console.ReadLine());
                        if(choice == 'n')
                        {
                            break;
                        }
                
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
            int count = 1;
            OrderBL obl = new OrderBL();
            var list = obl.GetAllOrderByIDUser(id);
            string line = "============================================================================================";
            Console.WriteLine(line);
            Console.WriteLine(" Order_id        |  Date                            |   Order Status  |");
            Console.WriteLine(line);
            foreach (var orders in list)
            {
                Console.WriteLine("{0,-20}  {1,-29} {2}",orders.Order_id,orders.Date_Order,orders.Order_status);
                
            }
            Console.WriteLine(line);
            Console.Write("Do you want to see Order details?(y/n)");
            string choice = Console.ReadLine();
            if(choice == "y" )
            {
                Console.Write("Input  Order_ID: ");
                while (true)
                {
                    try
                    {
                        int or_id = Convert.ToInt32(Console.ReadLine());
                        var orderdetail = obl.GetOrderDetailsByID(or_id);
                        if(orderdetail == null)
                        {
                            throw new valueexception("Not find Order_ID,please re-enter : ");
                        }
                        else if(orderdetail != null)
                        {
                            Console.Clear();
                            Console.WriteLine("          Order Detail Information        ");
                            Console.WriteLine("-ID                : {0}",orderdetail.Order_id);
                            Console.WriteLine("-Order date        : {0}",orderdetail.Date_Order);
                            foreach (var order in orderdetail.shoesList)
                            {
                                Console.WriteLine("-Product name {0}    : {1}",count,order.Shoes_name);
                                count++;
                                Console.WriteLine("-Amonut            : {0}",order.Amount);
                                Console.WriteLine("-Size              : {0}",order.Size);
                                Console.WriteLine("-Unit price        : {0}vndong",order.Price);
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
                        Console.Write("Please enter a valid value :");
                        continue;
                    }
                    break;
                }
            }
            Console.Write("\n    Press Enter key to back menu... !");
            Console.ReadLine();
            
        }
    }
}

}