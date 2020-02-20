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
            AddCliente.Click += delegate { new ClientesAdd().ShowDialog(); };

            Item i1 = new Item("Matheus", "Avenida 1º de Junho", 162, "Centro", "Casa", "Igreja Matriz");
            Item i2 = new Item("Paulo", "Jao Kisse", 666, "Inferno", "Casa", "Perto da Casa do Karalho");

            List<Item> items = new List<Item>() { i1, i2 };
            GridClientes.ItemsSource = items;
        }

        private void Clientes_Loaded(object sender, RoutedEventArgs e)
        {
            GridClientes.Columns[0].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            GridClientes.Columns[1].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            GridClientes.Columns[2].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            GridClientes.Columns[3].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            GridClientes.Columns[4].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            GridClientes.Columns[5].Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            GridClientes.Columns[2].MinWidth = 75;
            GridClientes.Columns[4].MinWidth = 120;

            Style s = new Style(typeof(DataGridColumnHeader));
            s.Setters.Add(new Setter(DataGridRow.BackgroundProperty, (SolidColorBrush) FindResource("AzulBruxao")));
            s.Setters.Add(new Setter(DataGridRow.ForegroundProperty, Brushes.White));
            for (int i = 0; i < GridClientes.Columns.Count; i++)
                GridClientes.Columns[i].HeaderStyle = s;
        }

        public class Item
        {
            public string NOME { get; set; }
            public string RUA { get; set; }
            public int NUMERO { get; set; }
            public string BAIRRO { get; set; }
            public string COMPLEMENTO { get; set; }
            public string REFERENCIA { get; set; }

            public Item(string nome, string rua, int numero, string bairro, string complemento, string referencia)
            {
                this.NOME = nome;
                this.RUA = rua;
                this.REFERENCIA = referencia;
                this.NUMERO = numero;
                this.BAIRRO = bairro;
                this.COMPLEMENTO = complemento;
            }
        }
    }
}
