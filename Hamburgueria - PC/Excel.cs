using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using m_Excel = Microsoft.Office.Interop.Excel;

namespace Hamburgueria
{
    public class Excel
    {
        private m_Excel.Application app;
        private m_Excel.Workbook workbook;
        private m_Excel.Worksheet worksheet;
        private readonly object misValue = Missing.Value;

        public void Sales()
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "Exportar para Excel";
            fileDialog.FileName = "Vendas " + DateTime.Today.ToString("yyyy-MM-dd");
            fileDialog.DefaultExt = "*.xlsx";
            fileDialog.Filter = "*.xlsx | *.xlsx";
            fileDialog.ShowDialog();

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

                for (int i = 0; i < 50; i++)
                {
                    for (int j = 0; j < 15; j++)
                    {
                        worksheet.Cells[i + 1, j + 1] = i + j;
                    }
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
    }
}
