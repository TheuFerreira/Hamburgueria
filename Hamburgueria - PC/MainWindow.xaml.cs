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
            InitializeComponent();

            this.PreviewKeyDown += MainWindow_PreviewKeyDown;

            BtnVendas.Click += delegate { new Vendas().Show(); };
            BtnClientes.Click += delegate { new Clientes().Show(); };
            BtnProdutos.Click += delegate { new Produtos().Show(); };
            BtnRelatorios.Click += delegate { new Relatorios().Show(); };
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
                new Vendas().Show();
            else if (e.Key == Key.F2)
            {
            }
            else if (e.Key == Key.F3)
                new Clientes().Show();
            else if (e.Key == Key.F5)
                new Produtos().Show();
            else if (e.SystemKey == Key.F10)
                new Relatorios().Show();
        }
    }
}
