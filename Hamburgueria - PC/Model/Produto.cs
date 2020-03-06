using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SQLite;

namespace Hamburgueria.Model
{
    public class Produto : Database
    {

        public static void InsertType(string name)
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "INSERT INTO tipoProduto(tipo) VALUES (@tipo)";
            
            command.Parameters.AddWithValue("@tipo", name);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public static List<string> GetAllType()
        {

            List<string> items = new List<string>();

            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT * FROM tipoProduto";

            var r = command.ExecuteReader();
            while(r.Read())
            {
                string name = r.GetString(0);

                items.Add(name);
            }
            r.Close();
            items.Add("Adicionar...");

            connection.Close();

            return items;
        }

    }
}
