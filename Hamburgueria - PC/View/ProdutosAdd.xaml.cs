﻿using System;
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
    /// Lógica interna para ProdutosAdd.xaml
    /// </summary>
    public partial class ProdutosAdd : Window
    {
        public Produtos produtos;

        public int id = -1;
        public int cod;
        public string name;
        public decimal price;

        public ProdutosAdd()
        {
            InitializeComponent();

            this.Loaded += ProdutosAdd_Loaded;
            this.Closed += ProdutosAdd_Closed;

            this.Code.PreviewTextInput += Code_PreviewTextInput;
            this.Price.PreviewTextInput += Price_PreviewTextInput;

            this.ClearBtn.Click += ClearBtn_Click;
            this.SaveBtn.Click += SaveBtn_Click;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        private void ProdutosAdd_Closed(object sender, EventArgs e)
        {
            produtos.Produtos_Loaded(null, null);
        }

        private void ProdutosAdd_Loaded(object sender, RoutedEventArgs e)
        {
            if (id != -1)
            {
                Name.Text = name;
                Code.Text = cod.ToString();
                Price.Text = price.ToString();
            }
        }

        private void Code_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]").IsMatch(e.Text);
        }

        private void Price_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9,.]").IsMatch(e.Text);
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            Name.Text = "";
            Code.Text = "";
            Price.Text = "";
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Price.Text) || string.IsNullOrEmpty(Name.Text) || string.IsNullOrEmpty(Code.Text))
            {
                MessageBox.Show("Todos os campos precisam serem preenchidos!!!", "ERRO", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (id == -1)
            {
                cod = Convert.ToInt32(Code.Text);
                if (new Hamburgueria.Sql.Product().Exist(cod))
                {
                    MessageBox.Show("O Codigo digitado, já pertence a outro PRODUTO!!!", "ERRO", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                decimal price = Convert.ToDecimal(Price.Text);

                Hamburgueria.Tables.Product product = new Hamburgueria.Tables.Product(cod, Name.Text, price);
                new Hamburgueria.Sql.Product().AddOrUpdate(product);

                MessageBox.Show("Produto cadastrado com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Information);

                Name.Text = "";
                Code.Text = "";
                Price.Text = "";

                produtos.Produtos_Loaded(null, null);
            }
            else
            {
                int newCod = Convert.ToInt32(Code.Text);
                name = Name.Text;
                price = Convert.ToDecimal(Price.Text);

                if (cod != newCod)
                {
                    if (new Hamburgueria.Sql.Product().Exist(cod))
                    {
                        MessageBox.Show("O Codigo digitado, já pertence a outro PRODUTO!!!", "ERRO", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                Hamburgueria.Tables.Product product = new Hamburgueria.Tables.Product(id, newCod, name, price);
                new Hamburgueria.Sql.Product().AddOrUpdate(product);

                this.Close();
            }
        }
    }
}
