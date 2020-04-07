using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Hamburgueria.Model
{
    class Venda : Database
    {

        public static void Insert(DateTime date, decimal totalBruto, decimal desconto, decimal total, string pagamento, List<View.VendasRapida.Item> items)
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "INSERT INTO venda(date, total_bruto, desconto, total, pagamento) VALUES (@date, @totalBruto, @desconto, @total, @pagamento)";

            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@totalBruto", totalBruto);
            command.Parameters.AddWithValue("@desconto", desconto);
            command.Parameters.AddWithValue("@total", total);
            command.Parameters.AddWithValue("@pagamento", pagamento);

            command.ExecuteNonQuery();

            command.CommandText = "SELECT MAX(id) FROM venda LIMIT 1";
            var r = command.ExecuteReader();
            int saleId = 1;
            while (r.Read())
                saleId = r.GetInt32(0);
            r.Close();

            foreach (View.VendasRapida.Item it in items)
            {
                command = new SQLiteCommand(connection);
                command.CommandText = "INSERT INTO produto_venda(venda_id, produto_id, nome, preco, quantidade, total) VALUES (@saleId, @id, @name, @price, @qtd, @total)";
                command.Parameters.AddWithValue("@saleId", saleId);
                command.Parameters.AddWithValue("@id", it.Id);
                command.Parameters.AddWithValue("@name", it.Name);
                command.Parameters.AddWithValue("@price", it.Price);
                command.Parameters.AddWithValue("@qtd", it.Quantity);
                command.Parameters.AddWithValue("@total", it.Total);
                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        public static void Insert(int numTable, DateTime date, decimal totalBruto, decimal desconto, decimal total, string pagamento, List<View.VendasBalcao.Item> items)
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "INSERT INTO venda(date, total_bruto, desconto, total, pagamento) VALUES (@date, @totalBruto, @desconto, @total, @pagamento)";

            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@totalBruto", totalBruto);
            command.Parameters.AddWithValue("@desconto", desconto);
            command.Parameters.AddWithValue("@total", total);
            command.Parameters.AddWithValue("@pagamento", pagamento);

            command.ExecuteNonQuery();

            command.CommandText = "SELECT MAX(id) FROM venda LIMIT 1";
            var r = command.ExecuteReader();
            int saleId = 1;
            while (r.Read())
                saleId = r.GetInt32(0);
            r.Close();

            command.CommandText = "INSERT INTO venda_mesa(venda_id, num_table) VALUES (@id, @table)";
            command.Parameters.AddWithValue("@id", saleId);
            command.Parameters.AddWithValue("@table", numTable);
            command.ExecuteNonQuery();

            foreach (View.VendasBalcao.Item it in items)
            {
                command = new SQLiteCommand(connection);
                command.CommandText = "INSERT INTO produto_venda(venda_id, produto_id, nome, preco, quantidade, total) VALUES (@saleId, @id, @name, @price, @qtd, @total)";
                command.Parameters.AddWithValue("@saleId", saleId);
                command.Parameters.AddWithValue("@id", it.Id);
                command.Parameters.AddWithValue("@name", it.Name);
                command.Parameters.AddWithValue("@price", it.Price);
                command.Parameters.AddWithValue("@qtd", it.Quantity);
                command.Parameters.AddWithValue("@total", it.Total);
                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        public static void Insert(Cliente.Item client, DateTime date, decimal totalBruto, decimal desconto, decimal total, string pagamento, List<View.VendasDelivery.Item> items)
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "INSERT INTO venda(date, total_bruto, desconto, total, pagamento) VALUES (@date, @totalBruto, @desconto, @total, @pagamento)";

            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@totalBruto", totalBruto);
            command.Parameters.AddWithValue("@desconto", desconto);
            command.Parameters.AddWithValue("@total", total);
            command.Parameters.AddWithValue("@pagamento", pagamento);
            command.ExecuteNonQuery();

            command.CommandText = "SELECT MAX(id) FROM venda LIMIT 1";
            var r = command.ExecuteReader();
            int saleId = 1;
            while (r.Read())
                saleId = r.GetInt32(0);
            r.Close();

            command.CommandText = "INSERT INTO venda_delivery(venda_id, name, address) VALUES (@sale_id, @name, @address)";
            command.Parameters.AddWithValue("@sale_id", saleId);
            command.Parameters.AddWithValue("@name", client.NAME);
            command.Parameters.AddWithValue("@address", client.ADDRESS + ", Nº" + client.NUMBER + ", " + client.DISTRICT + ", "  + client.COMPLEMENT);
            command.ExecuteNonQuery();

            foreach (View.VendasDelivery.Item it in items)
            {
                command = new SQLiteCommand(connection);
                command.CommandText = "INSERT INTO produto_venda(venda_id, produto_id, nome, preco, quantidade, total) VALUES (@saleId, @id, @name, @price, @qtd, @total)";
                command.Parameters.AddWithValue("@saleId", saleId);
                command.Parameters.AddWithValue("@id", it.Id);
                command.Parameters.AddWithValue("@name", it.Name);
                command.Parameters.AddWithValue("@price", it.Price);
                command.Parameters.AddWithValue("@qtd", it.Quantity);
                command.Parameters.AddWithValue("@total", it.Total);
                command.ExecuteNonQuery();
            }


            connection.Close();
        }
    }
}
