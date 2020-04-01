using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;

namespace Hamburgueria.Sales
{
    public class Delivery
    {
        private static string DefaultPath()
        {
            string pathData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Necos";

            if (Directory.Exists(pathData) == false)
                pathData = Directory.CreateDirectory(pathData).FullName;

            pathData += "\\Delivery";
            if (Directory.Exists(pathData) == false)
                pathData = Directory.CreateDirectory(pathData).FullName;

            return pathData + "\\";
        }

        public static void Create(Model.Cliente.Item address, DateTime dateSale, decimal totalSale, List<View.VendasDelivery.Item> items)
        {
            string path = DefaultPath() + address.NAME + ".bin";

            string content = "";
            content += dateSale + "\n";
            content += totalSale + "\n";
            content += address.ADDRESS + ">" + address.NUMBER + ">" + address.COMPLEMENT + "\n";
            content += "-\n";

            foreach (View.VendasDelivery.Item it in items)
                content += it.Id + ">" + it.Quantity + "\n";

            File.WriteAllText(path, content);
        }

        public static void Select(DataGrid grid)
        {
            string[] files = Directory.GetFiles(DefaultPath(), "*.bin", SearchOption.TopDirectoryOnly);

            for (int i = 0; i < files.Length; i++)
            {
                string[] lines = File.ReadAllLines(files[i]);
                Model.Cliente.Item address = new Model.Cliente.Item();

                address.NAME = Path.GetFileNameWithoutExtension(files[i]);
                DateTime dateSale = Convert.ToDateTime(lines[0]);
                decimal totalSale = Convert.ToDecimal(lines[1]);
                string[] addressFile = lines[2].Split('>');
                address.ADDRESS = addressFile[0];
                address.NUMBER = addressFile[1];
                address.COMPLEMENT = addressFile[2];

                string info = address.NAME + "\n\n";
                info += address.ADDRESS + ", Nº" + address.NUMBER + "," + address.COMPLEMENT + "\n\n";

                for (int j = 4; j < lines.Length; j++)
                {
                    string[] requests = lines[j].Split('>');

                    int id = Convert.ToInt32(requests[0]);
                    int quantity = Convert.ToInt32(requests[1]);

                    var p = Model.Produto.GetProduct(id);
                    info += quantity + "x " + p.NAME + "\t\t" + (p.PRICE * quantity).ToString("C2") + "\n";
                }

                grid.Items.Add(new View.Vendas.Item() { Type = 1, Value = "DELIVERY", File = address.NAME, Info = info, Date = dateSale, Total = totalSale });
            }
        }

        public static string[] Info(string nameClient)
        {
            string[] lines = File.ReadAllLines(DefaultPath() + nameClient + ".bin");
            string[] info = new string[3];

            info[0] = lines[0];
            info[1] = lines[1];
            info[2] = lines[2];
            return info;
        }

        public static void Edit(string oldNameClient, Model.Cliente.Item address, DateTime dateSale, decimal totalSale, List<View.VendasDelivery.Item> items)
        {
            File.Delete(DefaultPath() + oldNameClient + ".bin");
            Create(address, dateSale, totalSale, items);
        }

        public static bool Exist(string nameClient)
        {
            return File.Exists(DefaultPath() + nameClient + ".bin");
        }

        public static void Delete(string nameClient)
        {
            File.Delete(DefaultPath() + nameClient + ".bin");
        }

        public static List<View.VendasDelivery.Item> Products(string nameClient)
        {
            List<View.VendasDelivery.Item> it = new List<View.VendasDelivery.Item>();

            string[] lines = File.ReadAllLines(DefaultPath() + nameClient + ".bin");

            for (int j = 4; j < lines.Length; j++)
            {
                string[] requests = lines[j].Split('>');

                int id = Convert.ToInt32(requests[0]);
                int quantity = Convert.ToInt32(requests[1]);

                var p = Model.Produto.GetProduct(id);
                it.Add(new View.VendasDelivery.Item() { Id = id, Cod = p.COD, Name = p.NAME, Price = p.PRICE, Quantity = quantity, Total = p.PRICE * quantity });
            }

            return it;
        }
    }
}
