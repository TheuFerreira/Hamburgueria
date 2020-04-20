using System.Collections.Generic;
using System;
using System.Data.SQLite;
using System.Windows;
namespace Hamburgueria.Model
{
    partial class Relatorio : Database
    {
        public class Cliente
        {
            public DateTime Date { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public decimal TotalBruto { get; set; }
            public decimal Desconto { get; set; }
            public decimal Total { get; set; }
            public string Payment { get; set; }

            public Cliente(DateTime date, string name, string address, decimal totalBruto, decimal desconto, decimal total, string payment)
            {
                this.Date = date;
                this.Name = name;
                this.Address = address;
                this.TotalBruto = totalBruto;
                this.Desconto = desconto;
                this.Total = total;
                this.Payment = payment;
            }
        }

        public static List<Cliente> Client(string date)
        {
            List<Cliente> p = new List<Cliente>();

            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "" + 
                "SELECT v.date, vd.name, vd.address, v.total_bruto, v.desconto, v.total, v.pagamento " +
                "FROM venda_delivery vd " +
                "INNER JOIN venda v ON v.id = vd.venda_id " +
                "WHERE v.date like '" + date + "%';";

            var r = command.ExecuteReader();
            while (r.Read())
                p.Add(new Relatorio.Cliente(r.GetDateTime(0), r.GetString(1), r.GetString(2), r.GetDecimal(3), r.GetDecimal(4), r.GetDecimal(5), r.GetString(6)));
            r.Close();

            connection.Close();

            return p;
        }

        public static List<Cliente> Client(string startDate, string endDate)
        {
            List<Cliente> p = new List<Cliente>();

            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "" +
                "SELECT v.date, vd.name, vd.address, v.total_bruto, v.desconto, v.total, v.pagamento " +
                "FROM venda_delivery vd " +
                "INNER JOIN venda v ON v.id = vd.venda_id " +
                "WHERE v.date >= '" + startDate + "%' and v.date <= '" + endDate + "%';";

            var r = command.ExecuteReader();
            while (r.Read())
                p.Add(new Relatorio.Cliente(r.GetDateTime(0), r.GetString(1), r.GetString(2), r.GetDecimal(3), r.GetDecimal(4), r.GetDecimal(5), r.GetString(6)));
            r.Close();

            connection.Close();

            return p;
        }
    }
}
