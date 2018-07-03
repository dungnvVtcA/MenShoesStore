using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MSS_Persistence;
namespace MSS_DAL
{
    public class ShoesDAL
    {
        public List<Shoes> GetAllShoes()
        {
           lock (this)
           {
               string  query = "Select Shoes.Shoes_id ,Shoes.Shoes_name,Trademark.TM_id,Shoes.Color,Shoes.Material,Shoes.Price,Shoes.Size,Shoes.Manufacturers,Shoes.Amount,Shoes.Style from Shoes inner join Trademark on Shoes.TM_id = Trademark.TM_id;";
           DBHelper.OpenConnection();
           MySqlDataReader reader = DBHelper.ExecQuery(query);
           List<Shoes> sh1 = new List<Shoes>();
           while(reader.Read())
           {
                sh1.Add(GetShoes(reader));
           }
           reader.Close();
           DBHelper.CloseConnection();
           return sh1;
           }
        }
        private static Shoes GetShoes(MySqlDataReader reader)
        {
            Shoes sh = new Shoes();
                sh.Shoes_id = reader.GetInt32("Shoes_id");
                sh.Shoes_name = reader.GetString("Shoes_name");
                sh.TM.Trademark_id = reader.GetInt16("TM_id");
                sh.Color = reader.GetString("Color");
                sh.Material = reader.GetString("Material");
                sh.Price = reader.GetDecimal("Price");
                sh.Size = reader.GetInt32("Size");
                sh.Manufacture = reader.GetString("Manufacturers");
                sh.Style = reader.GetString("Style");
                sh.Amount = reader.GetInt32("Amount");
                return sh;
        }
        public Shoes GetShoesById(int Shoes_id)
        {
            
            lock (this)
            {
                string  query = "Select Shoes.Shoes_id ,Shoes.Shoes_name,Trademark.TM_id,Shoes.Color,Shoes.Material,Shoes.Price,Shoes.Size,Shoes.Manufacturers,Shoes.Style,Shoes.Amount from Shoes inner join Trademark on Shoes.TM_id = Trademark.TM_id where Shoes.Shoes_id='"+Shoes_id+"';";
            DBHelper.OpenConnection();
            MySqlDataReader reader = DBHelper.ExecQuery(query);
            Shoes sh = new Shoes();
            if(reader.Read())
            {
                sh = GetShoes(reader);
            

            }
            DBHelper.CloseConnection();

            return sh;
            }

        }
        public Shoes GetShoesByName(string Shoes_name)
        {
            
            lock (this)
            {
                string  query = "Select Shoes.Shoes_id ,Shoes.Shoes_name,Trademark.TM_id,Shoes.Color,Shoes.Material,Shoes.Price,Shoes.Style,Shoes.Size,Shoes.Manufacturers,Shoes.Amount from Shoes inner join Trademark on Shoes.TM_id = Trademark.TM_id where Shoes.Shoes_Name='"+Shoes_name+"';";
            DBHelper.OpenConnection();
            MySqlDataReader reader = DBHelper.ExecQuery(query);
            Shoes sh = null;
            if(reader.Read())
            {
                sh = GetShoes(reader);

            }
            reader.Close();
            DBHelper.CloseConnection();
            return sh;
            }

        }
    }
}