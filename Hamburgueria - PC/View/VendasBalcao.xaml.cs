using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hamburgueria.View
{
    /// <summary>
    /// Interação lógica para VendasBalcao.xam
    /// </summary>
    public partial class VendasBalcao : Window
    {
        public Vendas sales;

        public ObservableCollection<Item> Items = new ObservableCollection<Item>();

        private bool isNumber = false;
        private Tables.Product product = null;

        private bool isEditing = false;
        private DateTime dateSale;
        private int oldNumTable;

        public VendasBalcao()
        {
            InitializeComponent();

            gridProduct.DataContext = Items;

            Closed += VendasBalcao_Closed;
            Loaded += VendasBalcao_Loaded;

            search.PreviewKeyDown += Search_PreviewKeyDown;
            search.PreviewTextInput += Search_PreviewTextInput;
            search.TextChanged += Search_TextChanged;

            gridSearch.PreviewKeyDown += GridSearch_PreviewKeyDown;
            gridSearch.MouseDoubleClick += GridSearch_MouseDoubleClick;

            quantity.PreviewKeyDown += Quantity_PreviewKeyDown;
            quantity.PreviewTextInput += (s, e) => e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);

            numTable.PreviewTextInput += (s, e) => e.Handled = new Regex("[^0-9]+").IsMatch(e.Text); ;

            gridProduct.BeginningEdit += (sender, e) => e.Cancel = true;
            gridProduct.PreviewKeyDown += GridProduct_PreviewKeyDown;

            confirm.Click += Confirm_Click;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void VendasBalcao_Closed(object sender, EventArgs e)
        {
            sales.UpdateGrid();
        }

        public void LoadEditing(int num, decimal totalSale, DateTime dateSale, string observation, ObservableCollection<Item> items)
        {
            isEditing = true;
            oldNumTable = num;
            numTable.Text = num.ToString();
            labelTotalSale.Content = "TOTAL:" + totalSale.ToString("C2");
            this.dateSale = dateSale;
            this.observation.Text = observation;

            Items = items;

            foreach(Item it in items)
                gridProduct.Items.Add(it);
        }

        private void VendasBalcao_Loaded(object sender, RoutedEventArgs e)
        {
            gridSearch.Visibility = Visibility.Hidden;
        }

        private void Search_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GridSearch_MouseDoubleClick(null, null);
            }
            else if (gridSearch.HasItems)
            {
                if (e.Key == Key.Down)
                {
                    int index = gridSearch.SelectedIndex + 1;
                    if (index == gridSearch.Items.Count)
                        index = 0;
                    gridSearch.SelectedIndex = index;
                }
                else if (e.Key == Key.Up)
                {
                    int index = gridSearch.SelectedIndex - 1;
                    if (index < 0)
                        index = gridSearch.Items.Count - 1;
                    gridSearch.SelectedIndex = index;
                }
            }
        }

        private void Search_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (isNumber)
                e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = search.Text;
            if(string.IsNullOrEmpty(text))
            {
                isNumber = false;
                gridSearch.Visibility = Visibility.Hidden;
                return;
            }

            gridSearch.Visibility = Visibility.Visible;
            isNumber = char.IsDigit(text[0]);
            gridSearch.Items.Clear();
            if (isNumber)
            {
                var products = new Hamburgueria.Sql.Product().Select(Convert.ToInt32(text));
                foreach (var p in products)
                    gridSearch.Items.Add(p);
            }
            else
            {
                var products = new Hamburgueria.Sql.Product().Select(text);
                foreach (var p in products)
                    gridSearch.Items.Add(p);
            }

            if (gridSearch.HasItems)
                gridSearch.SelectedItem = gridSearch.Items[0];
        }

        private void GridSearch_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                GridSearch_MouseDoubleClick(null, null);
        }

        private void GridSearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (gridSearch.HasItems == false)
                return;
            
            product = (Tables.Product)gridSearch.SelectedItem;
            gridSearch.Visibility = Visibility.Hidden;
            quantity.Focus();
        }

        private void Quantity_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int q = Convert.ToInt32(quantity.Text);
                if (q == 0)
                {
                    MessageBox.Show("Valor na quantidade precisa ser maior que 0!!!");
                    return;
                }

                if (product == null)
                {
                    MessageBox.Show("Selecione um produto!!!", "ERRO", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                bool exist = false;
                foreach (Item i in Items)
                {
                    if (i.Id == product.Id)
                    {
                        i.Quantity++;

                        exist = true;
                        break;
                    }
                }

                if (exist == false)
                    Items.Add(new Item(product.Id, product.Cod, product.Name, product.Price, q));

                labelTotalSale.Content = "TOTAL:" + TotalSale().ToString("C2");

                product = null;

                quantity.Text = "";
                search.Text = "";
                gridSearch.Visibility = Visibility.Hidden;

                search.Focus();
            }
        }

        private void GridProduct_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Items.Count == 0)
                return;

            int index = gridProduct.SelectedIndex;

            if (e.Key == Key.Add)
            {
                Items[index].Quantity++;
            }
            else if (e.Key == Key.Subtract)
            {
                Items[index].Quantity--;
                if (Items[index].Quantity == 0)
                    Items.RemoveAt(index);
            }
            else if (e.Key == Key.Delete)
            {
                Items.RemoveAt(index);
            }

            labelTotalSale.Content = "TOTAL:" + TotalSale().ToString("C2");
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(numTable.Text))
            {
                MessageBox.Show("É necessário informar o número da mesa");
                return;
            }
            else if (gridProduct.HasItems == false)
            {
                MessageBox.Show("É necessário ter produtos a venda");
                return;
            }

            if (isEditing == false)
            {
                int table = Convert.ToInt32(numTable.Text);
                if (Sales.Balcao.Exist(table))
                {
                    MessageBox.Show("Essa mesa já está sendo utilizada!!!");
                    return;
                }

                Sales.Balcao.Create(table, DateTime.Now, TotalSale(), observation.Text, Items);

                product = null;

                labelTotalSale.Content = "TOTAL:R$0,00";
                quantity.Text = "";
                numTable.Text = "";
                observation.Text = "";
                search.Focus();

                MessageBox.Show("Venda adicionada com sucesso!!!");

                sales.UpdateGrid();
            }
            else
            {
                int table = Convert.ToInt32(numTable.Text);
                if (oldNumTable != table)
                {
                    if (Sales.Balcao.Exist(table))
                    {
                        MessageBox.Show("Essa mesa já está sendo utilizada!!!");
                        return;
                    }
                }

                Sales.Balcao.Edit(oldNumTable, table, dateSale, TotalSale(), observation.Text, Items);

                MessageBox.Show("Venda alterada com sucesso!!!");

                this.Close();
            }
        }

        private decimal TotalSale()
        {
            decimal t = 0;
            foreach (Item i in Items)
                t += i.Total;

            return t;
        }
    }
}
