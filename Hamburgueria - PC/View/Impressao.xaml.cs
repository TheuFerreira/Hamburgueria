﻿using System;
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

using RawPrint;
using System;
using System.Drawing.Printing;
using PdfiumViewer;

namespace Hamburgueria.View
{
    /// <summary>
    /// Lógica interna para Impressao.xaml
    /// </summary>
    public partial class Impressao : Window
    {
        public Impressao()
        {
            InitializeComponent();

            this.Loaded += Impressao_Loaded;

            this.printsList.SelectionChanged += PrintsList_SelectedIndexChanged;

            this.print.Click += Print_Click;
            this.cancel.Click += Cancel_Click;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        private void Impressao_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (string printerName in PrinterSettings.InstalledPrinters)
                printsList.Items.Add(printerName);

            Properties.Settings.Default.Reload();
            int index = Properties.Settings.Default.printIndex;
            printsList.SelectedIndex = index >= printsList.Items.Count ? 0 : index;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Print_Click(object sender, EventArgs e)
        {
            string filePath = PDF.Path() + "\\sale.pdf";
            string fileName = "Impressao-Pizzaria.pdf";
            string printerName = printsList.Text;

            RawPrinterHelper.SendFileToPrinter(filePath, printerName);

            /*
            IPrinter printer = new Printer();
            printer.PrintRawFile(printerName, filePath, fileName);*/

            this.Close();
        }

        private void PrintsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.printIndex = printsList.SelectedIndex;
            Properties.Settings.Default.Save();
        }
    }
}
