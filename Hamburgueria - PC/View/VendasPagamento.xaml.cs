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
using System.Windows.Shapes;

namespace Hamburgueria.View
{
    /// <summary>
    /// Lógica interna para VendasPagamento.xaml
    /// </summary>
    public partial class VendasPagamento : Window
    {
        public Vendas sales;
        public int typeSale = -1;
        public int numTable;
        public DateTime dateSale;
        public List<Item> items;
        public string observation = "";

        private decimal desconto = 0;
        private decimal pago = 0;

        public bool Confirmed = false;

        public VendasPagamento(decimal valorTotal)
        {
            InitializeComponent();

            bruteValue.Content = valorTotal.ToString("N2");

            Values_TextChanged(null, null);

            this.payment.SelectionChanged += Payment_SelectionChanged;

            this.discount.PreviewTextInput += Values_PreviewTextInput;
            this.valuePay.PreviewTextInput += Values_PreviewTextInput;

            this.discount.GotFocus += Values_GotFocus;
            this.valuePay.GotFocus += Values_GotFocus;

            this.discount.LostFocus += Values_LostFocus;
            this.valuePay.LostFocus += Values_LostFocus;

            this.discount.TextChanged += Values_TextChanged;
            this.valuePay.TextChanged += Values_TextChanged;

            this.cancel.Click += Cancel_Click;
            this.print.Click += Print_Click;
            this.confirm.Click += Confirm_Click;
        }

        private void Payment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (payment.SelectedIndex == 0)
            {
                valuePay.Visibility = Visibility.Visible;
                change.Visibility = Visibility.Visible;

                labelValuePay.Visibility = Visibility.Visible;
                labelChange.Visibility = Visibility.Visible;

                Values_TextChanged(null, null);
            }
            else
            {
                valuePay.Visibility = Visibility.Hidden;
                change.Visibility = Visibility.Hidden;

                labelValuePay.Visibility = Visibility.Hidden;
                labelChange.Visibility = Visibility.Hidden;

                confirm.Visibility = Visibility.Visible;
                print.Visibility = Visibility.Visible;
            }
        }

        private void Values_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            t.SelectAll();

            if (t.Text == "0,00")
                t.Text = "";
        }

        private void Values_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (string.IsNullOrEmpty(tb.Text))
                tb.Text = "0,00";
            else
                tb.Text = Convert.ToDecimal(tb.Text).ToString("N2");
        }

        private void Values_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+,").IsMatch(e.Text);
        }

        private void Values_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(discount.Text))
                desconto = 0;
            else
                desconto = Convert.ToDecimal(discount.Text);
            totalValue.Content = (Convert.ToDecimal(bruteValue.Content) - desconto).ToString();

            if (payment.SelectedIndex == 0)
            {
                if (string.IsNullOrEmpty(valuePay.Text))
                    pago = 0;
                else
                    pago = Convert.ToDecimal(valuePay.Text);

                change.Content = (pago - Convert.ToDecimal(totalValue.Content)).ToString();

                decimal total = Convert.ToDecimal(totalValue.Content);
                if (pago < total)
                {
                    confirm.Visibility = Visibility.Hidden;
                    print.Visibility = Visibility.Hidden;
                }
                else
                {
                    confirm.Visibility = Visibility.Visible;
                    print.Visibility = Visibility.Visible;
                }
            }
            else
            {
                pago = 0;
                valuePay.Text = "0,00";
                change.Content = "0,00";

                confirm.Visibility = Visibility.Visible;
                print.Visibility = Visibility.Visible;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            if (typeSale == 1)
                TXT.Sale(dateSale, Convert.ToDecimal(bruteValue.Content), Convert.ToDecimal(discount.Text), Convert.ToDecimal(totalValue.Content), Convert.ToDecimal(valuePay.Text), Convert.ToDecimal(change.Content), payment.Text, observation, Sales.Balcao.Products(numTable));
            else
                TXT.Sale(dateSale, Convert.ToDecimal(bruteValue.Content), Convert.ToDecimal(discount.Text), Convert.ToDecimal(totalValue.Content), Convert.ToDecimal(valuePay.Text), Convert.ToDecimal(change.Content), payment.Text, items);
            new Impressao().ShowDialog();
            Confirm_Click(null, null);
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            // TABLE
            if (typeSale == 1)
            {
                new Sql.Sale().Insert(numTable, dateSale, Convert.ToDecimal(bruteValue.Content), Convert.ToDecimal(discount.Text), Convert.ToDecimal(totalValue.Content), payment.Text, Sales.Balcao.Products(numTable));

                Sales.Balcao.Delete(numTable);

                sales.UpdateGrid();
            }
            // FAST
            else
            {
                new Sql.Sale().Insert(dateSale, Convert.ToDecimal(bruteValue.Content), Convert.ToDecimal(discount.Text), Convert.ToDecimal(totalValue.Content), payment.Text, items);
            }

            Confirmed = true;
            this.Close();
        }
    }
}
