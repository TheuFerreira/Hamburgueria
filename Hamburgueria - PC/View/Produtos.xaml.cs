using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace Hamburgueria.View
{
    /// <summary>
    /// Lógica interna para Produtos.xaml
    /// </summary>
    public partial class Produtos : Window
    {
        private bool isNumber = false;

        public Produtos()
        {
            InitializeComponent();

            this.Loaded += Produtos_Loaded;

            this.GridProdutos.BeginningEdit += (sender, e) => e.Cancel = true;

            this.Search.GotFocus += Search_GotFocus;
            this.Search.PreviewKeyDown += Search_PreviewKeyDown;
            this.Search.PreviewTextInput += Search_PreviewTextInput;
            this.Search.TextChanged += Search_TextChanged;

            this.BackProduto.Click += BackProduto_Click;
            this.DelProduto.Click += DelProduto_Click;
            this.EditProduto.Click += EditProduto_Click;
            this.AddProduto.Click += AddProduto_Click;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        public void Produtos_Loaded(object sender, RoutedEventArgs e)
        {
            GridProdutos.Items.Clear();
            var products = new Hamburgueria.Sql.Product().Select();
            foreach (var p in products)
                GridProdutos.Items.Add(p);
        }

        private void Search_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Search.Text == "Pesquisar")
                Search.Text = "";
        }

        private void Search_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                if (GridProdutos.HasItems)
                {
                    int index = GridProdutos.SelectedIndex;
                    index++;
                    if (index == GridProdutos.Items.Count)
                        index = 0;
                    GridProdutos.SelectedIndex = index;
                }
            }
            else if (e.Key == Key.Up)
            {
                if (GridProdutos.HasItems)
                {
                    int index = GridProdutos.SelectedIndex;
                    index--;
                    if (index < 0)
                        index = GridProdutos.Items.Count - 1;
                    GridProdutos.SelectedIndex = index;
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
            string text = Search.Text;
            GridProdutos.Items.Clear();
            if (string.IsNullOrEmpty(text))
            {
                isNumber = false;
                var products = new Hamburgueria.Sql.Product().Select();
                foreach (var p in products)
                    GridProdutos.Items.Add(p);
                return;
            }

            GridProdutos.Visibility = Visibility.Visible;
            isNumber = char.IsDigit(text[0]);
            if (isNumber)
            {
                var products = new Hamburgueria.Sql.Product().Select(Convert.ToInt32(text));
                foreach (var p in products)
                    GridProdutos.Items.Add(p);
            }
            else
            {
                var products = new Hamburgueria.Sql.Product().Select(text);
                foreach (var p in products)
                    GridProdutos.Items.Add(p);
            }

            if (GridProdutos.HasItems)
                GridProdutos.SelectedItem = GridProdutos.Items[0];
        }

        private void BackProduto_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DelProduto_Click(object sender, RoutedEventArgs e)
        {
            if (GridProdutos.SelectedIndex != -1)
            {
                int id = ((Hamburgueria.Tables.Product)GridProdutos.SelectedItem).Id;
                new Hamburgueria.Sql.Product().Delete(id);

                Produtos_Loaded(null, null);
            }
            else
            {
                MessageBox.Show("Selecione um PRODUTO para ser editado!", "ERRO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void EditProduto_Click(object sender, RoutedEventArgs e)
        {
            if (GridProdutos.SelectedIndex != -1)
            {
                var item = (Hamburgueria.Tables.Product)GridProdutos.SelectedItem;

                ProdutosAdd p = new ProdutosAdd
                {
                    produtos = this,
                    id = item.Id,
                    cod = item.Cod,
                    name = item.Name,
                    price = item.Price
                };
                p.ShowDialog();
            }
            else
            {
                MessageBox.Show("Selecione um PRODUTO para ser editado!", "ERRO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void AddProduto_Click(object sender, RoutedEventArgs e)
        {
            new ProdutosAdd
            {
                produtos = this
            }.ShowDialog();
        }
    }
}
