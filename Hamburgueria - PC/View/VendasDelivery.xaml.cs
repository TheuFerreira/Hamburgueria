﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private bool isEditing = false;
        private Model.Cliente.Item oldAddress;
        private DateTime dateSale;

        public VendasDelivery(Vendas sales)
        {
            InitializeComponent();

            this.Closed += (sender, e) => sales.UpdateGrid();
            this.Loaded += VendasDelivery_Loaded;

            // CLIENT

            this.searchName.LostFocus += (sender, e) => gridClient.Visibility = Visibility.Hidden;
            this.searchName.PreviewKeyDown += SearchName_PreviewKeyDown;
            this.searchName.TextChanged += SearchName_TextChanged;

            this.number.PreviewTextInput += (sender, e) => e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);

            this.discount.LostFocus += Discount_LostFocus;
            this.discount.PreviewTextInput += (sender, e) => e.Handled = new Regex("[^0-9,]+").IsMatch(e.Text);

            // Product

            this.searchProduct.PreviewKeyDown += SearchProduct_PreviewKeyDown;
            this.searchProduct.PreviewTextInput += SearchProduct_PreviewTextInput;
            this.searchProduct.TextChanged += SearchProduct_TextChanged;

            this.gridSearch.MouseDoubleClick += GridSearch_MouseDoubleClick;

            this.quantity.PreviewKeyDown += Quantity_PreviewKeyDown;
            this.quantity.PreviewTextInput += (sender, e) => e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);

            this.gridProduct.BeginningEdit += (sender, e) => e.Cancel = true;
            this.gridProduct.PreviewKeyDown += GridProduct_PreviewKeyDown;

            // BUTTONS

            this.confirm.Click += Confirm_Click;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        public void LoadEditing(Model.Cliente.Item oldAddress, decimal totalSale, string payment, string discount, DateTime dateSale, List<Item> items)
        {
            isEditing = true;
            labelTotalSale.Content = "TOTAL:" + totalSale.ToString("C2");
            this.totalSale = totalSale;
            this.dateSale = dateSale;
            this.oldAddress = oldAddress;

            searchName.Text = oldAddress.NAME;
            number.Text = oldAddress.NUMBER;
            street.Text = oldAddress.ADDRESS;
            district.Text = oldAddress.DISTRICT;
            complement.Text = oldAddress.COMPLEMENT;

            switch (payment)
            {
                case "Á VISTA":
                    this.payment.SelectedIndex = 0;
                    break;
                case "CRÉDITO":
                    this.payment.SelectedIndex = 1;
                    break;
                case "DÉBITO":
                    this.payment.SelectedIndex = 2;
                    break;
            }

            this.discount.Text = discount;

            foreach (Item it in items)
                gridProduct.Items.Add(it);
        }

        private void VendasDelivery_Loaded(object sender, RoutedEventArgs e)
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

        private void SearchName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (gridClient.HasItems)
                {
                    var selected = (Model.Cliente.Item)gridClient.SelectedItem;

                    searchName.Text = selected.NAME;
                    number.Text = selected.NUMBER;
                    street.Text = selected.ADDRESS;
                    district.Text = selected.DISTRICT;
                    complement.Text = selected.COMPLEMENT;

                    payment.Focus();
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

        private void SearchName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = searchName.Text;
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

        private void Discount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(discount.Text, out decimal d))
                discount.Text = d.ToString("N2");
            else
                discount.Text = "0,00";

            totalSale -= Convert.ToDecimal(discount.Text);
            labelTotalSale.Content = "TOTAL:" + totalSale.ToString("C2");
        }

        // PRODUCT
        private bool isNumber = false;
        private int searchId = -1;
        private int searchCod = 0;
        private string searchNames = "";
        private decimal searchPrice = 0;

        private decimal totalSale = 0;

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

        private void SearchProduct_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GridSearch_MouseDoubleClick(null, null);
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

        private void SearchProduct_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (isNumber)
                e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void SearchProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = searchProduct.Text;
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

        private void GridSearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (gridSearch.HasItems)
            {
                var selected = (Model.Produto.Item)gridSearch.SelectedItem;
                searchId = selected.ID;
                searchCod = selected.COD;
                searchNames = selected.NAME;
                searchProduct.Text = selected.NAME;
                searchPrice = selected.PRICE;
            }

            gridSearch.Visibility = Visibility.Hidden;
            quantity.Focus();
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
                    gridProduct.Items.Add(new Item() { Id = searchId, Cod = searchCod, Name = searchNames, Price = searchPrice, Quantity = q, Total = searchPrice * q });

                totalSale = 0;
                for (int i = 0; i < gridProduct.Items.Count; i++)
                {
                    var row = (Item)gridProduct.Items[i];
                    totalSale += row.Total;
                }
                totalSale -= Convert.ToDecimal(discount.Text);
                labelTotalSale.Content = "TOTAL:" + totalSale.ToString("C2");

                searchId = -1;
                searchCod = 0;
                searchNames = "";
                searchPrice = 0;

                quantity.Text = "";
                searchProduct.Text = "";
                gridSearch.Visibility = Visibility.Hidden;

                searchProduct.Focus();
            }
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
                totalSale -= Convert.ToDecimal(discount.Text);
                labelTotalSale.Content = "TOTAL:" + totalSale.ToString("C2");
            }
        }

        // BUTTONS

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchName.Text) ||
                string.IsNullOrEmpty(street.Text) ||
                string.IsNullOrEmpty(number.Text) ||
                string.IsNullOrEmpty(district.Text) ||
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
                NAME = searchName.Text,
                ADDRESS = street.Text,
                COMPLEMENT = complement.Text,
                DISTRICT = district.Text,
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
                Sales.Delivery.Create(address, DateTime.Now, totalSale, payment.Text, Convert.ToDecimal(discount.Text), items);

                searchId = -1;
                searchCod = 0;
                searchNames = "";
                searchPrice = 0;
                totalSale = 0;

                searchName.Text = "";
                street.Text = "";
                complement.Text = "";
                district.Text = "";
                number.Text = "";
                discount.Text = "0,00";

                gridProduct.Items.Clear();
                labelTotalSale.Content = "TOTAL:R$0,00";
                quantity.Text = "";
                searchName.Focus();

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
                Sales.Delivery.Edit(oldAddress.NAME, address, dateSale, totalSale, payment.Text, Convert.ToDecimal(discount.Text), items);

                MessageBox.Show("Venda alterada com sucesso!!!");

                this.Close();
            }
        }
    }
}
