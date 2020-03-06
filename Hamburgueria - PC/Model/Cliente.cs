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
            public string ADDRESS { get; set; }
            public string NUMBER { get; set; }
            public string DISTRICT { get; set; }
            public string COMPLEMENT { get; set; }
            public string REFERENCE { get; set; }
        }

        public static List<Item> GetAll()
        {
            List<Item> items = new List<Item>();

            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT id, nome, rua, numero, bairro, complemento, referencia FROM Cliente WHERE excluido = 0";

            var r = command.ExecuteReader();
            while (r.Read())
            {
                int id = Convert.ToInt32(r[0].ToString());
                string name = r[1].ToString();
                string address = r[2].ToString();
                string number = r[3].ToString();
                string district = r[4].ToString();
                string complement = r[5].ToString();
                string reference = r[6].ToString();

                items.Add(new Item()
                {
                    ID = id,
                    NAME = name,
                    ADDRESS = address,
                    NUMBER = number,
                    DISTRICT = district,
                    COMPLEMENT = complement,
                    REFERENCE = reference
                });
            }
            r.Close();

            connection.Close();

            return items;
        }

        public static void Insert(string name, string address, string district, string number, string complement, string reference)
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "INSERT INTO cliente" +
                "(nome, rua, numero, bairro, complemento, referencia) " +
                "VALUES " +
                "(@nome, @rua, @numero, @bairro, @complemento, @referencia)";

            command.Parameters.AddWithValue("@nome", name);
            command.Parameters.AddWithValue("@rua", address);
            command.Parameters.AddWithValue("@numero", number);
            command.Parameters.AddWithValue("@bairro", district);
            command.Parameters.AddWithValue("@complemento", complement);
            command.Parameters.AddWithValue("@referencia", reference);

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}
