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
            GridClientes.ItemsSource = Model.Cliente.GetAll();

            GridClientes.Columns[0].Visibility = Visibility.Collapsed;

            GridClientes.Columns[1].Header = "NOME";
            GridClientes.Columns[2].Header = "RUA";
            GridClientes.Columns[3].Header = "NÚMERO";
            GridClientes.Columns[4].Header = "BAIRRO";
            GridClientes.Columns[5].Header = "COMPLEMENTO";
            GridClientes.Columns[6].Header = "REFERÊNCIA";

            GridClientes.Columns[1].MinWidth = 100;
            GridClientes.Columns[2].MinWidth = 150;
            GridClientes.Columns[3].MinWidth = 100;
            GridClientes.Columns[4].MinWidth = 100;
            GridClientes.Columns[5].MinWidth = 150;
            GridClientes.Columns[6].MinWidth = 150;

            Style s = new Style(typeof(DataGridColumnHeader));
            s.Setters.Add(new Setter(DataGridRow.BackgroundProperty, (SolidColorBrush)FindResource("AzulBruxao")));
            s.Setters.Add(new Setter(DataGridRow.ForegroundProperty, Brushes.White));
            for (int i = 0; i < GridClientes.Columns.Count; i++)
                GridClientes.Columns[i].HeaderStyle = s;
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
    }
}
