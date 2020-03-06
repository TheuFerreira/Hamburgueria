﻿using System;
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
            public string NAME { get; set; }
            public string TYPE { get; set; }
        }

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

        public static void Insert(int cod, string name, decimal price, string typeProduct)
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "INSERT INTO produto(id, nome, preco, tipo) VALUES (@cod, @nome, @preço, @tipo)";

            command.Parameters.AddWithValue("@cod", cod);
            command.Parameters.AddWithValue("@nome", name);
            command.Parameters.AddWithValue("@nome", price);
            command.Parameters.AddWithValue("@tipo", typeProduct);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public static void Update(int cod, string name, decimal price, string typeProduct)
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "UPDATE produto SET nome = @name, preco = @price, tipo = @type WHERE id = @id";

            command.Parameters.AddWithValue("@id", cod);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@price", price);
            command.Parameters.AddWithValue("@type", typeProduct);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public static List<Item> Select()
        {
            List<Item> items = new List<Item>();

            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT id, nome, preco, tipo FROM produto WHERE excluido = 0";

            var r = command.ExecuteReader();
            while (r.Read())
            {
                int id = r.GetInt32(0);
                string name = r.GetString(1);
                string type = r.GetString(2);

                items.Add(new Item() { ID = id, NAME = name, TYPE = type });
            }
            r.Close();

            connection.Close();

            return items;
        }

        public static void Delete(int cod)
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = "UPDATE produto SET excluido = 1 WHERE id = @id";

            command.Parameters.AddWithValue("@id", cod);

            command.ExecuteNonQuery();

            connection.Close();
        }

    }
}
