using System;
using System.Collections.ObjectModel;
using System.IO;

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

        public static void Select(ObservableCollection<View.Sale> sales)
        {
            string[] files = Directory.GetFiles(DefaultPath(), "*.bin", SearchOption.TopDirectoryOnly);

            for (int i = 0; i < files.Length; i++)
            {
                string[] lines = File.ReadAllLines(files[i]);

                int numTable = Convert.ToInt32(Path.GetFileNameWithoutExtension(files[i]));
                DateTime dateSale = Convert.ToDateTime(lines[0]);

                string info = "MESA: Nº" + numTable + "\n\n";
                decimal totalSale = 0;
                for (int j = 2; j < lines.Length; j++)
                {
                    string[] requests = lines[j].Split('>');

                    int id = Convert.ToInt32(requests[0]);
                    string obs = requests[1];
                    int quantity = Convert.ToInt32(requests[2]);

                    var p = new Sql.Product().GetProduct(id);
                    info += quantity + "x " + p.Name + " " + obs + "\t\t" + (p.Price * quantity).ToString("C2") + "\n";
                    totalSale += p.Price * quantity;
                }

                sales.Add(new View.Sale() { Type = 0, Value = "BALCÃO", File = numTable.ToString(), Info = info, Date = dateSale, Total = totalSale });
            }
        }

        public static void Create(int numTable, DateTime dateSale, ObservableCollection<Item> items)
        {
            string path = DefaultPath() + numTable + ".bin";

            string content = "";
            content += dateSale + "\n";
            content += "-\n";

            foreach (Item i in items)
            {
                string obs = i.Name.Substring(new Sql.Product().GetProduct(i.Id).Name.Length + 1);
                content += i.Id + ">" + obs + ">" + i.Quantity + "\n";
            }
            File.WriteAllText(path, content);
        }

        public static bool Exist(int numTable)
        {
            return File.Exists(DefaultPath() + numTable + ".bin");
        }

        public static string[] Info(int numTable)
        {
            string[] lines = File.ReadAllLines(DefaultPath() + numTable + ".bin");
            string[] info = new string[1];

            info[0] = lines[0];
            return info;
        }

        public static void Edit(int oldNumTable, int numTable, DateTime dateSale, ObservableCollection<Item> items)
        {
            File.Delete(DefaultPath() + oldNumTable + ".bin");
            Create(numTable, dateSale, items);
        }

        public static void Delete(int numTable)
        {
            File.Delete(DefaultPath() + numTable + ".bin");
        }

        public static ObservableCollection<Item> Products(int numTable)
        {
            ObservableCollection<Item> it = new ObservableCollection<Item>();

            string[] lines = File.ReadAllLines(DefaultPath() + numTable + ".bin");

            for (int j = 2; j < lines.Length; j++)
            {
                string[] requests = lines[j].Split('>');

                int id = Convert.ToInt32(requests[0]);
                string obs = requests[1];
                int quantity = Convert.ToInt32(requests[2]);

                var p = new Sql.Product().GetProduct(id);
                it.Add(new Item(id, p.Cod, p.Name + " " + obs, p.Price, quantity));
            }

            return it;
        }
    }
}
