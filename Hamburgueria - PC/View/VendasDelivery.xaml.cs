using System;
using System.Collections.Generic;
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
    /// Interação lógica para VendasDelivery.xam
    /// </summary>
    public partial class VendasDelivery : Window
    {
        public class Item
        {
            public int Id { get; set; }
            public int Cod { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public decimal Total { get; set; }
        }

        public Vendas sales;

        private bool isNumber = false;
        private int searchId = -1;
        private int searchCod = 0;
        private string searchName = "";
        private decimal searchPrice = 0;

        private bool isEditing = false;
        private Model.Cliente.Item oldAddress;
        private decimal totalSale = 0;
        private DateTime dateSale;

        public VendasDelivery()
        {
            InitializeComponent();

            this.Closed += VendasDelivery_Closed;
            this.Loaded += VendasBalcao_Loaded;

            this.nameClient.LostFocus += NameClient_LostFocus;
            this.nameClient.PreviewKeyDown += NameClient_PreviewKeyDown;
            this.nameClient.TextChanged += NameClient_TextChanged;

            this.number.PreviewTextInput += Number_PreviewTextInput;

            // PRODUCTS

            this.search.PreviewKeyDown += Search_PreviewKeyDown;
            this.search.PreviewTextInput += Search_PreviewTextInput;
            this.search.TextChanged += Search_TextChanged;

            this.quantity.PreviewKeyDown += Quantity_PreviewKeyDown;
            this.quantity.PreviewTextInput += Quantity_PreviewTextInput;

            this.gridProduct.BeginningEdit += (sender, e) => e.Cancel = true;
            this.gridProduct.PreviewKeyDown += GridProduct_PreviewKeyDown;

            this.confirm.Click += Confirm_Click;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        private void VendasDelivery_Closed(object sender, EventArgs e)
        {
            sales.UpdateGrid();
        }

        public void LoadEditing(Model.Cliente.Item oldAddress, decimal totalSale, DateTime dateSale, List<Item> items)
        {
            isEditing = true;
            labelTotalSale.Content = "TOTAL:" + totalSale.ToString("C2");
            this.totalSale = totalSale;
            this.dateSale = dateSale;
            this.oldAddress = oldAddress;

            nameClient.Text = oldAddress.NAME;
            number.Text = oldAddress.NUMBER;
            street.Text = oldAddress.ADDRESS;
            complement.Text = oldAddress.COMPLEMENT;

            foreach (Item it in items)
                gridProduct.Items.Add(it);
        }

        private void VendasBalcao_Loaded(object sender, RoutedEventArgs e)
        {
            gridSearch.Visibility = Visibility.Hidden;
            gridClient.Visibility = Visibility.Hidden;
        }

        // CLIENT

        public void UpdateClientSearch()
        {
            if (gridClient.Visibility == Visibility)
            {
                gridClient.Columns[0].Visibility = Visibility.Hidden;
                gridClient.Columns[2].Visibility = Visibility.Hidden;
                gridClient.Columns[3].Visibility = Visibility.Hidden;
                gridClient.Columns[4].Visibility = Visibility.Hidden;
                gridClient.Columns[5].Visibility = Visibility.Hidden;
                gridClient.Columns[6].Visibility = Visibility.Hidden;

                gridClient.Columns[1].Header = "NOME";

                gridClient.Columns[1].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void NameClient_LostFocus(object sender, RoutedEventArgs e)
        {
            gridClient.Visibility = Visibility.Hidden;
        }

        private void NameClient_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (gridClient.HasItems)
                {
                    var selected = (Model.Cliente.Item)gridClient.SelectedItem;

                    nameClient.Text = selected.NAME;
                    number.Text = selected.NUMBER;
                    street.Text = selected.ADDRESS;
                    complement.Text = selected.COMPLEMENT;
                }

                gridClient.Visibility = Visibility.Hidden;
            }
            else if (e.Key == Key.Down)
            {
                if (gridClient.HasItems)
                {
                    int index = gridClient.SelectedIndex;
                    index++;
                    if (index == gridClient.Items.Count)
                        index = 0;
                    gridClient.SelectedIndex = index;
                }
            }
            else if (e.Key == Key.Up)
            {
                if (gridClient.HasItems)
                {
                    int index = gridClient.SelectedIndex;
                    index--;
                    if (index < 0)
                        index = gridClient.Items.Count - 1;
                    gridClient.SelectedIndex = index;
                }
            }
        }

        private void NameClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = nameClient.Text;
            if (string.IsNullOrEmpty(text))
            {
                gridClient.Visibility = Visibility.Hidden;
                return;
            }

            gridClient.Visibility = Visibility.Visible;
            gridClient.ItemsSource = Model.Cliente.Select(text);
            if (gridClient.HasItems)
                gridClient.SelectedItem = gridClient.Items[0];
            else
                gridClient.Visibility = Visibility.Hidden;

            UpdateClientSearch();
        }

        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        // PRODUCTS

        public void UpdateProductSearch()
        {
            gridSearch.Columns[0].Visibility = Visibility.Hidden;

            gridSearch.Columns[1].Header = "CÓDIGO";
            gridSearch.Columns[2].Header = "NOME";
            gridSearch.Columns[3].Header = "PREÇO";

            gridSearch.Columns[1].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            gridSearch.Columns[2].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            gridSearch.Columns[3].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);

            gridSearch.Columns[3].ClipboardContentBinding.StringFormat = "C2";
        }

        private void Search_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (gridSearch.HasItems)
                {
                    var selected = (Model.Produto.Item)gridSearch.SelectedItem;
                    searchId = selected.ID;
                    searchCod = selected.COD;
                    searchName = selected.NAME;
                    search.Text = selected.NAME;
                    searchPrice = selected.PRICE;
                }

                gridSearch.Visibility = Visibility.Hidden;
                quantity.Focus();
                quantity.SelectAll();
            }
            else if (e.Key == Key.Down)
            {
                if (gridSearch.HasItems)
                {
                    int index = gridSearch.SelectedIndex;
                    index++;
                    if (index == gridSearch.Items.Count)
                        index = 0;
                    gridSearch.SelectedIndex = index;
                }
            }
            else if (e.Key == Key.Up)
            {
                if (gridSearch.HasItems)
                {
                    int index = gridSearch.SelectedIndex;
                    index--;
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
            if (string.IsNullOrEmpty(text))
            {
                isNumber = false;
                gridSearch.Visibility = Visibility.Hidden;
                return;
            }

            gridSearch.Visibility = Visibility.Visible;
            isNumber = char.IsDigit(text[0]);
            if (isNumber)
                gridSearch.ItemsSource = Model.Produto.Select(Convert.ToInt32(text));
            else
                gridSearch.ItemsSource = Model.Produto.Select(text);

            if (gridSearch.HasItems)
                gridSearch.SelectedItem = gridSearch.Items[0];

            UpdateProductSearch();
        }

        private void Quantity_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int q;
                try
                {
                    q = Convert.ToInt32(quantity.Text);

                    if (q == 0)
                        return;
                }
                catch
                {
                    MessageBox.Show("Valor na quantidade inválido!!!");
                    return;
                }

                bool exist = false;
                for (int i = 0; i < gridProduct.Items.Count; i++)
                {
                    var row = (Item)gridProduct.Items[i];
                    if (row.Id == searchId)
                    {
                        row.Quantity += q;
                        row.Total = row.Price * row.Quantity;
                        gridProduct.Items.RemoveAt(i);
                        gridProduct.Items.Insert(i, row);

                        exist = true;
                        break;
                    }
                }

                if (exist == false)
                {
                    gridProduct.Items.Add(new Item() { Id = searchId, Cod = searchCod, Name = searchName, Price = searchPrice, Quantity = q, Total = searchPrice * q });

                }

                totalSale = 0;
                for (int i = 0; i < gridProduct.Items.Count; i++)
                {
                    var row = (Item)gridProduct.Items[i];
                    totalSale += row.Total;
                }
                labelTotalSale.Content = "TOTAL:" + totalSale.ToString("C2");

                searchId = -1;
                searchCod = 0;
                searchName = "";
                searchPrice = 0;

                quantity.Text = "1";
                search.Text = "";
                gridSearch.Visibility = Visibility.Hidden;

                search.Focus();
            }
        }

        private void Quantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void GridProduct_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (gridProduct.HasItems)
            {
                int index = gridProduct.SelectedIndex;
                var it = (Item)gridProduct.SelectedItem;

                if (e.Key == Key.Add)
                {
                    it.Quantity++;
                    it.Total = it.Quantity * it.Price;

                    gridProduct.Items.RemoveAt(index);
                    gridProduct.Items.Insert(index, it);
                }
                else if (e.Key == Key.Subtract)
                {
                    it.Quantity--;
                    if (it.Quantity == 0)
                    {
                        gridProduct.Items.RemoveAt(index);

                        if (gridProduct.HasItems)
                            gridProduct.SelectedIndex = 0;
                    }
                    else
                    {
                        it.Total = it.Quantity * it.Price;

                        gridProduct.Items.RemoveAt(index);
                        gridProduct.Items.Insert(index, it);
                    }
                }
                else if (e.Key == Key.Delete)
                {
                    gridProduct.Items.RemoveAt(index);

                    if (gridProduct.HasItems)
                        gridProduct.SelectedIndex = 0;
                }

                totalSale = 0;
                for (int i = 0; i < gridProduct.Items.Count; i++)
                {
                    var row = (Item)gridProduct.Items[i];
                    totalSale += row.Total;
                }
                labelTotalSale.Content = "TOTAL:" + totalSale.ToString("C2");
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(nameClient.Text) ||
                string.IsNullOrEmpty(street.Text) ||
                string.IsNullOrEmpty(number.Text) ||
                string.IsNullOrEmpty(complement.Text))
            {
                MessageBox.Show("Por favor preencha todas as informações");
                return;
            }
            else if (gridProduct.HasItems == false)
            {
                MessageBox.Show("É necessário ter produtos a venda");
                return;
            }

            Model.Cliente.Item address = new Model.Cliente.Item()
            {
                NAME = nameClient.Text,
                ADDRESS = street.Text,
                COMPLEMENT = complement.Text,
                NUMBER = number.Text
            };
            
            if (isEditing == false)
            {
                if (Sales.Delivery.Exist(address.NAME))
                {
                    MessageBox.Show("Um cliente com este nome, já fez um pedido");
                    return;
                }

                List<Item> items = new List<Item>();
                foreach (Item i in gridProduct.Items)
                    items.Add(i);
                Sales.Delivery.Create(address, DateTime.Now, totalSale, items);

                searchId = -1;
                searchCod = 0;
                searchName = "";
                searchPrice = 0;
                totalSale = 0;

                nameClient.Text = "";
                street.Text = "";
                complement.Text = "";
                number.Text = "";

                gridProduct.Items.Clear();
                labelTotalSale.Content = "TOTAL:R$0,00";
                quantity.Text = "0";
                search.Focus();

                MessageBox.Show("Venda adicionada com sucesso!!!");
            }
            else
            {
                if (oldAddress.NAME != address.NAME)
                {
                    if (Sales.Delivery.Exist(address.NAME))
                    {
                        MessageBox.Show("Um cliente com este nome, já fez um pedido");
                        return;
                    }
                }

                List<Item> items = new List<Item>();
                foreach (Item i in gridProduct.Items)
                    items.Add(i);
                Sales.Delivery.Edit(oldAddress.NAME, address, dateSale, totalSale, items);

                MessageBox.Show("Venda alterada com sucesso!!!");

                this.Close();
            }
        }
    }
}
