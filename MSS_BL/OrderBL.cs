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
            return or.GetOrders( 0,null);
        }
        public bool update(int id)
        {
            return or.Update(id);
        }
        public List<Orders> GetAllOrderByIDUser(int user_id)
        {
            return or.GetOrders(1, new Orders{user = new User{User_id=user_id}});
        }
        
    }
}