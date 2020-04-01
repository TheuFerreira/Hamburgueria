using System;

using System.Data.SQLite;

namespace Hamburgueria.Model
{
    partial class Relatorio : Database
    {

        public static DateTime GetMinDate()
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT MIN(date) FROM venda;";

            DateTime date = DateTime.Now;
            var r = command.ExecuteReader();
            while (r.Read())
                date = r.GetDateTime(0);
            r.Close();

            connection.Close();

            return date;
        }

    }
}
