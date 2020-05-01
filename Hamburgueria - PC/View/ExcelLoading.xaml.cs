using Hamburgueria.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using m_Excel = Microsoft.Office.Interop.Excel;

namespace Hamburgueria.View
{
    /// <summary>
    /// Lógica interna para ExcelLoading.xaml
    /// </summary>
    public partial class ExcelLoading : Window
    {
        private readonly string date;

        private int value = 0;

        public ExcelLoading(string date)
        {
            InitializeComponent();

            DispatcherTimer timer;
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new System.TimeSpan(0, 0, 1);
            timer.Start();

            this.date = date;
            this.Loaded += ExcelLoading_Loaded;
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            loadingBar.Value = value;
        }

        private void ExcelLoading_Loaded(object sender, RoutedEventArgs e)
        {
            loadingBar.Maximum = 8;
            Task.Factory.StartNew(() => SaleDay(date));
        }

        private m_Excel.Application app;
        private m_Excel.Workbook workbook;
        private m_Excel.Worksheet worksheet;
        private readonly object misValue = Missing.Value;

        private bool ok = false;

        private void SaleDay(string date)
        {
            ok = false;

            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "Exportar para Excel";
            fileDialog.FileName = "Vendas Diárias - " + date;
            fileDialog.DefaultExt = "*.xlsx";
            fileDialog.Filter = "*.xlsx | *.xlsx";
            fileDialog.FileOk += FileDialog_FileOk;
            fileDialog.ShowDialog();

            if (ok == false)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => this.Close()));
                return;
            }

            string fileName = fileDialog.FileName;
            try
            {
                File.Copy("Excel\\vendasDiarias.xlsx", fileName, true);
                value += 1;
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                app = new m_Excel.Application();
                workbook = app.Workbooks.Open(fileName);
                worksheet = workbook.Worksheets[1];

                List<Relatorio.VendaDia> all = Relatorio.SaleDay(date);
                value += 1;
                for (int i = 0; i < all.Count; i++)
                {
                    worksheet.Cells[i + 3, 1] = all[i].TYPE;
                    worksheet.Cells[i + 3, 2] = all[i].Date;
                    worksheet.Cells[i + 3, 3] = all[i].TotalBrute;
                    worksheet.Cells[i + 3, 4] = all[i].Discount;
                    worksheet.Cells[i + 3, 5] = all[i].Total;
                    worksheet.Cells[i + 3, 6] = all[i].Payment;
                }
                value += 1;

                worksheet = workbook.Worksheets[2];
                List<Relatorio.VendaDia> delivery = Relatorio.SaleDayDelivery(date);
                value += 1;
                for (int i = 0; i < delivery.Count; i++)
                {
                    worksheet.Cells[i + 3, 1] = delivery[i].TYPE;
                    worksheet.Cells[i + 3, 2] = delivery[i].Date;
                    worksheet.Cells[i + 3, 3] = delivery[i].TotalBrute;
                    worksheet.Cells[i + 3, 4] = delivery[i].Discount;
                    worksheet.Cells[i + 3, 5] = delivery[i].Total;
                    worksheet.Cells[i + 3, 6] = delivery[i].Payment;
                }
                value += 1;

                worksheet = workbook.Worksheets[3];
                List<Relatorio.VendaDia> local = Relatorio.SaleDayLocal(date);
                value += 1;
                for (int i = 0; i < local.Count; i++)
                {
                    worksheet.Cells[i + 3, 1] = local[i].TYPE;
                    worksheet.Cells[i + 3, 2] = local[i].Date;
                    worksheet.Cells[i + 3, 3] = local[i].TotalBrute;
                    worksheet.Cells[i + 3, 4] = local[i].Discount;
                    worksheet.Cells[i + 3, 5] = local[i].Total;
                    worksheet.Cells[i + 3, 6] = local[i].Payment;
                }
                value += 1;

                workbook.Save();
                workbook.Close(true, misValue, misValue);
                value += 1;

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => this.Close()));

                Process.Start(fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                app.Quit();
            }
        }

        private void FileDialog_FileOk(object sender, CancelEventArgs e)
        {
            ok = true;
        }

    }
}
