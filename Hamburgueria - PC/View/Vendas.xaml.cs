using System;
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

            this.back.Click += Back_Click;
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
        }

        private void GridSales_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void GridSales_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (gridSales.HasItems == false || gridSales.CurrentCell.IsValid == false)
                return;

            Item it = (Item)gridSales.SelectedItem;
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
                    decimal totalSale = Convert.ToDecimal(info[1]);
                    string observation = info[2];

                    VendasBalcao balcao = new VendasBalcao
                    {
                        sales = this
                    };
                    balcao.LoadEditing(numTable, totalSale, dateSale, observation, Sales.Balcao.Products(numTable));
                    balcao.ShowDialog();
                }
                else
                {
                    string fileName = it.File;
                    string[] info = Sales.Delivery.Info(fileName);

                    DateTime dateSale = Convert.ToDateTime(info[1]);
                    decimal totalSale = Convert.ToDecimal(info[2]);
                    string[] addressFile = info[3].Split('>');
                    Hamburgueria.Tables.Client address = new Hamburgueria.Tables.Client(info[0], addressFile[0], Convert.ToInt32(addressFile[1]), addressFile[2], addressFile[3], info[4], info[5]);

                    VendasDelivery delivery = new VendasDelivery(this);
                    delivery.LoadEditing(fileName, address, totalSale, info[6], info[7], dateSale, info[8], Sales.Delivery.Products(fileName));
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
                    decimal totalSale = Convert.ToDecimal(info[2]);
                    string[] addressFile = info[3].Split('>');
                    Hamburgueria.Tables.Client address = new Hamburgueria.Tables.Client(info[0], addressFile[0], Convert.ToInt32(addressFile[1]), addressFile[2], addressFile[3], info[4], info[5]);
                    string payment = info[6];
                    decimal discount = Convert.ToDecimal(info[7]);

                    ObservableCollection<Hamburgueria.Item> items = Sales.Delivery.Products(fileName);

                    TXT.Sale(address, dateSale, totalSale, discount, totalSale - discount, payment, info[8], items);
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
                    decimal totalSale = Convert.ToDecimal(info[1]);
                    string observation = info[2];

                    new VendasPagamento(totalSale)
                    {
                        typeSale = 1,
                        sales = this,
                        numTable = numTable,
                        dateSale = dateSale,
                        observation = observation
                    }.ShowDialog();
                }
                else
                {
                    string fileName = it.File;
                    string[] info = Sales.Delivery.Info(fileName);

                    if (MessageBox.Show("Tem certeza de que deseja FINALIZAR a venda do Cliente " + info[0] + "??", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        return;

                    DateTime dateSale = Convert.ToDateTime(info[1]);
                    decimal totalSale = Convert.ToDecimal(info[2]);
                    string[] addressFile = info[3].Split('>');
                    Hamburgueria.Tables.Client address = new Hamburgueria.Tables.Client(info[0], addressFile[0], Convert.ToInt32(addressFile[1]), addressFile[2], addressFile[3], info[4], info[5]);
                    string payment = info[6];
                    decimal discount = Convert.ToDecimal(info[7]);

                    ObservableCollection<Hamburgueria.Item>items = Sales.Delivery.Products(fileName);

                    new Sql.Sale().Insert(address, dateSale, totalSale - discount, discount, totalSale, payment, items);

                    if (MessageBox.Show("Deseja imprimir o CUPOM NÃO FISCAL??", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        TXT.Sale(address, dateSale, totalSale, discount, totalSale - discount, payment, info[8], items);
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
            VendasBalcao balcao = new VendasBalcao
            {
                sales = this
            };
            balcao.ShowDialog();
        }

        private void AddDelivery_Click(object sender, RoutedEventArgs e)
        {
            VendasDelivery delivery = new VendasDelivery(this);
            delivery.ShowDialog();
        }
    }
}
