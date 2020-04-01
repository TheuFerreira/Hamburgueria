using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SQLite;

namespace Hamburgueria.Model
{
    partial class Relatorio : Database
    {
        public class S
        {
            public string TYPE { get; set; }
            public DateTime Date { get; set; }
            public decimal TotalBrute { get; set; }
            public int Discount { get; set; }
            public decimal Total { get; set; }
            public string Payment { get; set; }

            public S(string type, DateTime date, decimal totalBrute, int discount, decimal total, string payment)
            {
                this.TYPE = type;
                this.Date = date;
                this.TotalBrute = totalBrute;
                this.Discount = discount;
                this.Total = total;
                this.Payment = payment;
            }
        }

        public static List<S> SaleDayLocal(string date)
        {
            List<S> s = new List<S>();

            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "" +
                "SELECT v.date, v.total_bruto, v.desconto, v.total, v.pagamento from venda_mesa vm " +
                "INNER JOIN venda v ON vm.venda_id = v.id " +
                "WHERE v.date like '" + date + "%' " +
                "GROUP BY v.id;";

            var r = command.ExecuteReader();
            while (r.Read())
                s.Add(new S("BALCÃO", r.GetDateTime(0), r.GetDecimal(1), r.GetInt32(2), r.GetDecimal(3), r.GetString(4)));
            r.Close();

            connection.Close();

            return s;
        }
    }
}
