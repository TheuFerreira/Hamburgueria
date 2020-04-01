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
    /// Lógica interna para Relatorios.xaml
    /// </summary>
    public partial class Relatorios : Window
    {
        public Relatorios()
        {
            InitializeComponent();

            this.Loaded += Relatorios_Loaded;

            this.styleBox.SelectionChanged += StyleBox_SelectionChanged;
            this.periodBox.SelectionChanged += PeriodBox_SelectionChanged;

            this.Filter.Click += Filter_Click;
        }

        private void Relatorios_Loaded(object sender, RoutedEventArgs e)
        {
            styleBox.SelectedIndex = 0;
            periodBox.SelectedIndex = 0;
            typeBox.SelectedIndex = 0;

            dateOne.DisplayDateStart = Model.Relatorio.GetMinDate();
            dateTwo.DisplayDateStart = Model.Relatorio.GetMinDate();

            dateOne.SelectedDate = DateTime.Now;
            dateTwo.SelectedDate = DateTime.Now;

            dateOne.DisplayDate = DateTime.Now;
            dateTwo.DisplayDate = DateTime.Now;

            dateOne.DisplayDateEnd = DateTime.Now;
            dateTwo.DisplayDateEnd = DateTime.Now;
        }

        private void StyleBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            typeBox.Visibility = styleBox.SelectedIndex == 0 ? Visibility.Visible : Visibility.Hidden;
        }

        private void PeriodBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dateTwo.Visibility = periodBox.SelectedIndex == 4 ? Visibility.Visible : Visibility.Hidden;
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            if (styleBox.SelectedIndex == 0)
            {
                if (periodBox.SelectedIndex == 0)
                {
                    if (typeBox.SelectedIndex == 2)
                    {
                        string date = dateOne.SelectedDate.Value.ToString("yyyy-MM-dd");
                        grid.ItemsSource = Model.Relatorio.SaleDayLocal(date);
                    }
                }
            }
            else if (styleBox.SelectedIndex == 1)
            {
                if (periodBox.SelectedIndex == 0)
                {
                    string date = dateOne.SelectedDate.Value.ToString("yyyy-MM-dd");
                    grid.ItemsSource = Model.Relatorio.Product(date);
                }
                else if (periodBox.SelectedIndex == 1)
                {
                    DateTime date1 = dateOne.SelectedDate.Value.StartOfWeek(DayOfWeek.Saturday);
                    DateTime date2 = date1.AddDays(6);
                    date1 = date1.AddDays(-1);
                    date2 = date2.AddDays(1);

                    string startDate = date1.ToString("yyyy-MM-dd");
                    string endDate = date2.ToString("yyyy-MM-dd");
                    grid.ItemsSource = Model.Relatorio.Product(startDate, endDate);
                }
                else if (periodBox.SelectedIndex == 2)
                {
                    string date = dateOne.SelectedDate.Value.ToString("yyyy-MM");
                    grid.ItemsSource = Model.Relatorio.Product(date);
                }
                else if (periodBox.SelectedIndex == 3)
                {
                    string date = dateOne.SelectedDate.Value.ToString("yyyy");
                    grid.ItemsSource = Model.Relatorio.Product(date);
                }
                else if (periodBox.SelectedIndex == 4)
                {
                    DateTime date1 = dateOne.SelectedDate.Value;
                    DateTime date2 = dateOne.SelectedDate.Value;
                    date1 = date1.AddDays(-1);
                    date2 = date2.AddDays(1);

                    string startDate = date1.ToString("yyyy-MM-dd");
                    string endDate = date2.ToString("yyyy-MM-dd");
                    grid.ItemsSource = Model.Relatorio.Product(startDate, endDate);
                }
            }
        }

    }
}
