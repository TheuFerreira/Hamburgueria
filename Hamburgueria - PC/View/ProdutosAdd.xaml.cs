using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Lógica interna para ProdutosAdd.xaml
    /// </summary>
    public partial class ProdutosAdd : Window
    {
        public ProdutosAdd()
        {
            InitializeComponent();

            this.Loaded += ProdutosAdd_Loaded;
        }

        private void ProdutosAdd_Loaded(object sender, RoutedEventArgs e)
        {
            TypeProduto.ItemsSource = Model.Produto.GetAllType();
        }
    }
}
