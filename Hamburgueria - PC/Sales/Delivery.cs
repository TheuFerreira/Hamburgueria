using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;

namespace Hamburgueria.Sales
{
    public class Delivery
    {
        private static string DefaultPath()
        {
            string pathData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\TWO Sistemas";

            if (Directory.Exists(pathData) == false)
                pathData = Directory.CreateDirectory(pathData).FullName;

            pathData += "\\Delivery";
            if (Directory.Exists(pathData) == false)
                pathData = Directory.CreateDirectory(pathData).FullName;

            return pathData + "\\";
        }

        public static void Create(Tables.Client address, DateTime dateSale, string payment, decimal discount, ObservableCollection<Item> items, string fileName = "-1")
        {
            int i = Directory.GetFiles(DefaultPath(), "*.bin").Length;

            string path;
            if (fileName == "-1")
                path = DefaultPath() + i + ".bin";
            else
                path = DefaultPath() + fileName + ".bin";

            string content = "";
            content += address.Name + "\n";
            content += dateSale + "\n";
            content += address.Street + ">" + address.Number + ">" + address.District + ">" + address.Complement + "\n";
            content += address.Telephone + "\n";
            content += address.Reference + "\n";
            content += payment + "\n";
            content += discount + "\n";
            content += "-\n";

            foreach (Item it in items)
            {
                string obs = it.Name.Substring(new Sql.Product().GetProduct(it.Id).Name.Length + 1);
                content += it.Id + ">" + obs + ">" + it.Quantity + "\n";
            }

            File.WriteAllText(path, content);
        }

        public static void Select(DataGrid grid)
        {
            string[] files = Directory.GetFiles(DefaultPath(), "*.bin", SearchOption.TopDirectoryOnly);

            for (int i = 0; i < files.Length; i++)
            {
                string[] lines = File.ReadAllLines(files[i]);
                Tables.Client address = new Tables.Client();

                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                address.Name = lines[0];
                DateTime dateSale = Convert.ToDateTime(lines[1]);
                string[] addressFile = lines[2].Split('>');
                address.Street = addressFile[0];
                address.Number = Convert.ToInt32(addressFile[1]);
                address.District = addressFile[2];
                address.Complement = addressFile[3];
                address.Telephone = lines[3];
                address.Reference = lines[4];

                string info = address.Name + "\n\n";
                info += "ENDEREÇO: " + address.Street + ", Nº" + address.Number + ", " + address.District + ", " + address.Complement + "\n";
                info += "TELEFONE: " + address.Telephone + "\n";
                info += "REFERÊNCIA: " + address.Reference + "\n";
                info += "FORMA DE PAGAMENTO: " + lines[5] + "\n";
                info += "DESCONTO: R$" + lines[6] + "\n";
                info += "\n";

                info += "PEDIDOS\n";
                decimal totalSale = 0;
                for (int j = 8; j < lines.Length; j++)
                {
                    string[] requests = lines[j].Split('>');

                    int id = Convert.ToInt32(requests[0]);
                    string obs = requests[1];
                    int quantity = Convert.ToInt32(requests[2]);

                    var p = new Sql.Product().GetProduct(id);
                    info += quantity + "x " + p.Name +" " + obs + "\t\t" + (p.Price * quantity).ToString("C2") + "\n";
                    totalSale += p.Price * quantity;
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

        public static void Edit(string oldFileName, Tables.Client address, DateTime dateSale, string payment, decimal discount, ObservableCollection<Item> items)
        {
            Create(address, dateSale, payment, discount, items, oldFileName);
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

        public static ObservableCollection<Item> Products(string fileName)
        {
            ObservableCollection<Item> it = new ObservableCollection<Item>();

            string[] lines = File.ReadAllLines(DefaultPath() + fileName + ".bin");

            for (int j = 8; j < lines.Length; j++)
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
