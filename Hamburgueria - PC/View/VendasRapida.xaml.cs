﻿using System;
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
    /// Interação lógica para VendasRapida.xam
    /// </summary>
    public partial class VendasRapida : Window
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

        private bool isNumber = false;
        private int searchId = -1;
        private int searchCod = 0;
        private string searchName = "";
        private decimal searchPrice = 0;

        private decimal totalSale = 0;

        public VendasRapida()
        {
            InitializeComponent();

            this.Loaded += VendasBalcao_Loaded;

            this.search.PreviewKeyDown += Search_PreviewKeyDown;
            this.search.PreviewTextInput += Search_PreviewTextInput;
            this.search.TextChanged += Search_TextChanged;

            this.gridSearch.PreviewKeyDown += GridSearch_PreviewKeyDown;
            this.gridSearch.MouseDoubleClick += GridSearch_MouseDoubleClick;

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
            gridSearch.Items.Clear();
            if (isNumber)
                Model.Produto.Select(gridSearch, Convert.ToInt32(text));
            else
                Model.Produto.Select(gridSearch, text);

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

                if (searchId == -1)
                {
                    MessageBox.Show("Selecione um produto!!!", "ERRO", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    gridProduct.Items.Add(new Item() { Id = searchId, Cod = searchCod, Name = searchName, Price = searchPrice, Quantity = q, Total = searchPrice * q });

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

                quantity.Text = "";
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
            if (gridProduct.HasItems == false)
            {
                MessageBox.Show("É necessário ter produtos a venda");
                return;
            }

            if (MessageBox.Show("Tem certeza que deseja finalizar a venda?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                List<Item> items = new List<Item>();
                for (int i = 0; i < gridProduct.Items.Count; i++)
                {
                    var row = (Item)gridProduct.Items[i];
                    items.Add(row);
                }

                VendasPagamento pagamento = new VendasPagamento(totalSale)
                {
                    items = items,
                    dateSale = DateTime.Now
                };
                pagamento.ShowDialog();

                if (pagamento.Confirmed)
                {
                    MessageBox.Show("Venda realizada com sucesso!!!");

                    gridProduct.Items.Clear();
                    search.Text = "";
                    quantity.Text = "";
                    totalSale = 0;
                    searchId = -1;
                    labelTotalSale.Content = "TOTAL:" + totalSale.ToString("C2");
                }
            }
        }
    }
}
