using System;
using System.Collections.Generic;
using MSS_Persistence;
using MSS_DAL;
namespace MSS_Bl
{
    public class OrderBL
    {
        OrdersDAl or = new OrdersDAl();
        public bool CreateOrder(Orders orders)
        {
            bool create = or.CreateOrders(orders);
            return create;
        }
        public List<Orders> GetAllOrder()
        {
            return or.GetAllOrder();
        }
        
    }
}