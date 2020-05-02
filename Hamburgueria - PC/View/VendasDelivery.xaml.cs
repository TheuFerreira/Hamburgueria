using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Hamburgueria.View
{
    /// <summary>
    /// Interação lógica para VendasDelivery.xam
    /// </summary>
    public partial class VendasDelivery : Window
    {
        public ObservableCollection<Item> Items = new ObservableCollection<Item>();

        private readonly Vendas sales;
        private string fileName;
        private bool isEditing = false;
        private DateTime dateSale;

        public VendasDelivery(Vendas sales)
        {
            InitializeComponent();

            gridProduct.DataContext = Items;

            this.sales = sales;
            Closed += (sender, e) => sales.UpdateGrid();
            Loaded += VendasDelivery_Loaded;

            // CLIENT

            searchName.PreviewKeyDown += SearchName_PreviewKeyDown;
            searchName.TextChanged += SearchName_TextChanged;

            gridClient.MouseDoubleClick += GridClient_MouseDoubleClick;

            number.PreviewTextInput += (sender, e) => e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);

            discount.GotFocus += Discount_GotFocus;
            discount.LostFocus += Discount_LostFocus;
            discount.PreviewTextInput += (sender, e) => e.Handled = new Regex("[^0-9,]+").IsMatch(e.Text);

            // Product

            searchProduct.PreviewKeyDown += SearchProduct_PreviewKeyDown;
            searchProduct.PreviewTextInput += SearchProduct_PreviewTextInput;
            searchProduct.TextChanged += SearchProduct_TextChanged;

            gridSearch.PreviewKeyDown += GridSearch_PreviewKeyDown;
            gridSearch.MouseDoubleClick += GridSearch_MouseDoubleClick;

            quantity.PreviewKeyDown += Quantity_PreviewKeyDown;
            quantity.PreviewTextInput += (sender, e) => e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);

            gridProduct.BeginningEdit += (sender, e) => e.Cancel = true;
            gridProduct.PreviewKeyDown += GridProduct_PreviewKeyDown;

            searchName.LostFocus += (sender, e) => CheckClient();
            street.LostFocus += (sender, e) => CheckClient(); ;
            number.LostFocus += (sender, e) => CheckClient(); ;
            district.LostFocus += (sender, e) => CheckClient(); ;

            // BUTTONS

            newClient.Click += NewClient;
            confirm.Click += Confirm_Click;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        public void LoadEditing(string fileName, Tables.Client oldAddress, decimal totalSale, string payment, string discount, DateTime dateSale, string observation, List<Item> items)
        {
            isEditing = true;
            labelTotalSale.Content = "TOTAL:" + totalSale.ToString("C2");
            this.fileName = fileName;
            this.dateSale = dateSale;

            searchName.Text = oldAddress.Name;
            number.Text = oldAddress.Number.ToString();
            street.Text = oldAddress.Street;
            district.Text = oldAddress.District;
            complement.Text = oldAddress.Complement;
            telephone.Text = oldAddress.Telephone;
            Reference.Text = oldAddress.Reference;

            this.observation.Text = observation;

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

            Items = new ObservableCollection<Item>(items);
        }

        private void VendasDelivery_Loaded(object sender, RoutedEventArgs e)
        {
            gridSearch.Visibility = Visibility.Hidden;
            gridClient.Visibility = Visibility.Hidden;
        }

        // CLIENT

        private void SearchName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (gridClient.HasItems)
                {
                    var selected = (Tables.Client)gridClient.SelectedItem;

                    searchName.Text = selected.Name;
                    number.Text = selected.Number.ToString();
                    street.Text = selected.Street;
                    district.Text = selected.District;
                    complement.Text = selected.Complement;
                    telephone.Text = selected.Telephone;
                    Reference.Text = selected.Reference;

                    searchProduct.Focus();
                }

                gridClient.Visibility = Visibility.Hidden;
            }
            else if (gridClient.HasItems)
            {
                if (e.Key == Key.Down)
                {
                    int index = gridClient.SelectedIndex++;
                    if (index == gridClient.Items.Count)
                        index = 0;
                    gridClient.SelectedIndex = index;
                }
                else if (e.Key == Key.Up)
                {
                    int index = gridClient.SelectedIndex--;
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
            gridClient.Items.Clear();
            var clients = new Hamburgueria.Sql.Client().Select(text);
            foreach (var c in clients)
                gridClient.Items.Add(c);
            if (gridClient.HasItems)
                gridClient.SelectedItem = gridClient.Items[0];
            else
                gridClient.Visibility = Visibility.Hidden;
        }

        private void GridClient_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (gridClient.HasItems)
            {
                var selected = (Tables.Client)gridClient.SelectedItem;

                searchName.Text = selected.Name;
                number.Text = selected.Number.ToString();
                street.Text = selected.Street;
                district.Text = selected.District;
                complement.Text = selected.Complement;
                Reference.Text = selected.Reference;

                searchProduct.Focus();
            }

            gridClient.Visibility = Visibility.Hidden;
        }

        private void Discount_GotFocus(object sender, RoutedEventArgs e)
        {
            if (discount.Text == "0,00")
                discount.Text = "";
        }

        private void Discount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(discount.Text, out decimal d))
                discount.Text = d.ToString("N2");
            else
                discount.Text = "0,00";

            labelTotalSale.Content = "TOTAL:" + TotalSale().ToString("C2");
        }

        // PRODUCT
        private bool isNumber = false;
        private Tables.Product product = null;

        private void SearchProduct_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GridSearch_MouseDoubleClick(null, null);
            }
            else if (gridSearch.HasItems)
            {
                if (e.Key == Key.Down)
                {
                    int index = gridSearch.SelectedIndex++;
                    if (index == gridSearch.Items.Count)
                        index = 0;
                    gridSearch.SelectedIndex = index;
                }
                else if (e.Key == Key.Up)
                {
                    int index = gridSearch.SelectedIndex--;
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
                        i.Quantity += q;

                        exist = true;
                        break;
                    }
                }

                if (exist == false)
                    Items.Add(new Item(product.Id, product.Cod, product.Name, product.Price, q));

                labelTotalSale.Content = "TOTAL:" + TotalSale().ToString("C2");

                quantity.Text = "";
                searchProduct.Text = "";
                gridSearch.Visibility = Visibility.Hidden;

                searchProduct.Focus();
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

        // BUTTONS

        private void CheckClient()
        {
            if (string.IsNullOrEmpty(searchName.Text) ||
                string.IsNullOrEmpty(street.Text) ||
                string.IsNullOrEmpty(number.Text) ||
                string.IsNullOrEmpty(district.Text))
            {
                newClient.Visibility = Visibility.Collapsed;
                return;
            }

            bool exist = new Hamburgueria.Sql.Client().Exist(searchName.Text, street.Text, Convert.ToInt32(number.Text), district.Text);
        
            if (exist)
                newClient.Visibility = Visibility.Collapsed;
            else
                newClient.Visibility = Visibility.Visible;
        }

        private void NewClient(object sender, RoutedEventArgs e)
        {
            Tables.Client client = new Tables.Client(searchName.Text, street.Text, Convert.ToInt32(number.Text), district.Text, complement.Text, telephone.Text, Reference.Text);
            new Sql.Client().AddOrUpdate(client);

            MessageBox.Show("Cliente cadastrado com sucesso!!!");

            newClient.Visibility = Visibility.Collapsed;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchName.Text) ||
                string.IsNullOrEmpty(street.Text) ||
                string.IsNullOrEmpty(number.Text) ||
                string.IsNullOrEmpty(district.Text))
            {
                MessageBox.Show("Por favor preencha todas as informações");
                return;
            }
            else if (gridProduct.HasItems == false)
            {
                MessageBox.Show("É necessário ter produtos a venda");
                return;
            }

            Tables.Client address = new Tables.Client(searchName.Text, street.Text, Convert.ToInt32(number.Text), district.Text, complement.Text, telephone.Text, Reference.Text);
            
            if (isEditing == false)
            {
                decimal total = 0;
                foreach (Item i in Items)
                    total += i.Total;
                Sales.Delivery.Create(address, DateTime.Now, total, payment.Text, Convert.ToDecimal(discount.Text), observation.Text, Items.ToList());

                searchName.Text = "";
                street.Text = "";
                complement.Text = "";
                district.Text = "";
                number.Text = "";
                discount.Text = "0,00";
                telephone.Text = "";
                Reference.Text = "";

                gridProduct.Items.Clear();
                labelTotalSale.Content = "TOTAL:R$0,00";
                quantity.Text = "";
                observation.Text = "";
                searchName.Focus();

                MessageBox.Show("Venda adicionada com sucesso!!!");

                sales.UpdateGrid();
            }
            else
            {
                decimal total = 0;
                foreach (Item i in Items)
                    total += i.Total;
                Sales.Delivery.Edit(fileName, address, dateSale, total, payment.Text, Convert.ToDecimal(discount.Text), observation.Text, Items.ToList());

                MessageBox.Show("Venda alterada com sucesso!!!");

                this.Close();
            }
        }

        private decimal TotalSale()
        {
            decimal t = -Convert.ToDecimal(discount.Text);
            foreach (Item i in Items)
                t += i.Total;

            return t;
        }
    }
}
