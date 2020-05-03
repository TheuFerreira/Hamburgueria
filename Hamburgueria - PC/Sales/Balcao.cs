using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;

namespace Hamburgueria.Sales
{
    public class Balcao
    {
        private static string DefaultPath()
        {
            string pathData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\TWO Sistemas";

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

            for (int i = 0; i < files.Length; i++)
            {
                string[] lines = File.ReadAllLines(files[i]);

                int numTable = Convert.ToInt32(Path.GetFileNameWithoutExtension(files[i]));
                DateTime dateSale = Convert.ToDateTime(lines[0]);
                decimal totalSale = Convert.ToDecimal(lines[1]);
                string observation = lines[2];

                string info = "MESA: Nº" + numTable + "\n\n";

                for (int j = 4; j < lines.Length; j++)
                {
                    string[] requests = lines[j].Split('>');

                    int id = Convert.ToInt32(requests[0]);
                    int quantity = Convert.ToInt32(requests[1]);

                    var p = new Hamburgueria.Sql.Product().GetProduct(id);
                    info += quantity + "x " + p.Name + "\t\t" + (p.Price * quantity).ToString("C2") + "\n";
                }
                if (string.IsNullOrWhiteSpace(observation) == false)
                    info += "OBSERVAÇÃO: " + observation + "\n";

                grid.Items.Add(new View.Vendas.Item() { Type = 0, Value = "BALCÃO", File = numTable.ToString(), Info = info, Date = dateSale, Total = totalSale });
            }
        }

        public static void Create(int numTable, DateTime dateSale, decimal totalSale, string observation, ObservableCollection<Item> items)
        {
            string path = DefaultPath() + numTable + ".bin";

            string content = "";
            content += dateSale + "\n";
            content += totalSale + "\n";
            content += observation + "\n";
            content += "-\n";

            foreach (Item i in items)
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
            string[] info = new string[3];

            info[0] = lines[0];
            info[1] = lines[1];
            info[2] = lines[2];
            return info;
        }

        public static void Edit(int oldNumTable, int numTable, DateTime dateSale, decimal totalSale, string observation, ObservableCollection<Item> items)
        {
            File.Delete(DefaultPath() + oldNumTable + ".bin");
            Create(numTable, dateSale, totalSale, observation, items);
        }

        public static void Delete(int numTable)
        {
            File.Delete(DefaultPath() + numTable + ".bin");
        }

        public static ObservableCollection<Item> Products(int numTable)
        {
            ObservableCollection<Item> it = new ObservableCollection<Item>();

            string[] lines = File.ReadAllLines(DefaultPath() + numTable + ".bin");

            for (int j = 4; j < lines.Length; j++)
            {
                string[] requests = lines[j].Split('>');

                int id = Convert.ToInt32(requests[0]);
                int quantity = Convert.ToInt32(requests[1]);

                var p = new Hamburgueria.Sql.Product().GetProduct(id);
                it.Add(new Item(id, p.Cod, p.Name, p.Price, quantity ));
            }

            return it;
        }
    }
}
