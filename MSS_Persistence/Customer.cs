using System;
namespace MSS_Persistence
{
    public class Customer : User
    {
        public int Customer_id{set;get;}
        public string Customer_name{set; get;}

        public int Phone{set; get;}

        public string Address{set ; get;}

        public string Email{set; get;}

    }
}