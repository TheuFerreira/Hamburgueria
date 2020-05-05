﻿using Hamburgueria.View;
using System.Windows;
using System.Windows.Input;
using System;

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

            /*ObservableCollection<Item> items = new ObservableCollection<Item>();
            items.Add(new Item(2, 1, "COCA COLA 1L", 6.5m, 1));
            for (int i = 0; i < 25; i++)
            {
                Sales.Balcao.Create(i + 1, DateTime.Today, 10, "", items);
            }*/

            InitializeComponent();

            //this.Loaded += MainWindow_Loaded;

            PreviewKeyDown += MainWindow_PreviewKeyDown;

            BtnVendas.Click += delegate { new Vendas().Show(); };
            BtnClientes.Click += delegate { new Clientes().Show(); };
            BtnProdutos.Click += delegate { new Produtos().Show(); };
            BtnRelatorios.Click += delegate { new Relatorios().Show(); };
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime maxDate = new DateTime(2000, 5, 10);

            if (DateTime.Today >= maxDate)
            {
                MessageBox.Show("Tempo de avalização expirado!!!");
                this.Close();
            }
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
                new Vendas().Show();
            else if (e.Key == Key.F3)
                new Clientes().Show();
            else if (e.Key == Key.F5)
                new Produtos().Show();
            else if (e.SystemKey == Key.F10)
                new Relatorios().Show();
        }
    }
}
