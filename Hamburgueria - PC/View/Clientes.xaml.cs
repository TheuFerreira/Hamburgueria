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

            GridClientes.Columns[0].MinWidth = 100;
            GridClientes.Columns[1].MinWidth = 150;
            GridClientes.Columns[2].MinWidth = 100;
            GridClientes.Columns[3].MinWidth = 100;
            GridClientes.Columns[4].MinWidth = 150;
            GridClientes.Columns[5].MinWidth = 150;

            Style s = new Style(typeof(DataGridColumnHeader));
            s.Setters.Add(new Setter(DataGridRow.BackgroundProperty, (SolidColorBrush)FindResource("AzulBruxao")));
            s.Setters.Add(new Setter(DataGridRow.ForegroundProperty, Brushes.White));
            for (int i = 0; i < GridClientes.Columns.Count; i++)
                GridClientes.Columns[i].HeaderStyle = s;
        }

        private void AddCliente_Click(object sender, RoutedEventArgs e)
        {
            ClientesAdd c = new ClientesAdd();
            c.clients = this;
            c.ShowDialog();
        }

        private void AddCliente_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void EditCliente_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
