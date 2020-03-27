using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;

namespace Hamburgueria.Sales
{
    public class Balcao
    {
        private static string DefaultPath()
        {
            string pathData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Necos";

            if (Directory.Exists(pathData) == false)
                pathData = Directory.CreateDirectory(pathData).FullName;

            pathData += "\\Balcao";
            if (Directory.Exists(pathData) == false)
                pathData = Directory.CreateDirectory(pathData).FullName;

            return pathData + "\\";
        }

        public static void Select(DataGrid grid)
        {
            string[] files = Directory.GetFiles(DefaultPath(), "*.bin", SearchOption.TopDirectoryOnly);

            grid.Items.Clear();
            for (int i = 0; i < files.Length; i++)
            {
                string[] lines = File.ReadAllLines(files[i]);

                int numTable = Convert.ToInt32(Path.GetFileNameWithoutExtension(files[i]));
                DateTime dateSale = Convert.ToDateTime(lines[0]);
                decimal totalSale = Convert.ToDecimal(lines[1]);

                string info = "MESA: Nº" + numTable + "\n\n";

                for (int j = 3; j < lines.Length; j++)
                {
                    string[] requests = lines[j].Split('>');

                    int id = Convert.ToInt32(requests[0]);
                    int quantity = Convert.ToInt32(requests[1]);

                    var p = Model.Produto.GetProduct(id);
                    info += quantity + "x " + p.NAME + "\t\t" + (p.PRICE * quantity).ToString("C2") + "\n";
                }

                grid.Items.Add(new View.Vendas.Item() { Type = 0, File = numTable.ToString(), Info = info, Date = dateSale, Total = totalSale });
            }
        }

        public static void Create(int numTable, DateTime dateSale, decimal totalSale, List<View.VendasBalcao.Item> items)
        {
            string path = DefaultPath() + numTable + ".bin";

            string content = "";
            content += dateSale + "\n";
            content += totalSale + "\n";
            content += "-\n";

            foreach (View.VendasBalcao.Item i in items)
                content += i.Id + ">" + i.Quantity + "\n";
            File.WriteAllText(path, content);
        }

        public static bool Exist(int numTable)
        {
            return File.Exists(DefaultPath() + numTable + ".bin");
        }

        public static string[] Info(int numTable)
        {
            string[] lines = File.ReadAllLines(DefaultPath() + numTable + ".bin");
            string[] info = new string[2];

            info[0] = lines[0];
            info[1] = lines[1];
            return info;
        }

        public static void Edit(int oldNumTable, int numTable, DateTime dateSale, decimal totalSale, List<View.VendasBalcao.Item> items)
        {
            File.Delete(DefaultPath() + oldNumTable + ".bin");
            Create(numTable, dateSale, totalSale, items);
        }

        public static void Del(int numTable)
        {
            File.Delete(DefaultPath() + numTable + ".bin");
        }

        public static List<View.VendasBalcao.Item> Products(int numTable)
        {
            List<View.VendasBalcao.Item> it = new List<View.VendasBalcao.Item>();

            string[] lines = File.ReadAllLines(DefaultPath() + numTable + ".bin");

            for (int j = 3; j < lines.Length; j++)
            {
                string[] requests = lines[j].Split('>');

                int id = Convert.ToInt32(requests[0]);
                int quantity = Convert.ToInt32(requests[1]);

                var p = Model.Produto.GetProduct(id);
                it.Add(new View.VendasBalcao.Item() { Id = id, Cod = p.COD, Name = p.NAME, Price = p.PRICE, Quantity = quantity, Total = p.PRICE * quantity });
            }

            return it;
        }
    }
}
