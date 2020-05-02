using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Hamburgueria.View
{
    /// <summary>
    /// Lógica interna para Clientes.xaml
    /// </summary>
    public partial class Clientes : Window
    {
        private readonly Sql.Client sqlClient;

        public Clientes()
        {
            InitializeComponent();

            sqlClient = new Sql.Client();

            Loaded += Clientes_Loaded;

            GridClientes.PreviewMouseDoubleClick += GridClientes_PreviewMouseDoubleClick;

            Search.GotFocus += Search_GotFocus;
            Search.PreviewKeyDown += Search_PreviewKeyDown;
            Search.TextChanged += Search_TextChanged;

            BackCliente.Click += (sender, e) => Close();
            DelCliente.Click += DelCliente_Click;
            EditCliente.Click += (sender, e) => GridClientes_PreviewMouseDoubleClick(null, null);
            AddCliente.Click += AddCliente_Click;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void Clientes_Loaded(object sender, RoutedEventArgs e)
        {
            GridClientes.Items.Clear();
            foreach (var c in sqlClient.Select())
                GridClientes.Items.Add(c);
        }

        private void GridClientes_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GridClientes.SelectedItem == null)
                return;

            if (GridClientes.SelectedIndex != -1)
            {
                Tables.Client selected = (Tables.Client)GridClientes.SelectedItem;
                new ClientesAdd(selected).ShowDialog();
                Search_TextChanged(null, null);
            }
            else
            {
                MessageBox.Show("Selecione um CLIENTE para ser editado!", "ERRO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Search_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Search.Text == "Pesquisar")
                Search.Text = "";
        }

        private void Search_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (GridClientes.HasItems)
            {
                if (e.Key == Key.Down)
                {
                    int index = GridClientes.SelectedIndex + 1;
                    if (index == GridClientes.Items.Count)
                        index = 0;
                    GridClientes.SelectedIndex = index;
                }
                else if (e.Key == Key.Up)
                {
                    int index = GridClientes.SelectedIndex - 1;
                    if (index < 0)
                        index = GridClientes.Items.Count - 1;
                    GridClientes.SelectedIndex = index;
                }
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = Search.Text;
            GridClientes.Items.Clear();

            if (string.IsNullOrWhiteSpace(text) || Search.Text == "Pesquisar")
                foreach (var c in sqlClient.Select())
                    GridClientes.Items.Add(c);
            else
                foreach (var c in sqlClient.Select(text))
                    GridClientes.Items.Add(c);
        }

        private void DelCliente_Click(object sender, RoutedEventArgs e)
        {
            if (GridClientes.SelectedIndex != -1)
            {
                var select = (Tables.Client)GridClientes.SelectedItem;
                sqlClient.Delete(select.Id);

                Search_TextChanged(null, null);
            }
            else
            {
                MessageBox.Show("Selecione um CLIENTE para ser editado!", "ERRO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void AddCliente_Click(object sender, RoutedEventArgs e)
        {
#pragma warning disable IDE0017 // Simplificar a inicialização de objeto
            DispatcherTimer timerToUpdate = new DispatcherTimer();
#pragma warning restore IDE0017 // Simplificar a inicialização de objeto
            timerToUpdate.Interval = new System.TimeSpan(0, 0, 1);
            timerToUpdate.Tick += (s, r) => Search_TextChanged(null, null);
            timerToUpdate.Start();

            new ClientesAdd().ShowDialog();
            timerToUpdate.Stop();
        }
    }
}
