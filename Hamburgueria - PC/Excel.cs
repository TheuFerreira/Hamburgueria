using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using m_Excel = Microsoft.Office.Interop.Excel;

namespace Hamburgueria
{
    public class Excel
    {
        private m_Excel.Application app;
        private m_Excel.Workbook workbook;
        private m_Excel.Worksheet worksheet;
        private readonly object misValue = Missing.Value;

        private bool ok = false;

        public void Sales(DataGrid grid)
        {
            ok = false;

            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "Exportar para Excel";
            fileDialog.FileName = "Vendas " + DateTime.Today.ToString("yyyy-MM-dd");
            fileDialog.DefaultExt = "*.xlsx";
            fileDialog.Filter = "*.xlsx | *.xlsx";
            fileDialog.FileOk += FileDialog_FileOk;
            fileDialog.ShowDialog();

            if (ok == false)
                return;

            string fileName = fileDialog.FileName;
            try
            {
                File.Copy("Excel\\testingModel.xlsx", fileName, true);
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

                for (int i = 0; i < grid.Items.Count; i++)
                {
                    var row = (Model.Relatorio.VendaDia)grid.Items[i];
                    worksheet.Cells[i + 2, 1] = row.TYPE;
                    worksheet.Cells[i + 2, 2] = row.Date;
                    worksheet.Cells[i + 2, 3] = row.TotalBrute;
                    worksheet.Cells[i + 2, 4] = row.Discount;
                    worksheet.Cells[i + 2, 5] = row.Total;
                    worksheet.Cells[i + 2, 6] = row.Payment;
                }

                workbook.Save();
                workbook.Close(true, misValue, misValue);

                Process.Start(fileName);
            }
            catch
            {
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
