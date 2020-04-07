﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Hamburgueria
{
    class PDF
    {
        public static string Path()
        {
            string pathData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FTech";

            if (Directory.Exists(pathData) == false)
                pathData = Directory.CreateDirectory(pathData).FullName;

            return pathData;
        }

        public static void Sale(DateTime dateSale, decimal totalBrute, decimal discount, decimal totalValue, decimal valuePay, decimal change, string payment, List<View.VendasRapida.Item> products)
        {
            Paragraph title = new Paragraph("BIG BURGUER LANCHES");
            title.SetMarginTop(15);
            title.SetFontSize(10);
            title.SetTextAlignment(TextAlignment.CENTER);

            Paragraph header = new Paragraph("RUA TOCANTINS, Nº 395, MORRO DAS BICAS\nTELEFONE: ESPERANDO IGOR");
            header.SetFontSize(7);
            header.SetTextAlignment(TextAlignment.CENTER);

            Paragraph fiscal = new Paragraph("CUPOM NÃO FISCAL");
            fiscal.SetFontSize(7);
            fiscal.SetTextAlignment(TextAlignment.CENTER);
            fiscal.SetMarginTop(5);
            fiscal.SetMarginBottom(5);

            Table table = new Table(new float[] { 2, .75f, .5f, .75f }).UseAllAvailableWidth().SetFixedLayout();
            table.AddCell(CreateCell("DESC", TextAlignment.LEFT));
            table.AddCell(CreateCell("PREÇO", TextAlignment.RIGHT));
            table.AddCell(CreateCell("QTD", TextAlignment.RIGHT));
            table.AddCell(CreateCell("TOTAL", TextAlignment.RIGHT));
            foreach (View.VendasRapida.Item p in products)
            {
                table.AddCell(CreateCell(p.Name, TextAlignment.LEFT));
                table.AddCell(CreateCell(p.Price.ToString("N2"), TextAlignment.RIGHT));
                table.AddCell(CreateCell(p.Quantity.ToString(), TextAlignment.RIGHT));
                table.AddCell(CreateCell(p.Total.ToString("N2"), TextAlignment.RIGHT));
            }

            Table total = new Table(new float[] { 3.25f, .75f }).UseAllAvailableWidth().SetFixedLayout();
            total.AddCell(CreateCell("TOTAL BRUTO: R$", TextAlignment.LEFT, 6));
            total.AddCell(CreateCell(totalBrute.ToString("N2"), TextAlignment.RIGHT, 6));
            total.AddCell(CreateCell("DESCONTO: R$", TextAlignment.LEFT, 6));
            total.AddCell(CreateCell(discount.ToString("N2"), TextAlignment.RIGHT, 6));
            total.AddCell(CreateCell("TOTAL: R$", TextAlignment.LEFT));
            total.AddCell(CreateCell(totalValue.ToString("N2"), TextAlignment.RIGHT));

            if (payment == "Á VISTA")
            {
                total.AddCell(CreateCell("VALOR PAGO: R$", TextAlignment.LEFT));
                total.AddCell(CreateCell(valuePay.ToString("N2"), TextAlignment.RIGHT));
                total.AddCell(CreateCell("TROCO: R$", TextAlignment.LEFT));
                total.AddCell(CreateCell(change.ToString("N2"), TextAlignment.RIGHT));
            }

            Table others = new Table(new float[] { 1, 1 }).UseAllAvailableWidth().SetFixedLayout();
            others.AddCell(CreateCell("FORMA DE PAGAMENTO", TextAlignment.LEFT, 6));
            others.AddCell(CreateCell(payment, TextAlignment.RIGHT, 6));
            others.AddCell(CreateCell("DATA DA EMISSÃO", TextAlignment.LEFT, 6));
            others.AddCell(CreateCell(dateSale.ToString(), TextAlignment.RIGHT, 6));


            LineSeparator l1 = new LineSeparator(new DashedLine(1));
            l1.SetMargins(0, 0, 0, 0);
            LineSeparator l2 = new LineSeparator(new DashedLine(1));
            l2.SetMargins(0, 0, 0, 0);
            LineSeparator l3 = new LineSeparator(new DashedLine(1));
            l3.SetMargins(5, 0, 0, 0);


            PdfDocument pdf = new PdfDocument(new PdfWriter(/*Path() + "\\*/"sale.pdf"));
            Document doc = new Document(pdf, PageSize.A7);
            doc.SetMargins(0, 10, 0, 10);

            doc.Add(title);
            doc.Add(header);
            doc.Add(l1);
            doc.Add(fiscal);
            doc.Add(l2);
            doc.Add(table);
            doc.Add(l3);
            doc.Add(total);
            doc.Add(others);

            doc.Close();
        }

        public static void Sale(DateTime dateSale, decimal totalBrute, decimal discount, decimal totalValue, decimal valuePay, decimal change, string payment, List<View.VendasBalcao.Item> products)
        {
            Paragraph title = new Paragraph("BIG BURGUER LANCHES");
            title.SetMarginTop(15);
            title.SetFontSize(10);
            title.SetTextAlignment(TextAlignment.CENTER);

            Paragraph header = new Paragraph("RUA TOCANTINS, Nº 395, MORRO DAS BICAS\nTELEFONE: ESPERANDO IGOR");
            header.SetFontSize(7);
            header.SetTextAlignment(TextAlignment.CENTER);

            Paragraph fiscal = new Paragraph("CUPOM NÃO FISCAL");
            fiscal.SetFontSize(7);
            fiscal.SetTextAlignment(TextAlignment.CENTER);
            fiscal.SetMarginTop(5);
            fiscal.SetMarginBottom(5);

            Table table = new Table(new float[] { 2, .75f, .5f, .75f }).UseAllAvailableWidth().SetFixedLayout();
            table.AddCell(CreateCell("DESC", TextAlignment.LEFT));
            table.AddCell(CreateCell("PREÇO", TextAlignment.RIGHT));
            table.AddCell(CreateCell("QTD", TextAlignment.RIGHT));
            table.AddCell(CreateCell("TOTAL", TextAlignment.RIGHT));
            foreach (View.VendasBalcao.Item p in products)
            {
                table.AddCell(CreateCell(p.Name, TextAlignment.LEFT));
                table.AddCell(CreateCell(p.Price.ToString("N2"), TextAlignment.RIGHT));
                table.AddCell(CreateCell(p.Quantity.ToString(), TextAlignment.RIGHT));
                table.AddCell(CreateCell(p.Total.ToString("N2"), TextAlignment.RIGHT));
            }

            Table total = new Table(new float[] { 3.25f, .75f }).UseAllAvailableWidth().SetFixedLayout();
            total.AddCell(CreateCell("TOTAL BRUTO: R$", TextAlignment.LEFT, 6));
            total.AddCell(CreateCell(totalBrute.ToString("N2"), TextAlignment.RIGHT, 6));
            total.AddCell(CreateCell("DESCONTO: R$", TextAlignment.LEFT, 6));
            total.AddCell(CreateCell(discount.ToString("N2"), TextAlignment.RIGHT, 6));
            total.AddCell(CreateCell("TOTAL: R$", TextAlignment.LEFT));
            total.AddCell(CreateCell(totalValue.ToString("N2"), TextAlignment.RIGHT));

            if (payment == "Á VISTA")
            {
                total.AddCell(CreateCell("VALOR PAGO: R$", TextAlignment.LEFT));
                total.AddCell(CreateCell(valuePay.ToString("N2"), TextAlignment.RIGHT));
                total.AddCell(CreateCell("TROCO: R$", TextAlignment.LEFT));
                total.AddCell(CreateCell(change.ToString("N2"), TextAlignment.RIGHT));
            }

            Table others = new Table(new float[] { 1, 1 }).UseAllAvailableWidth().SetFixedLayout();
            others.AddCell(CreateCell("FORMA DE PAGAMENTO", TextAlignment.LEFT, 6));
            others.AddCell(CreateCell(payment, TextAlignment.RIGHT, 6));
            others.AddCell(CreateCell("DATA DA EMISSÃO", TextAlignment.LEFT, 6));
            others.AddCell(CreateCell(dateSale.ToString(), TextAlignment.RIGHT, 6));


            LineSeparator l1 = new LineSeparator(new DashedLine(1));
            l1.SetMargins(0, 0, 0, 0);
            LineSeparator l2 = new LineSeparator(new DashedLine(1));
            l2.SetMargins(0, 0, 0, 0);
            LineSeparator l3 = new LineSeparator(new DashedLine(1));
            l3.SetMargins(5, 0, 0, 0);


            PdfDocument pdf = new PdfDocument(new PdfWriter(/*Path() + "\\*/"sale.pdf"));
            Document doc = new Document(pdf, PageSize.A7);
            doc.SetMargins(0, 10, 0, 10);

            doc.Add(title);
            doc.Add(header);
            doc.Add(l1);
            doc.Add(fiscal);
            doc.Add(l2);
            doc.Add(table);
            doc.Add(l3);
            doc.Add(total);
            doc.Add(others);

            doc.Close();
        }

        /*
        public static void Test()
        {
            Paragraph title = new Paragraph("PIZZARIA FAMÍLIAS");
            title.SetMarginTop(15);
            title.SetFontSize(10);
            title.SetTextAlignment(TextAlignment.CENTER);

            PdfDocument pdf = new PdfDocument(new PdfWriter(Path() + "\\test.pdf"));
            Document doc = new Document(pdf, PageSize.A7);
            doc.SetMargins(0, 10, 0, 10);

            doc.Add(title);

            doc.Close();
        }
        
        public static void Sale(List<Item.SaleProduct> products, decimal totalBrute, decimal discount, decimal rate, decimal totalValue, string payment, DateTime dateSale, string name, string address, string telephone, string reference)
        {
            Paragraph title = new Paragraph("PIZZARIA FAMÍLIAS");
            title.SetMarginTop(15);
            title.SetFontSize(10);
            title.SetTextAlignment(TextAlignment.CENTER);

            Paragraph header = new Paragraph("RUA PALMIRO BIM, Nº 435, PARQUE DOS PINUS\nTELEFONE: (16)98219-0702");
            header.SetFontSize(7);
            header.SetTextAlignment(TextAlignment.CENTER);

            Paragraph fiscal = new Paragraph("CUPOM NÃO FISCAL");
            fiscal.SetFontSize(7);
            fiscal.SetTextAlignment(TextAlignment.CENTER);
            fiscal.SetMarginTop(5);
            fiscal.SetMarginBottom(5);

            Table table = new Table(new float[] { 2, .75f, .5f, .75f }).UseAllAvailableWidth().SetFixedLayout();
            table.AddCell(CreateCell("DESC", TextAlignment.LEFT));
            table.AddCell(CreateCell("PREÇO", TextAlignment.RIGHT));
            table.AddCell(CreateCell("QTD", TextAlignment.RIGHT));
            table.AddCell(CreateCell("TOTAL", TextAlignment.RIGHT));
            foreach (Item.SaleProduct p in products)
            {
                table.AddCell(CreateCell(p.Name, TextAlignment.LEFT));
                table.AddCell(CreateCell(p.Price.ToString(), TextAlignment.RIGHT));
                table.AddCell(CreateCell(p.Quantity.ToString(), TextAlignment.RIGHT));
                table.AddCell(CreateCell(p.Total.ToString(), TextAlignment.RIGHT));
            }

            Table total = new Table(new float[] { 3.25f, .75f }).UseAllAvailableWidth().SetFixedLayout();
            total.AddCell(CreateCell("TOTAL BRUTO: R$", TextAlignment.LEFT, 6));
            total.AddCell(CreateCell(totalBrute.ToString("N2"), TextAlignment.RIGHT, 6));
            total.AddCell(CreateCell("DESCONTO: R$", TextAlignment.LEFT, 6));
            total.AddCell(CreateCell(discount.ToString("N2"), TextAlignment.RIGHT, 6));
            total.AddCell(CreateCell("TAXA: R$", TextAlignment.LEFT, 6));
            total.AddCell(CreateCell(rate.ToString("N2"), TextAlignment.RIGHT, 6));
            total.AddCell(CreateCell("TOTAL: R$", TextAlignment.LEFT));
            total.AddCell(CreateCell(totalValue.ToString("N2"), TextAlignment.RIGHT));

            Table others = new Table(new float[] { 1, 1 }).UseAllAvailableWidth().SetFixedLayout();
            others.AddCell(CreateCell("FORMA DE PAGAMENTO", TextAlignment.LEFT, 6));
            others.AddCell(CreateCell(payment, TextAlignment.RIGHT, 6));
            others.AddCell(CreateCell("DATA DA EMISSÃO", TextAlignment.LEFT, 6));
            others.AddCell(CreateCell(dateSale.ToString(), TextAlignment.RIGHT, 6));


            Paragraph client = new Paragraph("CLIENTE: " + name + "\nTELEFONE:" + telephone + "\n" + address + "\n" + (string.IsNullOrWhiteSpace(reference) ? "" : reference));
            client.SetFontSize(6);
            client.SetTextAlignment(TextAlignment.LEFT);


            Paragraph ftech = new Paragraph("FTech - Inovações Tecnológicas\n+55 33 99999-9999");
            ftech.SetFontSize(6);
            ftech.SetTextAlignment(TextAlignment.CENTER);
            ftech.SetMarginTop(5);
            ftech.SetMarginBottom(5);

            LineSeparator l1 = new LineSeparator(new DashedLine(1));
            l1.SetMargins(0, 0, 0, 0);
            LineSeparator l2 = new LineSeparator(new DashedLine(1));
            l2.SetMargins(0, 0, 0, 0);
            LineSeparator l3 = new LineSeparator(new DashedLine(1));
            l3.SetMargins(5, 0, 0, 0);
            LineSeparator l4 = new LineSeparator(new DashedLine(1));
            l4.SetMargins(0, 0, 5, 0);


            PdfDocument pdf = new PdfDocument(new PdfWriter(Path() + "\\sale.pdf"));
            Document doc = new Document(pdf, PageSize.A7);
            doc.SetMargins(0, 10, 0, 10);

            doc.Add(title);
            doc.Add(header);
            doc.Add(l1);
            doc.Add(fiscal);
            doc.Add(l2);
            doc.Add(table);
            doc.Add(l3);
            doc.Add(total);
            doc.Add(others);
            doc.Add(l3);
            doc.Add(client);
            doc.Add(l4);
            doc.Add(ftech);

            doc.Close();
        }
        */
        private static Cell CreateCell(string content, TextAlignment textAlignment, float fontSize = 7)
        {
            Cell cell = new Cell(1, 1).Add(new Paragraph(content));
            cell.SetPaddingBottom(-1);
            cell.SetTextAlignment(textAlignment);
            cell.SetBorder(Border.NO_BORDER);
            cell.SetFontSize(fontSize);
            return cell;
        }
    }
}
