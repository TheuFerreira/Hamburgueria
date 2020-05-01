﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Controls;

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
            public string TELEPHONE { get; set; }
            public string REFERENCE { get; set; }
        }

        public static void GetAll(DataGrid grid)
        {
            List<Item> items = new List<Item>();

            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT id, nome, rua, numero, bairro, complemento, telefone, referencia FROM Cliente WHERE excluido = 0";

            var r = command.ExecuteReader();
            while (r.Read())
            {
                int id = Convert.ToInt32(r[0].ToString());
                string name = r[1].ToString();
                string address = r[2].ToString();
                string number = r[3].ToString();
                string district = r[4].ToString();
                string complement = r[5].ToString();
                string telephone = r[6].ToString();
                string reference = r[7].ToString();

                items.Add(new Item()
                {
                    ID = id,
                    NAME = name,
                    ADDRESS = address,
                    NUMBER = number,
                    DISTRICT = district,
                    COMPLEMENT = complement,
                    TELEPHONE = telephone,
                    REFERENCE = reference
                });
            }
            r.Close();

            connection.Close();

            for (int i = 0; i < items.Count; i++)
                grid.Items.Add(items[i]);
        }

        public static void Insert(string name, string address, string district, string number, string complement, string telephone, string reference)
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "INSERT INTO cliente" +
                "(nome, rua, numero, bairro, complemento, telefone, referencia) " +
                "VALUES " +
                "(@nome, @rua, @numero, @bairro, @complemento, @telephone, @referencia)";

            command.Parameters.AddWithValue("@nome", name);
            command.Parameters.AddWithValue("@rua", address);
            command.Parameters.AddWithValue("@numero", number);
            command.Parameters.AddWithValue("@bairro", district);
            command.Parameters.AddWithValue("@complemento", complement);
            command.Parameters.AddWithValue("@telephone", telephone);
            command.Parameters.AddWithValue("@referencia", reference);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public static void Update(int id, string name, string address, string district, string number, string complement, string telephone, string reference)
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "UPDATE cliente " +
                "set " +
                "nome = @nome, rua = @rua, numero = @numero, bairro = @bairro, complemento = @complemento, telefone = @telephone, referencia = @referencia " +
                "WHERE " +
                "id = @id";

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@nome", name);
            command.Parameters.AddWithValue("@rua", address);
            command.Parameters.AddWithValue("@numero", number);
            command.Parameters.AddWithValue("@bairro", district);
            command.Parameters.AddWithValue("@complemento", complement);
            command.Parameters.AddWithValue("@telephone", telephone);
            command.Parameters.AddWithValue("@referencia", reference);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public static void Select(DataGrid grid, string text)
        {
            List<Item> items = new List<Item>();

            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT id, nome, rua, numero, bairro, complemento, telefone, referencia FROM Cliente WHERE excluido = 0 and nome like '%" + text + "%'";

            var r = command.ExecuteReader();
            while (r.Read())
            {
                int id = Convert.ToInt32(r[0].ToString());
                string name = r[1].ToString();
                string address = r[2].ToString();
                string number = r[3].ToString();
                string district = r[4].ToString();
                string complement = r[5].ToString();
                string telephone = r[6].ToString();
                string reference = r[7].ToString();

                items.Add(new Item()
                {
                    ID = id,
                    NAME = name,
                    ADDRESS = address,
                    NUMBER = number,
                    DISTRICT = district,
                    COMPLEMENT = complement,
                    TELEPHONE = telephone,
                    REFERENCE = reference
                });
            }
            r.Close();

            connection.Close();

            for (int i = 0; i < items.Count; i++)
                grid.Items.Add(items[i]);
        }

        public static bool Exist(string name, string address, string district, string number)
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT id " +
                "FROM Cliente " +
                "WHERE excluido = 0 and nome = @name and rua = @street and numero = @number and bairro = @district";

            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@street", address);
            command.Parameters.AddWithValue("@number", number);
            command.Parameters.AddWithValue("@district", district);

            bool value = command.ExecuteScalar() == null ? false : true;

            connection.Close();

            return value;
        }

        public static void Delete(int id)
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "UPDATE cliente SET excluido = 1 WHERE id = @id";

            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}
