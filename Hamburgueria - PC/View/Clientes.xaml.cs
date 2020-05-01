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
    /// Lógica interna para Clientes.xaml
    /// </summary>
    public partial class Clientes : Window
    {
        public Clientes()
        {
            InitializeComponent();

            this.Loaded += Clientes_Loaded;

            this.GridClientes.BeginningEdit +=  (sender, e) => e.Cancel = true;

            this.Search.GotFocus += Search_GotFocus;
            this.Search.TextChanged += Search_TextChanged;

            this.BackCliente.Click += BackCliente_Click;
            this.DelCliente.Click += DelCliente_Click;
            this.EditCliente.Click += EditCliente_Click;
            this.AddCliente.Click += AddCliente_Click;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        public void Clientes_Loaded(object sender, RoutedEventArgs e)
        {
            GridClientes.Items.Clear();
            var clients = new Hamburgueria.Sql.Client().Select();
            foreach (var c in clients)
                GridClientes.Items.Add(c);
        }

        private void Search_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Search.Text == "Pesquisar")
                Search.Text = "";
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = Search.Text;
            GridClientes.Items.Clear();

            if (string.IsNullOrWhiteSpace(text))
            {
                var clients = new Hamburgueria.Sql.Client().Select();
                foreach (var c in clients)
                    GridClientes.Items.Add(c);
            }
            else
            {
                var clients = new Hamburgueria.Sql.Client().Select(text);
                foreach (var c in clients)
                    GridClientes.Items.Add(c);
            }
        }

        private void BackCliente_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DelCliente_Click(object sender, RoutedEventArgs e)
        {
            if (GridClientes.SelectedIndex != -1)
            {
                var select = (Hamburgueria.Tables.Client)GridClientes.SelectedItem;
                new Hamburgueria.Sql.Client().Delete(select.Id);

                Clientes_Loaded(null, null);
            }
            else
            {
                MessageBox.Show("Selecione um CLIENTE para ser editado!", "ERRO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void EditCliente_Click(object sender, RoutedEventArgs e)
        {
            if (GridClientes.SelectedIndex != -1)
            {
                var select = (Hamburgueria.Tables.Client)GridClientes.SelectedItem;

                ClientesAdd c = new ClientesAdd();
                c.clients = this;
                c.id = select.Id;
                c.name = select.Name;
                c.address = select.Street;
                c.number = select.Number.ToString();
                c.district = select.District;
                c.complement = select.Complement;
                c.telephone = select.Telephone;
                c.reference = select.Reference;
                c.ShowDialog();
            }
            else
            {
                MessageBox.Show("Selecione um CLIENTE para ser editado!", "ERRO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void AddCliente_Click(object sender, RoutedEventArgs e)
        {
            ClientesAdd c = new ClientesAdd();
            c.clients = this;
            c.ShowDialog();
        }
    }
}
