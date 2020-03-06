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

            AddProduto.Click += AddProduto_Click;

        }

        private void AddProduto_Click(object sender, RoutedEventArgs e)
        {
            ProdutosAdd p = new ProdutosAdd();
            p.produtos = this;
            p.ShowDialog();
        }

        public void Produtos_Loaded(object sender, RoutedEventArgs e)
        {
            GridProdutos.ItemsSource = Model.Produto.Select();

            GridProdutos.Columns[0].Header = "CÓDIGO";
            GridProdutos.Columns[1].Header = "NOME";
            GridProdutos.Columns[2].Header = "TIPO";

            GridProdutos.Columns[0].MinWidth = 150;
            GridProdutos.Columns[1].MinWidth = 150;
            GridProdutos.Columns[2].MinWidth = 200;

            GridProdutos.Columns[1].Width = new DataGridLength(1.0, DataGridLengthUnitType.Star);

            Style s = new Style(typeof(DataGridColumnHeader));
            s.Setters.Add(new Setter(DataGridRow.BackgroundProperty, (SolidColorBrush)FindResource("AzulBruxao")));
            s.Setters.Add(new Setter(DataGridRow.ForegroundProperty, Brushes.White));
            for (int i = 0; i < GridProdutos.Columns.Count; i++)
                GridProdutos.Columns[i].HeaderStyle = s;
        }
    }
}
