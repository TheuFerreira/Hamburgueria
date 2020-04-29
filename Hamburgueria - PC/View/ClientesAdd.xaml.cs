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

        public int id = -1;
        public string name = "";
        public string address = "";
        public string number = "";
        public string district = "";
        public string complement = "";
        public string telephone = "";
        public string reference = "";

        public ClientesAdd()
        {
            InitializeComponent();

            this.Loaded += ClientesAdd_Loaded;
            this.Closed += ClientesAdd_Closed;

            this.Number.PreviewTextInput += Number_PreviewTextInput;

            this.clientName.GotFocus += delegate { clientName.SelectAll(); };
            this.Adress.GotFocus += delegate { Adress.SelectAll(); };
            this.District.GotFocus += delegate { District.SelectAll(); };
            this.Number.GotFocus += delegate { Number.SelectAll(); };
            this.Complement.GotFocus += delegate { Complement.SelectAll(); };
            this.Telephone.GotFocus += delegate { Complement.SelectAll(); };
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
            clientName.Text = name;
            Adress.Text = address;
            District.Text = district;
            Number.Text = number;
            Complement.Text = complement;
            Telephone.Text = telephone;
            Reference.Text = reference;

            clientName.Focus();
            clientName.SelectAll();
        }

        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]").IsMatch(e.Text);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(clientName.Text) ||
                string.IsNullOrEmpty(Adress.Text) ||
                string.IsNullOrEmpty(District.Text) ||
                string.IsNullOrEmpty(Number.Text))
            {
                MessageBox.Show("Os campos com * não podem estar vazios!!!", "ALERTA", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (Model.Cliente.Exist(clientName.Text, Adress.Text, District.Text, Number.Text))
            {
               // MessageBox.Show("Já existe um cliente com exatamentes estas informações", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (id == -1)
            {
                Model.Cliente.Insert(clientName.Text, Adress.Text, District.Text, Number.Text, Complement.Text, Telephone.Text, Reference.Text);

                MessageBox.Show("Cliente cadastrado com Sucesso!!!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearBtn_Click(null, null);
                clients.Clientes_Loaded(null, null);
            }
            else
            {
                Model.Cliente.Update(id, clientName.Text, Adress.Text, District.Text, Number.Text, Complement.Text, Telephone.Text, Reference.Text);

                MessageBox.Show("Cliente atualizado com Sucesso!!!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            clientName.Text = String.Empty;
            Adress.Text = String.Empty;
            District.Text = String.Empty;
            Number.Text = String.Empty;
            Complement.Text = String.Empty;
            Telephone.Text = String.Empty;
            Reference.Text = String.Empty;

            clientName.Focus();
        }
    }
}
