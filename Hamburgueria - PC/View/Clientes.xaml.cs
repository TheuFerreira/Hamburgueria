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
            Model.Cliente.GetAll(GridClientes);
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = Search.Text;
            GridClientes.Items.Clear();

            if (string.IsNullOrWhiteSpace(text))
                Model.Cliente.GetAll(GridClientes);
            else
                Model.Cliente.Select(GridClientes, text);
        }

        private void BackCliente_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DelCliente_Click(object sender, RoutedEventArgs e)
        {
            if (GridClientes.SelectedIndex != -1)
            {
                var select = (Model.Cliente.Item)GridClientes.SelectedItem;
                Model.Cliente.Delete(select.ID);

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
                var select = (Model.Cliente.Item)GridClientes.SelectedItem;

                ClientesAdd c = new ClientesAdd();
                c.clients = this;
                c.id = select.ID;
                c.name = select.NAME;
                c.address = select.ADDRESS;
                c.number = select.NUMBER;
                c.district = select.DISTRICT;
                c.complement = select.COMPLEMENT;
                c.reference = select.REFERENCE;
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

        private void BackCliente_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
