using System;
using System.Collections.Generic;
using System.IO;

namespace Hamburgueria
{
    class PDF
    {
        public static string Path()
        {
            string pathData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Necos";

            if (Directory.Exists(pathData) == false)
                pathData = Directory.CreateDirectory(pathData).FullName;

            return pathData;
        }

        public static void Sale(DateTime dateSale, decimal totalBrute, decimal discount, decimal totalValue, decimal valuePay, decimal change, string payment, List<View.VendasRapida.Item> products)
        {
            string content = "BIG BURGUER LANCHES";
            content += "\nRUA TOCANTINS, Nº 395, MORRO DAS BICAS";
            content += "\nTELEFONE: ESPERANDO IGOR";
            content += "\n--------------------------------------";
            content += "\nCUPOM NÃO FISCAL";
            content += "\n--------------------------------------";
            content += "\n" + "PRODUTOS";
            foreach (View.VendasRapida.Item p in products)
                content += "\n" + p.Quantity + "x " + p.Name + "  " + p.Total.ToString("C2");

            content += "\n--------------------------------------";

            content += "\nTOTAL BRUTO: R$" + totalBrute.ToString("N2");
            content += "\nDESCONTO: R$" + discount.ToString("N2");
            content += "\nTOTAL: R$" + totalValue.ToString("N2");
            content += "\nVALOR PAGO: R$" + valuePay.ToString("N2");
            content += "\nTROCO: R$" + change.ToString("N2");

            content += "\nFORMA DE PAGAMENTO: " + payment;
            content += "\nDATA DA EMISSÃO: " + dateSale;

            File.WriteAllText(Path() + "\\sale.txt", content, System.Text.Encoding.UTF8);
        }

        public static void Sale(DateTime dateSale, decimal totalBrute, decimal discount, decimal totalValue, decimal valuePay, decimal change, string payment, List<View.VendasBalcao.Item> products)
        {
            string content = "BIG BURGUER LANCHES";
            content += "\nRUA TOCANTINS, Nº 395, MORRO DAS BICAS";
            content += "\nTELEFONE: ESPERANDO IGOR";
            content += "\n--------------------------------------";
            content += "\nCUPOM NÃO FISCAL";
            content += "\n--------------------------------------";
            content += "\n" + "PRODUTOS";
            foreach (View.VendasBalcao.Item p in products)
                content += "\n" + p.Quantity + "x " + p.Name + "  " + p.Total.ToString("C2");

            content += "\n--------------------------------------";

            content += "\nTOTAL BRUTO: R$" + totalBrute.ToString("N2");
            content += "\nDESCONTO: R$" + discount.ToString("N2");
            content += "\nTOTAL: R$" + totalValue.ToString("N2");
            content += "\nVALOR PAGO: R$" + valuePay.ToString("N2");
            content += "\nTROCO: R$" + change.ToString("N2");

            content += "\nFORMA DE PAGAMENTO: " + payment;
            content += "\nDATA DA EMISSÃO: " + dateSale;

            File.WriteAllText(Path() + "\\sale.txt", content, System.Text.Encoding.UTF8);
        }

        public static void Sale(Model.Cliente.Item clientItem, DateTime dateSale, decimal totalBrute, decimal discount, decimal totalValue, string payment, List<View.VendasDelivery.Item> products)
        {
            string content = "BIG BURGUER LANCHES";
            content += "\nRUA TOCANTINS, Nº 395, MORRO DAS BICAS";
            content += "\nTELEFONE: ESPERANDO IGOR";
            content += "\n--------------------------------------";
            content += "\nCUPOM NÃO FISCAL";
            content += "\n--------------------------------------";
            content += "\n" + "PRODUTOS";
            foreach (View.VendasDelivery.Item p in products)
                content += "\n" + p.Quantity + "x " + p.Name + "  " + p.Total.ToString("C2");

            content += "\n--------------------------------------";

            content += "\nTOTAL BRUTO: R$" + totalBrute.ToString("N2");
            content += "\nDESCONTO: R$" + discount.ToString("N2");
            content += "\nTOTAL: R$" + totalValue.ToString("N2");

            content += "\nFORMA DE PAGAMENTO: " + payment;
            content += "\nDATA DA EMISSÃO: " + dateSale;

            content += "\n--------------------------------------";

            content += "\nCLIENTE: " + clientItem.NAME;
            content += "\n" + clientItem.ADDRESS + ", Nº" + clientItem.NUMBER + ", " + clientItem.DISTRICT + ", " + clientItem.COMPLEMENT;

            File.WriteAllText(Path() + "\\sale.txt", content, System.Text.Encoding.UTF8);
        }
    }
}
