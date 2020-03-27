using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

            this.gridSales.PreviewMouseUp += GridSales_PreviewMouseUp;

            this.addLocal.Click += AddLocal_Click;
            this.addDelivery.Click += AddDelivery_Click;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        private void Vendas_Loaded(object sender, RoutedEventArgs e)
        {
            Sales.Balcao.Select(gridSales);
        }

        private void GridSales_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (gridSales.HasItems == false)
                return;

            if (gridSales.CurrentCell.Column.DisplayIndex == 5)
            {
                Item it = (Item)gridSales.SelectedItem;
                if (it.Type == 0)
                {
                    if (MessageBox.Show("Tem certeza que deseja excluir a venda?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        int numTable = Convert.ToInt32(it.File);
                        Sales.Balcao.Del(numTable);
                    }
                }
            }
            // Edit
            else if (gridSales.CurrentCell.Column.DisplayIndex == 6)
            {
                Item it = (Item)gridSales.SelectedItem;
                if (it.Type == 0)
                {
                    int numTable = Convert.ToInt32(it.File);
                    string[] info = Sales.Balcao.Info(numTable);

                    DateTime dateSale = Convert.ToDateTime(info[0]);
                    decimal totalSale = Convert.ToDecimal(info[1]);

                    VendasBalcao balcao = new VendasBalcao();
                    balcao.LoadEditing(numTable, totalSale, dateSale, Sales.Balcao.Products(numTable));
                    balcao.ShowDialog();
                }
            }
        }

        private void AddLocal_Click(object sender, RoutedEventArgs e)
        {
            VendasBalcao balcao = new VendasBalcao();
            balcao.ShowDialog();
        }

        private void AddDelivery_Click(object sender, RoutedEventArgs e)
        {
            VendasDelivery delivery = new VendasDelivery();
            delivery.ShowDialog();
        }
    }
}
