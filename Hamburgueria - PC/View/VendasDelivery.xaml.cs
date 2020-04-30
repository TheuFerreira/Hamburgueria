using System;
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

        private readonly Vendas sales;
        private string fileName;
        private bool isEditing = false;
        private DateTime dateSale;

        public VendasDelivery(Vendas sales)
        {
            InitializeComponent();

            this.sales = sales;
            this.Closed += (sender, e) => sales.UpdateGrid();
            this.Loaded += VendasDelivery_Loaded;

            // CLIENT

            //this.searchName.LostFocus += (sender, e) => gridClient.Visibility = Visibility.Hidden;
            this.searchName.PreviewKeyDown += SearchName_PreviewKeyDown;
            this.searchName.TextChanged += SearchName_TextChanged;

            this.gridClient.MouseDoubleClick += GridClient_MouseDoubleClick;

            this.number.PreviewTextInput += (sender, e) => e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);

            this.discount.GotFocus += Discount_GotFocus;
            this.discount.LostFocus += Discount_LostFocus;
            this.discount.PreviewTextInput += (sender, e) => e.Handled = new Regex("[^0-9,]+").IsMatch(e.Text);

            // Product

            this.searchProduct.PreviewKeyDown += SearchProduct_PreviewKeyDown;
            this.searchProduct.PreviewTextInput += SearchProduct_PreviewTextInput;
            this.searchProduct.TextChanged += SearchProduct_TextChanged;

            this.gridSearch.PreviewKeyDown += GridSearch_PreviewKeyDown;
            this.gridSearch.MouseDoubleClick += GridSearch_MouseDoubleClick;

            this.quantity.PreviewKeyDown += Quantity_PreviewKeyDown;
            this.quantity.PreviewTextInput += (sender, e) => e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);

            this.gridProduct.BeginningEdit += (sender, e) => e.Cancel = true;
            this.gridProduct.PreviewKeyDown += GridProduct_PreviewKeyDown;

            this.searchName.LostFocus += (sender, e) => CheckClient();
            this.street.LostFocus += (sender, e) => CheckClient(); ;
            this.number.LostFocus += (sender, e) => CheckClient(); ;
            this.district.LostFocus += (sender, e) => CheckClient(); ;

            // BUTTONS

            this.newClient.Click += NewClient;
            this.confirm.Click += Confirm_Click;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        public void LoadEditing(string fileName, Model.Cliente.Item oldAddress, decimal totalSale, string payment, string discount, DateTime dateSale, string observation, List<Item> items)
        {
            isEditing = true;
            labelTotalSale.Content = "TOTAL:" + totalSale.ToString("C2");
            this.fileName = fileName;
            this.totalSale = totalSale;
            this.dateSale = dateSale;

            searchName.Text = oldAddress.NAME;
            number.Text = oldAddress.NUMBER;
            street.Text = oldAddress.ADDRESS;
            district.Text = oldAddress.DISTRICT;
            complement.Text = oldAddress.COMPLEMENT;
            telephone.Text = oldAddress.TELEPHONE;
            Reference.Text = oldAddress.REFERENCE;

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

            foreach (Item it in items)
                gridProduct.Items.Add(it);
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
                    var selected = (Model.Cliente.Item)gridClient.SelectedItem;

                    searchName.Text = selected.NAME;
                    number.Text = selected.NUMBER;
                    street.Text = selected.ADDRESS;
                    district.Text = selected.DISTRICT;
                    complement.Text = selected.COMPLEMENT;
                    telephone.Text = selected.TELEPHONE;
                    Reference.Text = selected.REFERENCE;

                    searchProduct.Focus();
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
            gridClient.Items.Clear();
            Model.Cliente.Select(gridClient, text);
            if (gridClient.HasItems)
                gridClient.SelectedItem = gridClient.Items[0];
            else
                gridClient.Visibility = Visibility.Hidden;
        }

        private void GridClient_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (gridClient.HasItems)
            {
                var selected = (Model.Cliente.Item)gridClient.SelectedItem;

                searchName.Text = selected.NAME;
                number.Text = selected.NUMBER;
                street.Text = selected.ADDRESS;
                district.Text = selected.DISTRICT;
                complement.Text = selected.COMPLEMENT;
                Reference.Text = selected.REFERENCE;

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

            totalSale = 0;
            for (int i = 0; i < gridProduct.Items.Count; i++)
                totalSale += ((Item)gridProduct.Items[i]).Total;

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

            bool exist = Model.Cliente.Exist(searchName.Text, street.Text, district.Text, number.Text);
        
            if (exist)
                newClient.Visibility = Visibility.Collapsed;
            else
                newClient.Visibility = Visibility.Visible;
        }

        private void NewClient(object sender, RoutedEventArgs e)
        {
            Model.Cliente.Insert(searchName.Text, street.Text, district.Text, number.Text, complement.Text, telephone.Text, Reference.Text);

            MessageBox.Show("Cliente cadastrado com sucesso!!!");

            newClient.Visibility = Visibility.Collapsed;
        }

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
                NUMBER = number.Text,
                TELEPHONE = telephone.Text,
                REFERENCE = Reference.Text
            };

            if (isEditing == false)
            {
                decimal total = 0;
                List<Item> items = new List<Item>();
                foreach (Item i in gridProduct.Items)
                {
                    total += i.Total;
                    items.Add(i);
                }
                Sales.Delivery.Create(address, DateTime.Now, total, payment.Text, Convert.ToDecimal(discount.Text), observation.Text, items);

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
                List<Item> items = new List<Item>();
                foreach (Item i in gridProduct.Items)
                {
                    total += i.Total;
                    items.Add(i);
                }
                Sales.Delivery.Edit(fileName, address, dateSale, total, payment.Text, Convert.ToDecimal(discount.Text), observation.Text, items);

                MessageBox.Show("Venda alterada com sucesso!!!");

                this.Close();
            }
        }
    }
}
