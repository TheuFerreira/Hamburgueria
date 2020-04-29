using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

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

        public static void Create(Model.Cliente.Item address, DateTime dateSale, decimal totalSale, string payment, decimal discount, List<View.VendasDelivery.Item> items, string fileName = "-1")
        {
            int i = Directory.GetFiles(DefaultPath(), "*.bin").Length;

            string path;
            if (fileName == "-1")
                path = DefaultPath() + i + ".bin";
            else
                path = DefaultPath() + fileName + ".bin";

            string content = "";
            content += address.NAME + "\n";
            content += dateSale + "\n";
            content += totalSale + "\n";
            content += address.ADDRESS + ">" + address.NUMBER + ">" + address.DISTRICT + ">" + address.COMPLEMENT + "\n";
            content += address.REFERENCE + "\n";
            content += payment + "\n";
            content += discount + "\n";
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

                string fileName = System.IO.Path.GetFileNameWithoutExtension(files[i]);
                address.NAME = lines[0];
                DateTime dateSale = Convert.ToDateTime(lines[1]);
                decimal totalSale = Convert.ToDecimal(lines[2]);
                string[] addressFile = lines[3].Split('>');
                address.ADDRESS = addressFile[0];
                address.NUMBER = addressFile[1];
                address.DISTRICT = addressFile[2];
                address.COMPLEMENT = addressFile[3];
                address.REFERENCE = lines[4];

                string info = address.NAME + "\n\n";
                info += "ENDEREÇO: " + address.ADDRESS + ", Nº" + address.NUMBER + ", " + address.DISTRICT + ", " + address.COMPLEMENT + "\n";
                info += "REFERÊNCIA: " + address.REFERENCE + "\n";
                info += "FORMA DE PAGAMENTO: " + lines[5] + "\n";
                info += "DESCONTO: R$" + lines[6] + "\n";
                info += "\n";

                info += "PEDIDOS\n";
                for (int j = 8; j < lines.Length; j++)
                {
                    string[] requests = lines[j].Split('>');

                    int id = Convert.ToInt32(requests[0]);
                    int quantity = Convert.ToInt32(requests[1]);

                    var p = Model.Produto.GetProduct(id);
                    info += quantity + "x " + p.NAME + "\t\t" + (p.PRICE * quantity).ToString("C2") + "\n";
                }

                grid.Items.Add(new View.Vendas.Item() { Type = 1, Value = "DELIVERY", File = fileName, Info = info, Date = dateSale, Total = totalSale - Convert.ToDecimal(lines[6]) });
            }
        }

        public static string[] Info(string fileName)
        {
            string[] lines = File.ReadAllLines(DefaultPath() + fileName + ".bin");
            string[] info = new string[7];

            info[0] = lines[0];
            info[1] = lines[1];
            info[2] = lines[2];
            info[3] = lines[3];
            info[4] = lines[4];
            info[5] = lines[5];
            info[6] = lines[6];

            return info;
        }

        public static void Edit(string oldFileName, Model.Cliente.Item address, DateTime dateSale, decimal totalSale, string payment, decimal discount, List<View.VendasDelivery.Item> items)
        {
            Create(address, dateSale, totalSale, payment, discount, items, oldFileName);
        }

        public static bool Exist(string fileName)
        {
            return File.Exists(DefaultPath() + fileName + ".bin");
        }

        public static void Delete(string fileName)
        {
            File.Delete(DefaultPath() + fileName + ".bin");

            string[] files = Directory.GetFiles(DefaultPath(), "*.bin");

            for (int i = 0; i < files.Length; i++)
                File.Move(files[i], DefaultPath() + i + ".bin");
        }

        public static List<View.VendasDelivery.Item> Products(string fileName)
        {
            List<View.VendasDelivery.Item> it = new List<View.VendasDelivery.Item>();

            string[] lines = File.ReadAllLines(DefaultPath() + fileName + ".bin");

            for (int j = 8; j < lines.Length; j++)
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
