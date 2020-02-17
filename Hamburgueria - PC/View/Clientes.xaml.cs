using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Lógica interna para Clientes.xaml
    /// </summary>
    public partial class Clientes : Window
    {
        public Clientes()
        {
            InitializeComponent();

            AddCliente.Click += delegate { new ClientesAdd().ShowDialog(); };

            Item i1 = new Item("Matheus", "Avenida 1º de Junho", 162, "Centro", "Casa", "Igreja Matriz");
            Item i2 = new Item("Paulo", "Jao Kisse", 666, "Inferno", "Casa", "Perto da Casa do Karalho");

            List<Item> items = new List<Item>() { i1, i2 };
            //GridClientes.ItemsSource = items;
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
