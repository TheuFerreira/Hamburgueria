﻿using Hamburgueria.View;
using System.Windows;
using System.Windows.Input;
using System;
using System.Collections.ObjectModel;

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

            this.Loaded += MainWindow_Loaded;

            PreviewKeyDown += MainWindow_PreviewKeyDown;

            BtnVendas.Click += delegate { new Vendas().Show(); };
            BtnClientes.Click += delegate { new Clientes().Show(); };
            BtnProdutos.Click += delegate { new Produtos().Show(); };
            BtnRelatorios.Click += delegate { new Relatorios().Show(); };
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime maxDate = new DateTime(2020, 5, 10);

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
