using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Lógica interna para ClientesAdd.xaml
    /// </summary>
    public partial class ClientesAdd : Window
    {

        public Clientes clients;

        public ClientesAdd()
        {
            InitializeComponent();

            this.Loaded += ClientesAdd_Loaded;
            this.Closed += ClientesAdd_Closed;

            this.Number.PreviewTextInput += Number_PreviewTextInput;

            this.Name.GotFocus += delegate { Name.SelectAll(); };
            this.Adress.GotFocus += delegate { Adress.SelectAll(); };
            this.District.GotFocus += delegate { District.SelectAll(); };
            this.Number.GotFocus += delegate { Number.SelectAll(); };
            this.Complement.GotFocus += delegate { Complement.SelectAll(); };
            this.Reference.GotFocus += delegate { Reference.SelectAll(); };

            this.SaveBtn.Click += SaveBtn_Click;
            this.ClearBtn.Click += ClearBtn_Click;
        }

        private void ClientesAdd_Closed(object sender, EventArgs e)
        {
            clients.Clientes_Loaded(null, null);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        private void ClientesAdd_Loaded(object sender, RoutedEventArgs e)
        {
            Name.Focus();
        }

        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]").IsMatch(e.Text);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Name.Text) || 
                string.IsNullOrEmpty(Adress.Text) || 
                string.IsNullOrEmpty(District.Text) || 
                string.IsNullOrEmpty(Number.Text))
            {
                MessageBox.Show("Os campos com * não podem estar vazios!!!", "ALERTA", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Model.Cliente.Insert(Name.Text, Adress.Text, District.Text, Number.Text, Complement.Text, Reference.Text);

            MessageBox.Show("Cliente cadastrado com Sucesso!!!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);

            ClearBtn_Click(null, null);
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            Name.Text = String.Empty;
            Adress.Text = String.Empty;
            District.Text = String.Empty;
            Number.Text = String.Empty;
            Complement.Text = String.Empty;
            Reference.Text = String.Empty;

            Name.Focus();
        }

        private void ClearBtn_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
