using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SQLite;

namespace Hamburgueria.Model
{
    public class Cliente : Database
    {
        public class Item
        { 
            public int ID { get; set; }
            public string NAME { get; set; }
            public decimal PRICE { get; set; }
        }

        public static List<Item> GetAll()
        {
            try
            {
                List<Item> items = new List<Item>();

                connection.Open();

                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "SELECT * FROM Cliente";

                var r = command.ExecuteReader();
                while (r.Read())
                {
                    int id = Convert.ToInt32(r[0].ToString());
                    string name = r[1].ToString();
                    decimal price = Convert.ToDecimal(r[2].ToString());

                    items.Add(new Item() { ID = id, NAME = name, PRICE = price });
                }
                r.Close();

                connection.Close();

                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
