using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hamburgueria.Model;
using Hamburgueria.View;

namespace Hamburgueria
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            new Sql.Product().Select();

            InitializeComponent();

            this.PreviewKeyDown += MainWindow_PreviewKeyDown;

            BtnVendas.Click += delegate { new Vendas().Show(); };
            BtnClientes.Click += delegate { new View.Clientes().Show(); };
            BtnProdutos.Click += delegate { new Produtos().Show(); };
            BtnRelatorios.Click += delegate { new Relatorios().Show(); };
            //TestTXT();

            //Excel ex = new Excel();
            //ex.Sales();
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
                new Vendas().Show();
            else if (e.Key == Key.F2)
            {
            }
            else if (e.Key == Key.F3)
                new View.Clientes().Show();
            else if (e.Key == Key.F5)
                new Produtos().Show();
            else if (e.SystemKey == Key.F10)
                new Relatorios().Show();
        }
        /*
        void TestTXT()
        {
            Model.Cliente.Item clientItem = new Cliente.Item()
            {
                NAME = "UM NOME O QUAL NUNCA OUVIRAM FALAR EM TODAS AS SUAS VIDAS COMO PROGRAMADORES",
                ADDRESS = "AQUELA RUA QUE NUNCA FOI VISITADA",
                NUMBER = "555555",
                DISTRICT = "BAIRRO DISTANTE DE TODOS OS OUTROS",
                COMPLEMENT = "PERTO DA CASA DO KARALHO"
            };

            DateTime dateSale = DateTime.Today;
            decimal totalBrute = 28.5m;
            decimal discount = 0;
            decimal totalValue = 28.5m;
            string payment = "DÉBITO";
            List<View.VendasDelivery.Item> products = new List<VendasDelivery.Item>();
            products.Add(new VendasDelivery.Item()
            {
                Cod = 5,
                Name = "COCA COLA",
                Quantity = 1,
                Price = 8.5m,
                Total = 8.5m
            });
            products.Add(new VendasDelivery.Item()
            {
                Cod = 5,
                Name = "HAMBURGUER PARMEDIANO COM CALEBRASA E CHEDDAR",
                Quantity = 1,
                Price = 20m,
                Total = 20m
            });

            string content = "            BIG BURGUER";
            content += "\nRUA TOCANTINS, Nº95, MORRO DAS BICAS";
            content += "\nTELEFONE: 3543-0336";
            content += "\n(Whats) 99303-2638";
            content += "\n------------------------------------";
            content += "\n          CUPOM NÃO FISCAL";
            content += "\n------------------------------------";
            content += "\nPRODUTOS";
            foreach (View.VendasDelivery.Item p in products)
            {
                string nameProduct = p.Name;
                if (p.Name.Length >= 25)
                {
                    nameProduct = p.Name.Substring(0, 24);
                    nameProduct += "-";
                    content += "\n" + p.Quantity + "x " + nameProduct.PadRight(25);
                    p.Name = p.Name.Substring(24);

                    content += "\n   " + p.Name.PadRight(25) + p.Total.ToString("C2");
                }
                else
                {
                    content += "\n" + p.Quantity + "x " + p.Name.PadRight(25) + p.Total.ToString("C2");
                }
            }

            content += "\n--------------------------------------";

            content += "\nTOTAL BRUTO: R$" + totalBrute.ToString("N2");
            content += "\nDESCONTO: R$" + discount.ToString("N2");
            content += "\nTOTAL: R$" + totalValue.ToString("N2");

            content += "\nFORMA DE PAGAMENTO: " + payment;
            content += "\nDATA DA EMISSÃO: " + dateSale;
            content += "\n       Agradecemos a preferência";
            content += "\n             Volte Sempre!";

            content += "\n--------------------------------------";

            content += ReformText("CLIENTE: " + clientItem.NAME);
            content += ReformText("RUA: " + clientItem.ADDRESS);
            content += ReformText("NÚMERO: " + clientItem.NUMBER);
            content += ReformText("BAIRRO: " + clientItem.DISTRICT);
            content += ReformText("COMPLEMENTO: " + clientItem.COMPLEMENT);
            
            System.IO.File.WriteAllText("sale.txt", content, System.Text.Encoding.UTF8);
        }

        private string ReformText(string text)
        {
            string[] words = text.Split(' ');

            string lines = "";
            string line = "\n";


            for (int i = 0; i < words.Length; i ++)
            {
                if ((line + words[i]).Length <= 38)
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
        }*/

    }
}
