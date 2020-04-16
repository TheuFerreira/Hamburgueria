using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SQLite;

namespace Hamburgueria.Model
{
    public class Produto : Database
    {

        public class Item
        {
            public int ID { get; set; }
            public int COD { get; set; }
            public string NAME { get; set; }
            public decimal PRICE { get; set; }
        }

        public static Item GetProduct(int id)
        {
            Item i = new Item();

            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT id, cod, nome, preco FROM produto WHERE id = " + id;
            var r = command.ExecuteReader();
            while (r.Read())
            {
                i.ID = r.GetInt32(0);
                i.COD = r.GetInt32(1);
                i.NAME = r.GetString(2);
                i.PRICE = r.GetDecimal(3);
            }
            r.Close();

            connection.Close();
            return i;
        }

        public static void Insert(int cod, string name, decimal price)
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "INSERT INTO produto(cod, nome, preco) VALUES (@cod, @nome, @preco)";

            command.Parameters.AddWithValue("@cod", cod);
            command.Parameters.AddWithValue("@nome", name);
            command.Parameters.AddWithValue("@preco", price);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public static void Update(int id, int cod, string name, decimal price)
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "UPDATE produto SET cod = @cod,  nome = @name, preco = @price WHERE id = @id";

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@cod", cod);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@price", price);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public static List<Item> Select()
        {
            List<Item> items = new List<Item>();

            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT id, cod, nome, preco FROM produto WHERE excluido = 0";

            var r = command.ExecuteReader();
            while (r.Read())
            {
                int id = r.GetInt32(0);
                int cod = r.GetInt32(1);
                string name = r.GetString(2);
                decimal price = r.GetDecimal(3);

                items.Add(new Item() { ID = id, COD = cod, NAME = name, PRICE = price });
            }
            r.Close();

            connection.Close();

            return items;
        }

        public static List<Item> Select(int codigo)
        {
            List<Item> items = new List<Item>();

            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT id, cod, nome, preco FROM produto WHERE excluido = 0 AND cod like '%" + codigo + "%'";

            var r = command.ExecuteReader();
            while (r.Read())
            {
                int id = r.GetInt32(0);
                int cod = r.GetInt32(1);
                string name = r.GetString(2);
                decimal price = r.GetDecimal(3);

                items.Add(new Item() { ID = id, COD = cod, NAME = name, PRICE = price });
            }
            r.Close();

            connection.Close();

            return items;
        }

        public static List<Item> Select(string nome)
        {
            List<Item> items = new List<Item>();

            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT id, cod, nome, preco FROM produto WHERE excluido = 0 AND nome like '%" + nome + "%'";

            var r = command.ExecuteReader();
            while (r.Read())
            {
                int id = r.GetInt32(0);
                int cod = r.GetInt32(1);
                string name = r.GetString(2);
                decimal price = r.GetDecimal(3);

                items.Add(new Item() { ID = id, COD = cod, NAME = name, PRICE = price });
            }
            r.Close();

            connection.Close();

            return items;
        }

        public static void Delete(int id)
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "UPDATE produto SET excluido = 1 WHERE id = @id";

            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public static bool CodExist(int cod)
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT * FROM produto WHERE cod = @cod and excluido = 0";

            command.Parameters.AddWithValue("@cod", cod);

            bool exist = command.ExecuteScalar() != null ? true : false;

            connection.Close();

            return exist;
        }
    }
}
