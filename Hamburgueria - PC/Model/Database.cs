using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SQLite;

namespace Hamburgueria.Model
{
    public class Database
    {
        private static readonly string DATASOURCE = "DATASOURCE=hamburgueria.db";
        protected static SQLiteConnection connection = new SQLiteConnection(DATASOURCE);
    }
}
