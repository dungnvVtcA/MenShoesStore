using System;
using System.Collections.Generic;

namespace MSS_Persistence
{
    public class Orders
    {
        public int Order_id{set;get;}

        public DateTime Date_Order{set;get;}

        public Customer customer{get;set;} = new Customer();

        public Staff staff{set;get;} = new Staff();

        public int Order_status{set;get;}

        public List<Shoes> shoesList{set;get;} = new List<Shoes>();
    }
}