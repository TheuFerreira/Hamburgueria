﻿using System;
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
        public class Item
        {
            public int Type { get; set; }
            public string Value { get; set; }
            public string File { get; set; }
            public string Info { get; set; }
            public decimal Total { get; set; }
            public DateTime Date { get; set; }
            public string Remove { get; } = "/Hamburgueria;component/Resources/remove.png";
            public string Modify { get; } = "/Hamburgueria;component/Resources/modify.png";
            public string Print { get; } = "/Hamburgueria;component/Resources/print.png";
            public string Confirm { get; } = "/Hamburgueria;component/Resources/confirm.png";
        }

        public Vendas()
        {
            InitializeComponent();

            this.Loaded += Vendas_Loaded;

            this.gridSales.BeginningEdit += GridSales_BeginningEdit;
            this.gridSales.PreviewMouseUp += GridSales_PreviewMouseUp;

            this.addFast.Click += AddFast_Click;
            this.addLocal.Click += AddLocal_Click;
            this.addDelivery.Click += AddDelivery_Click;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        public void UpdateGrid()
        {
            gridSales.Items.Clear();
            Sales.Balcao.Select(gridSales);
            Sales.Delivery.Select(gridSales);
        }

        private void Vendas_Loaded(object sender, RoutedEventArgs e)
        {
            gridSales.Items.Clear();
            Sales.Balcao.Select(gridSales);
            Sales.Delivery.Select(gridSales);

            Style s = new Style(typeof(DataGridColumnHeader));
            s.Setters.Add(new Setter(DataGridRow.BackgroundProperty, (SolidColorBrush)FindResource("AzulBruxao")));
            s.Setters.Add(new Setter(DataGridRow.ForegroundProperty, Brushes.White));
            for (int i = 0; i < gridSales.Columns.Count; i++)
                gridSales.Columns[i].HeaderStyle = s;
        }

        private void GridSales_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void GridSales_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (gridSales.HasItems == false || gridSales.CurrentCell.IsValid == false)
                return;

            // Delete
            if (gridSales.CurrentCell.Column.DisplayIndex == 6)
            {
                Item it = (Item)gridSales.SelectedItem;
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
                Item it = (Item)gridSales.SelectedItem;
                if (it.Type == 0)
                {
                    int numTable = Convert.ToInt32(it.File);
                    string[] info = Sales.Balcao.Info(numTable);

                    DateTime dateSale = Convert.ToDateTime(info[0]);
                    decimal totalSale = Convert.ToDecimal(info[1]);

                    VendasBalcao balcao = new VendasBalcao();
                    balcao.sales = this;
                    balcao.LoadEditing(numTable, totalSale, dateSale, Sales.Balcao.Products(numTable));
                    balcao.ShowDialog();
                }
                else
                {
                    string nameClient = it.File;
                    string[] info = Sales.Delivery.Info(nameClient);

                    DateTime dateSale = Convert.ToDateTime(info[0]);
                    decimal totalSale = Convert.ToDecimal(info[1]);
                    string[] addressFile = info[2].Split('>');
                    Model.Cliente.Item address = new Model.Cliente.Item()
                    {
                        NAME = nameClient,
                        ADDRESS = addressFile[0],
                        NUMBER = addressFile[1],
                        COMPLEMENT = addressFile[2]
                    };

                    VendasDelivery delivery = new VendasDelivery();
                    delivery.sales = this;
                    delivery.LoadEditing(address, totalSale, dateSale, Sales.Delivery.Products(nameClient));
                    delivery.ShowDialog();
                }
            }
            // Confirm
            else if (gridSales.CurrentCell.Column.DisplayIndex == 9)
            {
                Item it = (Item)gridSales.SelectedItem;
                if (it.Type == 0)
                {
                    int numTable = Convert.ToInt32(it.File);
                    string[] info = Sales.Balcao.Info(numTable);
                    DateTime dateSale = Convert.ToDateTime(info[0]);
                    decimal totalSale = Convert.ToDecimal(info[1]);

                    VendasPagamento pagamento = new VendasPagamento(totalSale);
                    pagamento.typeSale = 1;
                    pagamento.sales = this;
                    pagamento.numTable = numTable;
                    pagamento.dateSale = dateSale;
                    pagamento.ShowDialog();
                }
            }

            UpdateGrid();
        }

        private void AddFast_Click(object sender, RoutedEventArgs e)
        {
            VendasRapida rapida = new VendasRapida();
            rapida.ShowDialog();
        }

        private void AddLocal_Click(object sender, RoutedEventArgs e)
        {
            VendasBalcao balcao = new VendasBalcao();
            balcao.sales = this;
            balcao.ShowDialog();
        }

        private void AddDelivery_Click(object sender, RoutedEventArgs e)
        {
            VendasDelivery delivery = new VendasDelivery();
            delivery.sales = this;
            delivery.ShowDialog();
        }
    }
}
