﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Hamburgueria.View
{
    /// <summary>
    /// Lógica interna para Vendas.xaml
    /// </summary>
    public partial class Vendas : Window
    {
        public ObservableCollection<Sale> sales = new ObservableCollection<Sale>();

        public Vendas()
        {
            InitializeComponent();

            gridSales.DataContext = sales;

            this.Loaded += Vendas_Loaded;

            this.gridSales.BeginningEdit += GridSales_BeginningEdit;
            this.gridSales.PreviewMouseUp += GridSales_PreviewMouseUp;

            this.back.Click += Back_Click;
            filter.SelectionChanged += Filter_SelectionChanged;
            this.addFast.Click += AddFast_Click;
            this.addLocal.Click += AddLocal_Click;
            this.addDelivery.Click += AddDelivery_Click;

            this.menuFast.Click += AddFast_Click;
            this.menuLocal.Click += AddLocal_Click;
            this.menuDelivery.Click += AddDelivery_Click;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        public void UpdateGrid()
        {
            Filter_SelectionChanged(null, null);
        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sales.Clear();
            if (filter.SelectedIndex == 0)
            {
                Sales.Balcao.Select(sales);
                Sales.Delivery.Select(sales);
            }
            else if (filter.SelectedIndex == 1)
            {
                Sales.Delivery.Select(sales);
            }
            else
            {
                Sales.Balcao.Select(sales);
            }
            sales.Rearrange();
        }

        private void Vendas_Loaded(object sender, RoutedEventArgs e)
        {
            sales.Clear();
            Sales.Balcao.Select(sales);
            Sales.Delivery.Select(sales);
            sales.Rearrange();
        }

        private void GridSales_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void GridSales_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (gridSales.HasItems == false || gridSales.CurrentCell.IsValid == false)
                return;

            View.Sale it = (View.Sale)gridSales.SelectedItem;
            if (it == null)
                return;

            // Delete
            if (gridSales.CurrentCell.Column.DisplayIndex == 6)
            {
                if (it.Type == 0)
                {
                    if (MessageBox.Show("Tem certeza que deseja excluir a venda?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        int numTable = Convert.ToInt32(it.File);
                        Sales.Balcao.Delete(numTable);
                    }
                }
                else
                {
                    if (MessageBox.Show("Tem certeza que deseja excluir a venda?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Sales.Delivery.Delete(it.File);
                    }
                }
            }
            // Edit
            else if (gridSales.CurrentCell.Column.DisplayIndex == 7)
            {
                if (it.Type == 0)
                {
                    int numTable = Convert.ToInt32(it.File);
                    string[] info = Sales.Balcao.Info(numTable);

                    DateTime dateSale = Convert.ToDateTime(info[0]);

                    VendasBalcao balcao = new VendasBalcao(this, Sales.Balcao.Products(numTable), dateSale, true, numTable);
                    balcao.ShowDialog();
                }
                else
                {
                    string fileName = it.File;
                    string[] info = Sales.Delivery.Info(fileName);

                    DateTime dateSale = Convert.ToDateTime(info[1]);
                    string[] addressFile = info[2].Split('>');
                    Tables.Client address = new Tables.Client(info[0], addressFile[0], Convert.ToInt32(addressFile[1]), addressFile[2], addressFile[3], info[3], info[4]);

                    VendasDelivery delivery = new VendasDelivery(this, Sales.Delivery.Products(fileName), address, dateSale, true, fileName, info[5], info[6]);
                    delivery.ShowDialog();
                }
            }
            // Print
            else if (gridSales.CurrentCell.Column.DisplayIndex == 8)
            {
                if (it.Type == 0)
                {
                    MessageBox.Show("Somente vendas Deliveries podem ser impressas antes de finalizadas!!!");
                    return;
                }
                else
                {
                    string fileName = it.File;
                    string[] info = Sales.Delivery.Info(fileName);

                    DateTime dateSale = Convert.ToDateTime(info[1]);
                    string[] addressFile = info[2].Split('>');
                    Tables.Client address = new Tables.Client(info[0], addressFile[0], Convert.ToInt32(addressFile[1]), addressFile[2], addressFile[3], info[3], info[4]);
                    string payment = info[5];
                    decimal discount = Convert.ToDecimal(info[6]);

                    ObservableCollection<Hamburgueria.Item> items = Sales.Delivery.Products(fileName);

                    TXT.Sale(address, dateSale, it.Total, discount, it.Total - discount, payment, items);
                    new Impressao().ShowDialog();
                }
            }
            // Confirm
            else if (gridSales.CurrentCell.Column.DisplayIndex == 9)
            {
                if (it.Type == 0)
                {
                    int numTable = Convert.ToInt32(it.File);
                    string[] info = Sales.Balcao.Info(numTable);
                    DateTime dateSale = Convert.ToDateTime(info[0]);

                    new VendasPagamento(it.Total)
                    {
                        typeSale = 1,
                        sales = this,
                        numTable = numTable,
                        dateSale = dateSale
                    }.ShowDialog();
                }
                else
                {
                    string fileName = it.File;
                    string[] info = Sales.Delivery.Info(fileName);

                    if (MessageBox.Show("Tem certeza de que deseja FINALIZAR a venda do Cliente " + info[0] + "??", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        return;

                    DateTime dateSale = Convert.ToDateTime(info[1]);
                    string[] addressFile = info[2].Split('>');
                    Tables.Client address = new Tables.Client(info[0], addressFile[0], Convert.ToInt32(addressFile[1]), addressFile[2], addressFile[3], info[3], info[4]);
                    string payment = info[5];
                    decimal discount = Convert.ToDecimal(info[6]);

                    ObservableCollection<Hamburgueria.Item>items = Sales.Delivery.Products(fileName);

                    new Sql.Sale().Insert(address, dateSale, it.Total - discount, discount, it.Total, payment, items);

                    if (MessageBox.Show("Deseja imprimir o CUPOM NÃO FISCAL??", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        TXT.Sale(address, dateSale, it.Total, discount, it.Total- discount, payment, items);
                        new Impressao().ShowDialog();
                    }

                    Sales.Delivery.Delete(fileName);
                }
            }

            UpdateGrid();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddFast_Click(object sender, RoutedEventArgs e)
        {
            VendasRapida rapida = new VendasRapida();
            rapida.ShowDialog();
        }

        private void AddLocal_Click(object sender, RoutedEventArgs e)
        {
            VendasBalcao balcao = new VendasBalcao(this, new ObservableCollection<Hamburgueria.Item>(), DateTime.Today);
            balcao.ShowDialog();
        }

        private void AddDelivery_Click(object sender, RoutedEventArgs e)
        {
            VendasDelivery delivery = new VendasDelivery(this, new ObservableCollection<Hamburgueria.Item>(), new Tables.Client(), DateTime.Today);
            delivery.ShowDialog();
        }

        private void filter_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
