using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Hamburgueria.View
{
    /// <summary>
    /// Lógica interna para ProdutosAdd.xaml
    /// </summary>
    public partial class ProdutosAdd : Window
    {
        private readonly Sql.Product sqlProduct;
        private readonly int idProduct = -1;
        private readonly int codProduct = -1;

        public ProdutosAdd(Tables.Product product = null)
        {
            InitializeComponent();

            sqlProduct = new Sql.Product();

            if (product != null)
            {
                idProduct = product.Id;
                codProduct = product.Cod;
                Name.Text = product.Name;
                Code.Text = product.Cod.ToString();
                Price.Text = product.Price.ToString();
            }

            Loaded += ProdutosAdd_Loaded;

            Code.PreviewTextInput += (s, e) => e.Handled = new Regex("[^0-9]").IsMatch(e.Text);
            Price.PreviewTextInput += (s, e) => e.Handled = new Regex("[^0-9,.]").IsMatch(e.Text);

            ClearBtn.Click += ClearBtn_Click;
            SaveBtn.Click += SaveBtn_Click;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void ProdutosAdd_Loaded(object sender, RoutedEventArgs e)
        {
            Name.Focus();
            Name.SelectAll();
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            Name.Text = string.Empty;
            Code.Text = string.Empty;
            Price.Text = string.Empty;

            Name.Focus();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Price.Text) || 
                string.IsNullOrEmpty(Name.Text) || 
                string.IsNullOrEmpty(Code.Text))
            {
                MessageBox.Show("Todos os campos precisam serem preenchidos!!!", "ERRO", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            int newCod = Convert.ToInt32(Code.Text);
            if (idProduct == -1)
            {
                if (sqlProduct.Exist(newCod))
                {
                    MessageBox.Show("O Codigo digitado, já pertence a outro PRODUTO!!!", "ERRO", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Tables.Product product = new Tables.Product(newCod, Name.Text, Convert.ToDecimal(Price.Text));
                new Sql.Product().AddOrUpdate(product);

                MessageBox.Show("Produto cadastrado com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearBtn_Click(null, null);
            }
            else
            {
                if (codProduct != newCod)
                {
                    if (sqlProduct.Exist(codProduct))
                    {
                        MessageBox.Show("O Codigo digitado, já pertence a outro PRODUTO!!!", "ERRO", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                Tables.Product product = new Tables.Product(idProduct, newCod, Name.Text, Convert.ToDecimal(Price.Text));
                new Sql.Product().AddOrUpdate(product);

                Close();
            }
        }
    }
}
