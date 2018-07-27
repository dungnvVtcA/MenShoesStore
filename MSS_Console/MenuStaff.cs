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
    public class MenuStaff
    {
        
        public static void Menustaff()
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
                Console.WriteLine("2.Orders management");
                Console.WriteLine("3.Reload page");
                Console.WriteLine("4.Log Out");
                Console.Write(line);
                Console.Write("You Choose :");
                var choice = Console.ReadLine();
                if(choice == " ")
                {
                    Console.ReadLine();
                }
                switch(choice)
                {
                    case "1" : MenuStaff.BrowseOrders();
                    break;
                    case "2" :MenuStaff.ListApprovedorders();
                    break;
                    case "3" :MenuStaff.Reload();
                    break;
                    case "4" :
                    MENU m = new MENU();
                    m.MainMENU();
                    break;
                    default :
                    break;
                }
                
                
            }
        }
        public static string full, full2, money,b, money2,status;
        public static bool BrowseOrders()
        {
            Console.Clear();
            string line1 = "------------------------------------------------------------------------------------\n";
            Decimal a = 0;
            int dem= 0;
            OrderBL obl = new OrderBL();
            Orders orderss = new Orders();
            orderss.shoesList = new List<Shoes>();
            var list = obl.GetAllOrderbyStatus(1);
            if(list.Count!= 0)
            {
                decimal k ;
                int sl = 0;
                   string line = "================================================================================================================================================================";
                    Console.WriteLine(line);
                    Console.WriteLine(" Order_id        |        Customer Name                                |   Date Create                 |    Total            |     Status Order                 |");
                    Console.WriteLine(line);
                    foreach (var orders in list)
                    {
                        k = 0;
                        if( orders.Order_status!=0)
                        {
                            var orderdetail = obl.GetOrderDetailsByID(orders.Order_id);
                            foreach (var item in orderdetail.shoesList)
                            {
                                decimal price = item.Price * item.Amount;
                                k +=price;
                                money = String.Format("{0:0,0}vnđ",k);
                                if( orderdetail.shoesList.Count > 1)
                                {
                                    sl = orderdetail.shoesList.Count -1;
                                    full2 = orderdetail.user.User_name+"("+orderdetail.shoesList[0].Shoes_name +" "+"..and"+ sl +" "+"other products "+")";
                                }else
                                {
                                    full2 = orderdetail.user.User_name+"("+orderdetail.shoesList[0].Shoes_name+")";
                                }
                            
                            }
                            if(orderdetail.Order_status == 1)
                            {
                                b = "The order is pending . . ";
                            }
                            else if( orderdetail.Order_status == 2)
                            {
                                b = "The order has been processed .. ";
                            }                
                            Console.WriteLine("{0,-20}{1,-53}{2,-32} {3,-19} {4}",orders.Order_id,full2,orders.Date_Order,money,b);
                        }
                        
                    }
                    Console.WriteLine(line);
                    Console.Write("Do you want to view customer order details? (y/n):");
                    char choicce = Convert.ToChar(Console.ReadLine());
                    if(choicce == 'y')
                    {
                        while(true){
                        Console.Write("Enter the ID to view the customer's order details: ");
                            while (true)
                            {  
                                try
                                {
                                    int or_id = Convert.ToInt32(Console.ReadLine());
                                    var orderdetail = obl.GetOrderDetailsByID(or_id);
                                    if(orderdetail != null && orderdetail.Order_status == 2 )
                                    {
                                        throw new valueexception("Order  does not exist or has been approved,please re-enter : ");
                                    }
                                    else if(orderdetail != null && orderdetail.Order_status == 1)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("          Order Detail of Customer Information        ");
                                        Console.WriteLine("-ID                : {0}",orderdetail.Order_id);
                                        Console.WriteLine("-Order date        : {0}",orderdetail.Date_Order);
                                        Console.WriteLine("-Customer name     : {0}",orderdetail.user.User_name );
                                        Console.WriteLine("-Phone             : {0}",orderdetail.phone);
                                        Console.WriteLine("-Address           : {0}",orderdetail.Address);
                                        Console.WriteLine("-Customer Email    : {0}",orderdetail.user.Email);
                                        Console.Write(line1);
                                        Console.Write(" Product name      |  Unitprice          |  Amount          |  Size     |     Total          \n");
                                        foreach (var order in orderdetail.shoesList)
                                        {
                                            Decimal b = order.Price*order.Amount;
                                            string  money = String.Format("{0:0,0}vnđ",b);
                                            string money3 = String.Format("{0:0,0}vnđ",order.Price);
                                            Console.WriteLine("{0,-20} {1,-26} {2,-15} {3,-10} {4}",order.Shoes_name,money3,order.Amount,order.Size,money);
                                            a +=b;
                                            money2 = String.Format("{0:0,0}vnđ",a);
                                            
                                        
                                        }
                                        Console.Write(line1);
                                        Console.WriteLine("-The total amount payable                                                  {0}",money2);
                                        Console.Write("Do you want to Browse this Order?(y/n) :");
                                        string choice3 = Console.ReadLine();
                                        if( choice3 =="y")
                                        {

                                            var result = obl.GetOrderDetailsByID(or_id);                                          
                                            orderss.Order_id = or_id;
                                            ShoesBL sbl = new ShoesBL();
                                            foreach (var shoes in result.shoesList)
                                            {
                                                orderss.shoesList.Add(sbl.GetShoesById(shoes.Shoes_id));
                                                orderss.shoesList[dem].Amount = shoes.Amount;
                                                dem++;
                                            }
                                            Console.WriteLine("Create Order: " + (obl.update(orderss) ? "completed!" : "not complete!"));
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
                            
                            Console.Write("You want to browse order?(y/n) :");
                            string choice = Console.ReadLine();
                            if( choice == "n")
                            {
                                break;
                                
                            }else if( choice == "y")
                            {
                                BrowseOrders();
                                break;
                            }
                        
                        }
                    }
                    
                return true;
            }
            else if(list.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The orders has been processed!");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.ReadLine();
            }
            return true;

        }
        
        public static void ListApprovedorders()
        
        {
            Console.Clear();
            OrderBL obl = new OrderBL();
            var list = obl.GetAllOrder();
            int b = list.FindIndex(x => x.Order_status == 0);
            int c = list.FindIndex(x => x.Order_status == 1);
            int d = list.FindIndex(x => x.Order_status == 1);
            if( b != -1 && c ==-1 && d == -1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Empty list, no invoices processed...!");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.ReadLine();

            }else 
            {
                decimal k = 0;
                int sl = 0;
                decimal price2 = 0;
                if( list.Count != 0)
                {
                    string line = "====================================================================================================================================================================";
                    Console.WriteLine(line);
                     Console.WriteLine(" Order_id        |        Customer Name                                |   Date Create                 |    Total            |     Status Order                   |");
                    Console.WriteLine(line);
                    foreach (var orders in list)
                    {
                        if( orders.Order_status!=0)
                        {
                            k = 0;
                            var orderdetail = obl.GetOrderDetailsByID(orders.Order_id);
                            foreach (var sh in orderdetail.shoesList)
                            {
                                price2 = sh.Price * sh.Amount;
                                k += price2;
                            
                            }
                            if( orderdetail.shoesList.Count > 1)
                            {
                                sl = orderdetail.shoesList.Count -1;
                                full = orderdetail.user.User_name+"("+orderdetail.shoesList[0].Shoes_name +" "+"..and"+ sl +" "+"other products "+")";
                            }else
                            {
                                full = orderdetail.user.User_name+"("+orderdetail.shoesList[0].Shoes_name+")";
                            }
                            if(orderdetail.Order_status == 1)
                            {
                                status = "The order is pending . . ";
                            }
                            else if( orderdetail.Order_status == 2)
                            {
                                status = "The order has been processed .. ";
                            }                
                            
                            string  money9 = String.Format("{0:0,0}vnđ",k);

                            Console.WriteLine("{0,-20}{1,-53}{2,-32} {3,-19} {4}",orders.Order_id,full,orders.Date_Order,money9,status);
                           
                        }
                        
                    }
                    Console.WriteLine(line);
                    Console.Write("Do you want to see Order details?(y/n)");
                    string choice = Console.ReadLine();
                    if(choice == "y" )
                    {
                       MenuStaff.listorder();
                    }
                    Console.Write("\n    Press Enter key to back menu... !");
                    Console.ReadLine();
                    }
                else if(list.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Empty list, no invoices processed...!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.ReadLine();

                }
            }
            
        }
        private static void listorder()
        {
            Decimal a = 0;
            Orders orderdetail = new Orders();
            OrderBL obl = new OrderBL();
            string line1 = "--------------------------------------------------------------------------------------\n";
            Console.Write("Input  Order_ID: ");
            while (orderdetail.Order_id == null)
            {
                try
                {
                    int or_id = Convert.ToInt32(Console.ReadLine());
                    orderdetail = obl.GetOrderDetailsByID(or_id);
                    if((orderdetail == null))
                    {
                        throw new valueexception("Not find Order_ID,please re-enter : ");
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
            
            }
            if( orderdetail.Order_id != null && orderdetail.Order_status !=0)
            {
                Console.Clear();
                Console.WriteLine("SHOES STORE");
                Console.WriteLine("          Shoe Sales Invoice        ");
                Console.WriteLine("-ID                : {0}",orderdetail.Order_id);
                Console.WriteLine("-Order date        : {0}",orderdetail.Date_Order);
                Console.WriteLine("-Customer name     : {0}",orderdetail.user.User_name );
                Console.WriteLine("-Phone             : {0}",orderdetail.phone);
                Console.WriteLine("-Address           : {0}",orderdetail.Address);
                Console.WriteLine("-Customer Email    : {0}",orderdetail.user.Email);
                Console.Write(line1);
                Console.Write(" Product name      |  Unitprice          |  Amount          |  Size     |     Total            \n");
                foreach (var order in orderdetail.shoesList)
                {
                    Decimal b = order.Price*order.Amount;
                    string  money = String.Format("{0:0,0}vnđ",b);
                    string money3 = String.Format("{0:0,0}vnđ",order.Price);
                    Console.WriteLine("{0,-20} {1,-26} {2,-15} {3,-10} {4}",order.Shoes_name,money3,order.Amount,order.Size,b);
                    a +=b;           
                    money2 = String.Format("{0:0,0}vnđ",a);
                            
                }
                Console.Write(line1);
                Console.WriteLine("-The total amount payable                                                  {0}",money2);
                if(orderdetail.Order_status == 1)
                {
                    Console.WriteLine("The order is pending . . ");
                }
                else if( orderdetail.Order_status == 0)
                {
                    Console.WriteLine("The order has been processed .. ");
                }       
                
            }else
            {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Empty list, no invoices processed...!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.ReadLine();

            }
        }

        public static void Reload()
        {
            Menustaff();
        }

    }
}