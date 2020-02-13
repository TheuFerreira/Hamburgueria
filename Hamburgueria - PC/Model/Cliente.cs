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
        public static DataTable GetAll()
        {
            try
            {
                DataTable dt = new DataTable("Cliente");

                connection.Open();

                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "SELECT * FROM Cliente";

                var r = new SQLiteDataAdapter(command);
                r.Fill(dt);

                connection.Close();

                return dt;
            }
            catch (SQLiteException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
