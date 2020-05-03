using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Hamburgueria
{
    class TXT
    {
        public static int IdFile { get; set; } = 0;

        public static string Path()
        {
            string pathData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\TWO Sistemas";

            if (Directory.Exists(pathData) == false)
                pathData = Directory.CreateDirectory(pathData).FullName;

            return pathData;
        }

        public static void Sale(DateTime dateSale, decimal totalBrute, decimal discount, decimal totalValue, decimal valuePay, decimal change, string payment, ObservableCollection<Item> products)
        {
            string content = "            BIG BURGUER";
            content += "\nRUA TOCANTINS, Nº95, MORRO DAS BICAS";
            content += "\nTELEFONE: 3543-0336";
            content += "\nWhatsapp: 99303-2638";
            content += "\n------------------------------------";
            content += "\n          CUPOM NÃO FISCAL";
            content += "\n------------------------------------";
            content += "\nPRODUTOS";
            foreach (Item p in products)
            {
                string nameProduct = p.Name;
                if (p.Name.Length >= 21)
                {
                    nameProduct = p.Name.Substring(0, 20);
                    nameProduct += "-";
                    content += "\n" + p.Quantity + "x " + nameProduct.PadRight(25);
                    p.Name = p.Name.Substring(20);

                    content += "\n   " + p.Name.PadRight(21) + p.Total.ToString("C2");
                }
                else
                {
                    content += "\n" + p.Quantity + "x " + p.Name.PadRight(21) + p.Total.ToString("C2");
                }
            }

            content += "\n------------------------------------";

            content += "\nTOTAL BRUTO: R$" + totalBrute.ToString("N2");
            content += "\nDESCONTO: R$" + discount.ToString("N2");
            content += "\nTOTAL: R$" + totalValue.ToString("N2");

            if (payment == "À VISTA")
            {
                content += "\nVALOR PAGO: R$" + valuePay.ToString("N2");
                content += "\nTROCO: R$" + change.ToString("N2");
            }

            content += "\nFORMA DE PAGAMENTO: " + payment;
            content += "\nDATA DA EMISSÃO: " + dateSale;

            content += "\n------------------------------------";

            content += "\n       Agradecemos a preferência";
            content += "\n             Volte Sempre!";

            File.WriteAllText(Path() + "\\sale" + IdFile + ".txt", content, System.Text.Encoding.UTF8);
        }

        public static void Sale(DateTime dateSale, decimal totalBrute, decimal discount, decimal totalValue, decimal valuePay, decimal change, string payment, string observation, ObservableCollection<Item> products)
        {
            string content = "            BIG BURGUER";
            content += "\nRUA TOCANTINS, Nº95, MORRO DAS BICAS";
            content += "\nTELEFONE: 3543-0336";
            content += "\nWhatsapp: 99303-2638";
            content += "\n------------------------------------";
            content += "\n          CUPOM NÃO FISCAL";
            content += "\n------------------------------------";
            content += "\nPRODUTOS";
            foreach (Item p in products)
            {
                string nameProduct = p.Name;
                if (p.Name.Length >= 21)
                {
                    nameProduct = p.Name.Substring(0, 20);
                    nameProduct += "-";
                    content += "\n" + p.Quantity + "x " + nameProduct.PadRight(25);
                    p.Name = p.Name.Substring(20);

                    content += "\n   " + p.Name.PadRight(21) + p.Total.ToString("C2");
                }
                else
                {
                    content += "\n" + p.Quantity + "x " + p.Name.PadRight(21) + p.Total.ToString("C2");
                }
            }
            if (string.IsNullOrWhiteSpace(observation) == false)
                content += ReformText("OBSERVAÇÃO: " + observation);

            content += "\n------------------------------------";

            content += "\nTOTAL BRUTO: R$" + totalBrute.ToString("N2");
            content += "\nDESCONTO: R$" + discount.ToString("N2");
            content += "\nTOTAL: R$" + totalValue.ToString("N2");

            if (payment == "À VISTA")
            {
                content += "\nVALOR PAGO: R$" + valuePay.ToString("N2");
                content += "\nTROCO: R$" + change.ToString("N2");
            }

            content += "\nFORMA DE PAGAMENTO: " + payment;
            content += "\nDATA DA EMISSÃO: " + dateSale;

            content += "\n------------------------------------";

            content += "\n       Agradecemos a preferência";
            content += "\n             Volte Sempre!";

            File.WriteAllText(Path() + "\\sale" + IdFile + ".txt", content, System.Text.Encoding.UTF8);
        }

        public static void Sale(Tables.Client clientItem, DateTime dateSale, decimal totalBrute, decimal discount, decimal totalValue, string payment, string observation, ObservableCollection<Item> products)
        {
            string content = "            BIG BURGUER";
            content += "\nRUA TOCANTINS, Nº95, MORRO DAS BICAS";
            content += "\nTELEFONE: 3543-0336";
            content += "\nWhatsapp: 99303-2638";
            content += "\n------------------------------------";
            content += "\n          CUPOM NÃO FISCAL";

            content += "\n------------------------------------";

            content += ReformText("CLIENTE: " + clientItem.Name);
            content += ReformText("RUA: " + clientItem.Street);
            content += ReformText("NÚMERO: " + clientItem.Number);
            content += ReformText("BAIRRO: " + clientItem.District);
            content += ReformText("COMPLEMENTO: " + clientItem.Complement);
            content += ReformText("TELEFONE: " + clientItem.Telephone);
            if (!string.IsNullOrWhiteSpace(clientItem.Reference))
                content += ReformText("REFERÊNCIA: " + clientItem.Reference);

            content += "\n------------------------------------";
            content += "\nPRODUTOS";
            foreach (Item p in products)
            {
                string nameProduct = p.Name;
                if (p.Name.Length >= 21)
                {
                    nameProduct = p.Name.Substring(0, 20);
                    nameProduct += "-";
                    content += "\n" + p.Quantity + "x " + nameProduct.PadRight(25);
                    p.Name = p.Name.Substring(20);

                    content += "\n   " + p.Name.PadRight(21) + p.Total.ToString("C2");
                }
                else
                {
                    content += "\n" + p.Quantity + "x " + p.Name.PadRight(21) + p.Total.ToString("C2");
                }
            }
            if (string.IsNullOrWhiteSpace(observation) == false)
                content += ReformText("OBSERVAÇÃO: " + observation);

            content += "\n------------------------------------";

            content += "\nTOTAL BRUTO: R$" + totalBrute.ToString("N2");
            content += "\nDESCONTO: R$" + discount.ToString("N2");
            content += "\nTOTAL: R$" + totalValue.ToString("N2");

            content += "\nFORMA DE PAGAMENTO: " + payment;
            content += "\nDATA DA EMISSÃO: " + dateSale;

            content += "\n------------------------------------";

            content += "\n       Agradecemos a preferência";
            content += "\n             Volte Sempre!";

            File.WriteAllText(Path() + "\\sale" + IdFile + ".txt", content, System.Text.Encoding.UTF8);
        }

        private static string ReformText(string text)
        {
            string[] words = text.Split(' ');

            string lines = "";
            string line = "\n";

            for (int i = 0; i < words.Length; i++)
            {
                if ((line + words[i]).Length <= 36)
                {
                    line += words[i] + " ";
                }
                else
                {
                    lines += line;
                    line = "\n    " + words[i] + " ";
                }

                if (i == words.Length - 1)
                    lines += line;
            }

            return lines;
        }
    }
}
