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

namespace Hamburgueria.View
{
    /// <summary>
    /// Lógica interna para Produtos.xaml
    /// </summary>
    public partial class Produtos : Window
    {
        public Produtos()
        {
            InitializeComponent();

            this.Loaded += Produtos_Loaded;

            this.GridProdutos.BeginningEdit += (sender, e) => e.Cancel = true;

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
            GridProdutos.ItemsSource = Model.Produto.Select();

            GridProdutos.Columns[0].Visibility = Visibility.Collapsed;

            GridProdutos.Columns[1].Header = "CÓDIGO";
            GridProdutos.Columns[2].Header = "NOME";
            GridProdutos.Columns[3].Header = "PREÇO";

            GridProdutos.Columns[3].ClipboardContentBinding.StringFormat = "C2";

            GridProdutos.Columns[1].MinWidth = 150;
            GridProdutos.Columns[2].MinWidth = 150;
            GridProdutos.Columns[3].MinWidth = 200;

            GridProdutos.Columns[2].Width = new DataGridLength(1.0, DataGridLengthUnitType.Star);

            Style s = new Style(typeof(DataGridColumnHeader));
            s.Setters.Add(new Setter(DataGridRow.BackgroundProperty, (SolidColorBrush)FindResource("AzulBruxao")));
            s.Setters.Add(new Setter(DataGridRow.ForegroundProperty, Brushes.White));
            for (int i = 0; i < GridProdutos.Columns.Count; i++)
                GridProdutos.Columns[i].HeaderStyle = s;
        }

        private void DelProduto_Click(object sender, RoutedEventArgs e)
        {
            if (GridProdutos.SelectedIndex != -1)
            {
                int id = ((Model.Produto.Item)GridProdutos.SelectedItem).ID;
                Model.Produto.Delete(id);

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
                var item = (Model.Produto.Item)GridProdutos.SelectedItem;

                ProdutosAdd p = new ProdutosAdd();
                p.produtos = this;
                p.id = item.ID;
                p.cod = item.COD;
                p.name = item.NAME;
                p.price = item.PRICE;
                p.ShowDialog();
            }
            else
            {
                MessageBox.Show("Selecione um PRODUTO para ser editado!", "ERRO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void AddProduto_Click(object sender, RoutedEventArgs e)
        {
            ProdutosAdd p = new ProdutosAdd();
            p.produtos = this;
            p.ShowDialog();
        }
    }
}
